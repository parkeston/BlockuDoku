using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class FigureMover : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Figure figure;
    
    [Space]
    [SerializeField] private Vector2 pointerOffsetFactor;
    [SerializeField] private float positionDamping;

    private Vector3 startingPosition;
    private Vector3 desiredPosition;
    private Vector2 pointerOffset;

    private bool isDragging;

    public static event Action<Figure> OnFigureDrag;
    public static event Predicate<Figure> OnFigureDragEnded;

    private void Awake()
    {
        pointerOffset = new Vector2(UnityEngine.Screen.width * pointerOffsetFactor.x, UnityEngine.Screen.height * pointerOffsetFactor.y);
    }

    //no need for screen space to world space conversion with screen space overlay canvas mode
    public void OnPointerDown(PointerEventData eventData)
    {
        startingPosition = transform.position;
        desiredPosition = eventData.position + pointerOffset;
        transform.position = desiredPosition;

        isDragging = true;
        figure.Expand();
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        desiredPosition = eventData.position+pointerOffset;
        
        OnFigureDrag?.Invoke(figure);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        
        if(!(OnFigureDragEnded?.Invoke(figure)??false)) //if not set to grid
            transform.position = startingPosition;
    }

    private void Update()
    {
        if (isDragging)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 1 - positionDamping);
    }
}
