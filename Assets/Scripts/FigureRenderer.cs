using UnityEngine;
using UnityEngine.UI;

public class FigureRenderer : Graphic
{ 
    [Space]
    [SerializeField] private Figure figure; 
    [SerializeField] private float thickness; 
    [SerializeField] private Color32 lineColor;

    private Vector2 corner;
    private float distance;
    private float cellSize;
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        var r = GetPixelAdjustedRect();
        corner = new Vector2(r.x,r.y);
        distance = Mathf.Sqrt((thickness*thickness)/2);
        cellSize = figure.CellSize;

        int count = 0;
        foreach (var cellsIndex in figure.DrawCellsIndices)
        {
            DrawCell(cellsIndex.x,cellsIndex.y,count,vh);
            count++;
        }
    }

    private void DrawCell(int x, int y, int index, VertexHelper vh)
    {
        float xPos = cellSize * x;
        float yPos = cellSize * y;
        
        vh.AddVert(new Vector3(corner.x+xPos, corner.y+yPos), lineColor, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos, corner.y+yPos+cellSize), lineColor,  Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+cellSize, corner.y+yPos+cellSize), lineColor,  Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+cellSize, corner.y+yPos), lineColor,  Vector2.zero);
        
        vh.AddVert(new Vector3(corner.x+xPos+distance,corner.y+yPos+distance),color, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+distance,corner.y+yPos+(cellSize-distance)),color, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+(cellSize-distance),corner.y+yPos+(cellSize-distance)),color, Vector2.zero);
        vh.AddVert(new Vector3(corner.x+xPos+(cellSize-distance),corner.y+yPos+distance),color, Vector2.zero);

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
}
