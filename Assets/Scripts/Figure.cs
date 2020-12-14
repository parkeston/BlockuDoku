using System;
using System.Collections;
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

    private void OnEnable()
    {
        transform.localScale = Vector3.one * 0.7f;
        figureRenderer.SetInset(1);
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

    public void MoveToGridClosestPoints(Vector3 point, Action onComplete) =>
        StartCoroutine(TranslateToPoint(point, 0.1f,onComplete));

    private IEnumerator TranslateToPoint(Vector3 point, float duration,Action onComplete)
    {
        Vector3 startingPosition = transform.position;
        
        float t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / duration;
            transform.position = Vector3.Lerp(startingPosition, point, t);
            yield return null;
        }
        
        onComplete?.Invoke();
    }


    public void Expand() => StartCoroutine(ScaleAnimation(0.7f,1.1f,1f,
        1f,0.8f));
    public void Shrink() => StartCoroutine(ScaleAnimation(1,0.6f,0.7f,
        0.8f,1f));
    
    private IEnumerator ScaleAnimation(float startingSize, float middleSize, float endingSize, float startingInset,
        float endingInset,float duration = 0.2f)
    {
        Vector3 startingScale = Vector3.one*startingSize;
        Vector3 middleScale = Vector3.one * middleSize;
        Vector3 endingScale = Vector3.one * endingSize;

        float t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / (duration/2);
            transform.localScale = Vector3.Lerp(startingScale,middleScale,Mathf.SmoothStep(0,1,t));
            figureRenderer.SetInset(Mathf.Lerp(startingInset,endingInset,t));
            yield return null;
        }

        t = 0;
        while (t<=1)
        {
            t += Time.deltaTime * 1 / (duration/2);
            transform.localScale = Vector3.Lerp(middleScale,endingScale,Mathf.SmoothStep(0,1,t));
            yield return null;
        }
    }
}
