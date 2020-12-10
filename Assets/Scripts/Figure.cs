using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Figure : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private FigureRenderer figureRenderer;
    
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
            if (bounds == default)
                bounds = new Bounds();
            rectTransform.GetBounds(ref bounds);

            return bounds;
        }
    }

    private bool interactable;

    public bool Interactable
    {
        get => interactable;
        set
        {
            interactable = value;
            figureRenderer.color = value ? Color.white : new Color(1, 1, 1, 0.7f);
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
        Interactable = true;
    }

    private void Start()
    {
        var r = rectTransform.rect;

        foreach (var drawCellsIndex in drawCellsIndices)
        {
            var point = new Vector2(r.x + drawCellsIndex.x * cellSize + cellSize / 2, r.y + drawCellsIndex.y * cellSize + cellSize / 2);
            LocalFigurePoints.Add(point);
        }
    }
}
