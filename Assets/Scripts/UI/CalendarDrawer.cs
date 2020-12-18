using System;
using System.Globalization;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CalendarDrawer : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private RectTransform calendarPointer;
    
    [Space]
    [SerializeField] private Color32 dayNamesColor;
    [SerializeField] private Color32 dayNumbersColor;
    [SerializeField] private Color32 currentDayColor;

    [Space] 
    [SerializeField] private float dayNamesScale;
    [SerializeField] private float dayNumbersScale;

    private Vector2 cellSize;
    private Vector2[] cellPositions;
    
    private TMP_TextInfo textInfo;
    private TMP_MeshInfo[] cachedMeshInfo;

    private int firstDayIndexOffset;
    private int selectedDayCellIndex=-1;
    private int currentDayCellIndex;

    private void Awake()
    {
       var rect = textComponent.rectTransform.rect;

       cellSize = new Vector2(rect.width/7,rect.height/7); //7 days of week, 7 rows including header
       cellPositions = new Vector2[49]; //7*6 = 42 cells in calendar grid
       
       int i = 0;
       for (int y = 6; y >= 0; y--)
       {
           for (int x = 0; x < 7; x++)
           {
               var point = new Vector2(rect.x+x*cellSize.x+cellSize.x/2,rect.y+y*cellSize.y+cellSize.y/2);
               cellPositions[i] = point;
               i++;
           }
       }
    }

    public void DrawMonth(DateTime firstMonthDate, int currentDayInMonth)
    {
        int days = DateTime.DaysInMonth(firstMonthDate.Year, firstMonthDate.Month);
        firstDayIndexOffset = (int) firstMonthDate.DayOfWeek; //index offset in grid for first day in month

        StringBuilder stringBuilder = new StringBuilder();

        var dtFormat = CultureInfo.CurrentCulture.DateTimeFormat; //add header day names
        for (int i = 0; i < 7; i++)
        {
            stringBuilder.Append(dtFormat.AbbreviatedDayNames[i]);
            stringBuilder.Append(' ');
        }

        for (int i = 1; i <= days; i++)
        {
            stringBuilder.Append(i.ToString());
            stringBuilder.Append(' ');
        }

        textComponent.text = stringBuilder.ToString();
        BuildCalendarLayout(currentDayInMonth);
    }
    
    private void  BuildCalendarLayout( int currentDay)
    {
        textComponent.ForceMeshUpdate();
        textInfo = textComponent.textInfo;
        // Cache the vertex data of the text object as the fx is applied to the original position of the characters.
        cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
        
        int cellIndex = 0;
        for (int i = 0; i < 7; i++)
        {
            UpdateWordData(cellIndex,i,dayNamesColor,dayNamesScale);
            cellIndex++;
        }

        int dayIndex = 0;
        cellIndex += firstDayIndexOffset;
        for (int i = 7; i < textInfo.wordCount; i++)
        {
            if (dayIndex + 1 == currentDay)
            {
                SetSelectedDay(cellIndex);
                currentDayCellIndex = cellIndex;
            }
            else
            {
                var currentColor = dayIndex+1 > currentDay ? dayNamesColor : dayNumbersColor;
                UpdateWordData(cellIndex,i,currentColor,dayNumbersScale);
            }
            
            cellIndex++;
            dayIndex++;
        }
        
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32 | TMP_VertexDataUpdateFlags.Vertices);
    }
    
    private void SetSelectedDay(int cellIndex)
    {
        if (selectedDayCellIndex>=0) //unselect previous selected day
            UpdateWordData(selectedDayCellIndex,selectedDayCellIndex-firstDayIndexOffset,dayNumbersColor,dayNumbersScale);
        selectedDayCellIndex = cellIndex;
        
        calendarPointer.transform.position = transform.TransformPoint(cellPositions[cellIndex]);
        int i = cellIndex - firstDayIndexOffset;
        UpdateWordData(cellIndex,i,currentDayColor,dayNumbersScale);
    }

    private void UpdateWordData(int cellIndex, int i,Color32 currentColor, float currentScale)
    {
        Vector2 charPosition = cellPositions[cellIndex];

        var wordInfo = textInfo.wordInfo[i];
        var xLeft = textInfo.characterInfo[wordInfo.firstCharacterIndex].bottomLeft.x;
        var xRight = textInfo.characterInfo[wordInfo.lastCharacterIndex].bottomRight.x;
        var wordXCenter = (xLeft + xRight) / 2;

        for (int j = 0; j < wordInfo.characterCount; j++)
        {
            UpdateCharacterData(wordInfo.firstCharacterIndex+j,charPosition,wordXCenter,currentColor,currentScale);
        }
    }

    private void UpdateCharacterData( int charIndex, Vector2 position, float wordXCenter, Color32 color,float scale)
    {
        TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
        int materialIndex = charInfo.materialReferenceIndex;
        int vertexIndex =charInfo.vertexIndex;
        Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

        // Determine the center point of each character at the midLine.
        Vector2 charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, (charInfo.ascender+charInfo.descender)/2);
        
        // Need to translate all 4 vertices of each quad to aligned with middle of character / baseline.
        // This is needed so the matrix TRS is applied at the origin for each character.
        Vector3 offset = charMidBasline;

        Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

        destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
        destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
        destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
        destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

        position += Vector2.right * (charMidBasline.x - wordXCenter)*scale;
        var matrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one*scale);
        
        destinationVertices[vertexIndex + 0] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
        destinationVertices[vertexIndex + 1] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
        destinationVertices[vertexIndex + 2] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
        destinationVertices[vertexIndex + 3] = matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);
        
        var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
        newVertexColors[vertexIndex + 0] = color;
        newVertexColors[vertexIndex + 1] = color;
        newVertexColors[vertexIndex + 2] = color;
        newVertexColors[vertexIndex + 3] = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int closestY = 0;
        int closestX = 0;

        float minDistance = 10000;
        
        for (int y = 0; y < 7; y++)
        {
            int cellIndex = y * 7;//first cell of each row
            var distance = (eventData.position - (Vector2) transform.TransformPoint(cellPositions[cellIndex])).y;
            distance = Mathf.Abs(distance);
            if ( distance< minDistance)
            {
                minDistance = distance;
                closestY = y;
            }
        }
        

        minDistance = 1000;
        for (int x = 0; x < 7; x++)
        {
            int cellIndex = x;
            var distance = (eventData.position - (Vector2) transform.TransformPoint(cellPositions[cellIndex])).x;
            distance = Mathf.Abs(distance);
            if ( distance< minDistance)
            {
                minDistance = distance;
                closestX = x;
            }
        }

        int closestCell = closestY * 7 + closestX;
        if(closestCell<7+firstDayIndexOffset || closestCell>currentDayCellIndex)
            return;
        
        SetSelectedDay(closestCell);
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
}
