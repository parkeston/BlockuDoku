using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    
    [Space]
    [SerializeField] private int cellSize = 100;
    [SerializeField] private List<Vector2Int> drawCellsIndices;

    public int CellSize => cellSize;
    public List<Vector2> LocalFigurePoints { get; private set; }
    public List<Vector2Int> DrawCellsIndices => drawCellsIndices;

    private Bounds bounds;
    public Bounds Bounds
    {
        get
        {
            var rect = rectTransform.rect;
            if (bounds == default)
                bounds = new Bounds(transform.TransformPoint(rect.center),rect.size);
            else
            {
                bounds.center = transform.TransformPoint(rect.center);
                bounds.size = rect.size;
            }

            return bounds;
        }
    }

    private void OnValidate()
    {
        float desiredWidth = (drawCellsIndices.Max(cell => cell.x) +1)* cellSize;
        float desiredHeight = (drawCellsIndices.Max(cell => cell.y) +1) * cellSize;
        
        var r = rectTransform.rect;
        if(Math.Abs(r.width - desiredWidth) > Mathf.Epsilon)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,desiredWidth);
        if(Math.Abs(r.height -desiredHeight) > Mathf.Epsilon)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,desiredHeight);
    }
    
    private void Awake()
    {
        LocalFigurePoints = new List<Vector2>();
        var r = rectTransform.rect;

        foreach (var drawCellsIndex in drawCellsIndices)
        {
            var point = new Vector2(r.x + drawCellsIndex.x * cellSize + cellSize / 2, r.y + drawCellsIndex.y * cellSize + cellSize / 2);
            LocalFigurePoints.Add(point);
        }
    }
}
