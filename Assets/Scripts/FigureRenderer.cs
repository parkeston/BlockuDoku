using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureRenderer : Graphic
{ 
    [Space]
    [SerializeField] private Figure figure; 
    [SerializeField] private float thickness; 
    
    [Space]
    [SerializeField] private Color32 lineColor;
    [SerializeField] private Color32 bgColor;

    private Vector2 corner;
    private float distance;
    private int figureCellSize;
    private int currentCellSize;
    private float cellInsetPercentage = 1f;

    private Color32 tintedBgColor;
    private Color32 tintedLineColor;
    
    private List<Vector2> cellsRandom = new List<Vector2>();

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        var r = GetPixelAdjustedRect();
        corner = new Vector2(r.x,r.y);
        distance = Mathf.Sqrt((thickness*thickness)/2);
        figureCellSize = figure.CellSize;
        currentCellSize = (int)(figureCellSize * cellInsetPercentage);

        tintedBgColor = bgColor * color;
        tintedLineColor = lineColor * color;

        int count = 0;
        bool revealing = cellsRandom.Count > 0;
        for(int i=0;i<figure.DrawCellsIndices.Count;i++)
        {
            var cell = revealing ? cellsRandom[i] : figure.DrawCellsIndices[i];
            DrawCell(cell.x,cell.y,count,vh);
            count++;
        }
    }

    private void DrawCell(float x, float y, int index, VertexHelper vh)
    {
        float xPos = figureCellSize * x + (figureCellSize - currentCellSize) / 2f;
        float yPos = figureCellSize * y + (figureCellSize - currentCellSize) / 2f;

        vh.AddVert(new Vector3(corner.x+xPos, corner.y+yPos), tintedLineColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos, corner.y+yPos+currentCellSize), tintedLineColor,  Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+currentCellSize, corner.y+yPos+currentCellSize), tintedLineColor,  Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+currentCellSize, corner.y+yPos), tintedLineColor,  Vector2.zero);
        
        vh.AddVert(new Vector3(corner.x+xPos+distance,corner.y+yPos+distance),tintedBgColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+distance,corner.y+yPos+(currentCellSize-distance)),tintedBgColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+(currentCellSize-distance),corner.y+yPos+(currentCellSize-distance)),tintedBgColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+(currentCellSize-distance),corner.y+yPos+distance),tintedBgColor, Vector2.zero);

        int offset = index * 8; //each cell has 8 vertices
        
        vh.AddTriangle(offset+0,offset+1,offset+5);
        vh.AddTriangle(offset+5,offset+4,offset+0);
        
        vh.AddTriangle(offset+1,offset+2,offset+6);
        vh.AddTriangle(offset+6,offset+5,offset+1);
        
        vh.AddTriangle(offset+2,offset+3,offset+7);
        vh.AddTriangle(offset+7,offset+6,offset+2);
        
        vh.AddTriangle(offset+3,offset+0,offset+4);
        vh.AddTriangle(offset+4,offset+7,offset+3);
        
        //cell bg mesh, add additional vertices for nor vertex color blending? (blending gives antialiasing effect?)
        vh.AddTriangle(offset+4,offset+5,offset+6);
        vh.AddTriangle(offset+6,offset+7,offset+4);
    }

    public void SetInset(float insetPercentage)
    {
        cellInsetPercentage = Mathf.Clamp01(insetPercentage);
        SetVerticesDirty();
    }

    
    public void PlayRevealAnimation()
    {
        cellsRandom.Clear();
        for(int i=0;i<figure.DrawCellsIndices.Count;i++)
            cellsRandom.Add(new Vector2(Random.Range(0,3),Random.Range(0,3)));

        StartCoroutine(RevealAnimation(0.5f));
    }

    private IEnumerator RevealAnimation(float duration)
    {
        float t = 0;
        while (t<=1)
        {
            for (int i = 0; i < cellsRandom.Count; i++)
            {
                cellsRandom[i] = Vector2.Lerp(cellsRandom[i],figure.DrawCellsIndices[i],t);
            }

            t += Time.deltaTime * 1 / duration;
            
            SetVerticesDirty();
            yield return null;
        }
        
        cellsRandom.Clear();
        SetVerticesDirty();
    }
}
