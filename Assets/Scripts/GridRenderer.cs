using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridRenderer : Graphic
{
    [Space]
    [SerializeField] private Grid grid;
    [SerializeField] private float thickness = 2;
    
    [Space]
    [SerializeField] private Color32 backGround1;
    [SerializeField] private Color32 backGround2;
    
    [Space]
    [SerializeField] private Color32 detectionBackgroundColor;
    [SerializeField] private Color32 setBackgroundColor;
    [SerializeField] private Color32 setComboBackgroundColor;

    private Vector2 corner;
    private float distance;
    private float cellSize;
    private int offset;
    
    private HashSet<(int x, int y)> comboCells = new HashSet<(int x, int y)>();
    private Color32 comboBgColor;
    private float comboDistance;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        var r = GetPixelAdjustedRect();
        corner = new Vector2(r.x,r.y);
        distance = Mathf.Sqrt((thickness*thickness)/2);
        cellSize = grid.CellSize;
        
        offset = 0;
        for (int y = 0; y < grid.GridSize.y; y++)
        {
            for (int x = 0; x < grid.GridSize.x; x++)
            {
                DrawCell(x,y,vh);
            }
        }
    }

    //todo: expose bg coloring settings
    private int diffStartingX = 3;
    private int diffWidth = 3;
    
    private int diffStartingY = 3;
    private int diffHeight = 3;
    

    private void DrawCell(int x, int y, VertexHelper vh)
    {
        bool diffColorX = x >= diffStartingX && x < diffStartingX + diffWidth;
        bool diffColorY = y >= diffStartingY && y < diffStartingY + diffHeight;
        Color32 bgColor = (diffColorX && !diffColorY) || (!diffColorX && diffColorY)? backGround2 : backGround1;
        
        if (grid.ClosestPoints != null && grid.ClosestPoints.Contains((x, y)))
            bgColor = detectionBackgroundColor;
        else if (grid.ComboHighlights!=null && grid.ComboHighlights.Contains((x,y)))
            bgColor = setComboBackgroundColor;
        else if (grid.SetPoints != null && grid.SetPoints.Contains((x, y)))
            bgColor = setBackgroundColor;

        float xPos = cellSize * x;
        float yPos = cellSize * y;
        
        vh.AddVert(new Vector3(corner.x+xPos, corner.y+yPos), color, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos, corner.y+yPos+cellSize), color,  Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+cellSize, corner.y+yPos+cellSize), color,  Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+cellSize, corner.y+yPos), color,  Vector2.zero);
        
        vh.AddVert(new Vector3(corner.x+xPos+distance,corner.y+yPos+distance),bgColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+distance,corner.y+yPos+(cellSize-distance)),bgColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+(cellSize-distance),corner.y+yPos+(cellSize-distance)),bgColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+(cellSize-distance),corner.y+yPos+distance),bgColor, Vector2.zero);

        if (comboCells.Contains((x, y)))
        {
            vh.AddVert(new Vector3(corner.x+xPos+comboDistance,corner.y+yPos+comboDistance),comboBgColor, Vector2.zero);
            vh.AddVert(new Vector3(corner.x+xPos+comboDistance,corner.y+yPos+(cellSize-comboDistance)),comboBgColor, Vector2.zero);
            vh.AddVert(new Vector3(corner.x+xPos+(cellSize-comboDistance),corner.y+yPos+(cellSize-comboDistance)),comboBgColor, Vector2.zero);
            vh.AddVert(new Vector3(corner.x+xPos+(cellSize-comboDistance),corner.y+yPos+comboDistance),comboBgColor, Vector2.zero);
        }
        
        vh.AddTriangle(offset+0,offset+1,offset+5);
        vh.AddTriangle(offset+5,offset+4,offset+0);
        
        vh.AddTriangle(offset+1,offset+2,offset+6);
        vh.AddTriangle(offset+6,offset+5,offset+1);
        
        vh.AddTriangle(offset+2,offset+3,offset+7);
        vh.AddTriangle(offset+7,offset+6,offset+2);
        
        vh.AddTriangle(offset+3,offset+0,offset+4);
        vh.AddTriangle(offset+4,offset+7,offset+3);
        
        vh.AddTriangle(offset+4,offset+5,offset+6);
        vh.AddTriangle(offset+6,offset+7,offset+4);

        if (comboCells.Contains((x, y)))
        {
            vh.AddTriangle(offset + 8, offset + 9, offset + 10);
            vh.AddTriangle(offset + 10, offset + 11, offset + 8);
            offset += 12;
        }
        else
            offset += 8;
    }
    
    public void PlayComboAnimation(HashSet<(int x, int y)> comboCells)
    {
        if (comboCells.Count == 0)
        {
            SetVerticesDirty();
            return;
        }

        this.comboCells.UnionWith(comboCells);
        comboBgColor = setBackgroundColor;
        comboDistance = Mathf.Sqrt((thickness*thickness)/2);
        
        StartCoroutine(DissolveAnimation());
    }

    private IEnumerator DissolveAnimation()
    {
        float colorChangeDuration = 0.15f;
        float dissolveDuration = 0.25f;

        float startingDistance = Mathf.Sqrt((thickness*thickness)/2);

        float t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / colorChangeDuration;
            comboBgColor = Color.Lerp(setBackgroundColor,color,t);
            SetVerticesDirty();
            yield return null;
        }
        
        t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / dissolveDuration;
            comboDistance = Mathf.Lerp(startingDistance, cellSize / 2, t);
            SetVerticesDirty();
            yield return null;
        }
        
        comboCells.Clear();
        SetVerticesDirty();
    }
}
