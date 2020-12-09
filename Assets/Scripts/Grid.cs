using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private FiguresPool figuresPool;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GridRenderer gridRenderer;
    
    [Space]
    [SerializeField] private int cellSize = 100;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(3,3);

    public int CellSize => cellSize;
    public Vector2Int GridSize => gridSize;
    
    public HashSet<(int,int)> ClosestPoints { get; private set; }
    public HashSet<(int,int)> SetPoints { get; private set; }
    
    private Dictionary<(int,int),Vector2> gridPoints;
    private Bounds gridBounds;

    private  void OnValidate()
    {
        var r = rectTransform.rect;
        if(Math.Abs(r.width - gridSize.x*cellSize) > Mathf.Epsilon)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,gridSize.x*cellSize);
        if(Math.Abs(r.height - gridSize.y*cellSize) > Mathf.Epsilon)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,gridSize.y*cellSize);
    }

    private void Awake()
    {
        ClosestPoints = new HashSet<(int, int)>();
        SetPoints = new HashSet<(int, int)>();
        gridPoints = new Dictionary<(int, int), Vector2>();
    }

    private void Start()
    {
        var r = rectTransform.rect;
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var point = new Vector2(r.x+x*cellSize+cellSize/2,r.y+y*cellSize+cellSize/2);
                gridPoints.Add((x,y),transform.TransformPoint(point));
            }
        }

        gridBounds = new Bounds();
        rectTransform.GetBounds(ref gridBounds);
    }

    private void OnEnable()
    {
        FigureMover.OnFigureDrag += UpdateGridDetectionOnDrag;
        FigureMover.OnFigureDragEnded += UpdateGridDetectionOnDragEnd;
    }
    
    private void OnDisable()
    {
        FigureMover.OnFigureDrag -= UpdateGridDetectionOnDrag;
        FigureMover.OnFigureDragEnded -= UpdateGridDetectionOnDragEnd;
    }

    private void UpdateGridDetectionOnDrag(Figure figure)
    {
        var figureBounds = figure.Bounds;
        //shrink bounds of figure to it's center points to easier stack it on grid edges
        figureBounds.Expand(new Vector3(-1,-1,0)*cellSize);
        
        if (gridBounds.Contains(figureBounds.min) && gridBounds.Contains(figureBounds.max))
        {
            ClosestPoints.Clear();

            foreach (var figurePoint in figure.LocalFigurePoints)
            {
                float minDistance = 1000;
                (int x, int y) closestPoint = (0,0);
                Vector2 worldFigurePoint = figure.transform.TransformPoint(figurePoint);
                    
                foreach (var gridPoint in gridPoints)
                {
                    var distance = (worldFigurePoint - gridPoint.Value).magnitude;
                    if ( distance < minDistance)
                    {
                        closestPoint = gridPoint.Key;
                        minDistance = distance;
                    }
                }
                    
                ClosestPoints.Add(closestPoint);
            }

            if (SetPoints.Overlaps(ClosestPoints))
                ClosestPoints.Clear();
            gridRenderer.SetVerticesDirty();
        }
        else if (ClosestPoints.Count>0)
        {
            ClosestPoints.Clear();
            gridRenderer.SetVerticesDirty();
        }
    }
    
    private void UpdateGridDetectionOnDragEnd(Figure figure)
    { 
        if (ClosestPoints.Count>0)
        {
            SetPoints.UnionWith(ClosestPoints);
            ClosestPoints.Clear();
            gridRenderer.SetVerticesDirty();
            
            figuresPool.DisposeFigure(figure);
        }
    }
}
