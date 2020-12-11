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

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        var r = GetPixelAdjustedRect();
        corner = new Vector2(r.x,r.y);
        distance = Mathf.Sqrt((thickness*thickness)/2);
        cellSize = grid.CellSize;
        
        int count = 0;
        for (int y = 0; y < grid.GridSize.y; y++)
        {
            for (int x = 0; x < grid.GridSize.x; x++)
            {
                DrawCell(x,y,count,vh);
                count++;
            }
        }
    }

    //todo: expose bg coloring settings
    private int diffStartingX = 3;
    private int diffWidth = 3;
    
    private int diffStartingY = 3;
    private int diffHeight = 3;
    
    //todo: fix diff edge line thickness
    //todo: resolve color blending between line & bg colors (remove or not blending?)
    //todo: add different line colors per cells, like diff bg colors
    //todo: maybe persist line thickness on diff resolutions?
    //todo: animations & effects

    private void DrawCell(int x, int y, int index, VertexHelper vh)
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
