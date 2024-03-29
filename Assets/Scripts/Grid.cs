﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    //todo: refactor
    [SerializeField] private ScorePopup scorePopup;
    [SerializeField] private GridText gridText;

    [Space]
    [SerializeField] private FiguresPool figuresPool;
    
    [Space]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private GridRenderer gridRenderer;
    
    [Space]
    [SerializeField] private int cellSize = 100;
    [SerializeField] private Vector2Int gridSize = new Vector2Int(3,3);

    public int CellSize => cellSize;
    public Vector2Int GridSize => gridSize;
    
    public HashSet<(int x,int y)> ClosestPoints { get; private set; }
    public HashSet<(int x,int y)> SetPoints { get; private set; }
    public HashSet<(int x, int y)> ComboHighlights { get; private set; }
    
    private Dictionary<(int x,int y),Vector2> gridPoints;
    private Dictionary<(int x, int y), int> multiLifePoints;
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
        ComboHighlights = new HashSet<(int x, int y)>();
        gridPoints = new Dictionary<(int, int), Vector2>();
        multiLifePoints = new Dictionary<(int x, int y), int>();
        
        GameManager.Instance.OnGameStarted += GenerateGrid;
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
        if(!figure.Interactable)
            return;
        
        var figureBounds = figure.Bounds;
        //shrink bounds of figure to it's center points to easier stack it on grid edges
        figureBounds.Expand(new Vector3(-1,-1,0)*cellSize*gridRenderer.canvas.scaleFactor);
        
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
            {
                ClosestPoints.Clear();
                ComboHighlights.Clear();
            }
            else
                UpdateComboSet();
            
            gridRenderer.SetVerticesDirty();
        }
        else if (ClosestPoints.Count>0)
        {
            ClosestPoints.Clear();
            ComboHighlights.Clear();
            gridRenderer.SetVerticesDirty();
        }
    }

    private bool UpdateGridDetectionOnDragEnd(Figure figure)
    { 
        if (ClosestPoints.Count>0)
        {
            var closestPointsBounds = new Bounds(gridPoints[ClosestPoints.First()], Vector3.one);
            foreach (var closestPoint in ClosestPoints)
                closestPointsBounds.Encapsulate(gridPoints[closestPoint]);
            figure.MoveToGridClosestPoints(closestPointsBounds.center,()=>UpdateGridLayout(figure));
            return true;
        }
        else
        {
            figure.Shrink();
            return false;
        }
    }

    private void UpdateGridLayout(Figure figure)
    {
        SetPoints.UnionWith(ClosestPoints);
        ConsumeComboSet();
        ClosestPoints.Clear();
        gridRenderer.PlayComboAnimation(ComboHighlights);
        ComboHighlights.Clear();
            
        figuresPool.DisposeFigure(figure);

        bool anyInteractable = false;
        foreach (var currentFigure in figuresPool.CurrentFigures)
            anyInteractable|= CheckFigurePlacementAbility(currentFigure);
        if(!anyInteractable)
            CoroutineSheduler.Instance.InvokeWithDelay(()=>
                GameManager.Instance.Lose(new Rect(gridBounds.min,gridBounds.size)),1f);
    }

    private bool CheckFigurePlacementAbility(Figure figure)
    {
        //if less than third part of grid set, skip actual check
        if (SetPoints.Count < gridSize.x * gridSize.y / 3)
        {
            figure.Interactable = true;
            return true;
        }
        
        bool canBePlaced = true;
        var figureCells = figure.DrawCellsIndices;
        
        foreach (var gridCell in gridPoints.Keys)
        {
            canBePlaced = true;
            for (int i = 0; i < figureCells.Count; i++)
            {
                (int x,int y) figureCellInGrid = (gridCell.x + figureCells[i].x, gridCell.y + figureCells[i].y);
                if (figureCellInGrid.x > gridSize.x - 1 || figureCellInGrid.y > gridSize.y - 1 ||
                    SetPoints.Contains(figureCellInGrid))
                {
                    canBePlaced = false;
                    break;
                }
            }
            
            if(canBePlaced)
                break;
        }

        figure.Interactable = canBePlaced;
        return canBePlaced;
    }

    private void UpdateComboSet()
    {
        ComboHighlights.Clear();
        
        var columnsForComboCheck = ClosestPoints.Select(cell => cell.x).Distinct();
        var rowsForComboCheck = ClosestPoints.Select(cell => cell.y).Distinct();
        
        HashSet<(int x, int y)> cellsSet = new HashSet<(int x, int y)>();
        
        foreach (var columnIndex in columnsForComboCheck)
        {
            cellsSet.Clear();
            for (int y = 0; y < gridSize.y; y++)
            {
                if(!ClosestPoints.Contains((columnIndex,y)) && !SetPoints.Contains((columnIndex,y)))
                    break;

                cellsSet.Add((columnIndex, y));
            }
            
            if(cellsSet.Count==gridSize.y)
                ComboHighlights.UnionWith(cellsSet);
        }
        
        foreach (var rowIndex in rowsForComboCheck)
        {
            cellsSet.Clear();
            for (int x = 0; x < gridSize.x; x++)
            {
                if(!ClosestPoints.Contains((x,rowIndex)) && !SetPoints.Contains((x,rowIndex)))
                    break;

                cellsSet.Add((x, rowIndex));
            }
            
            if(cellsSet.Count==gridSize.x)
                ComboHighlights.UnionWith(cellsSet);
        }

        int cubeSize = 3;
        HashSet<(int x, int y)> figureOverlapedCubes = new HashSet<(int x, int y)>();

        foreach (var point in ClosestPoints)
        {
            var cubeCornerX = point.x / cubeSize * cubeSize;
            var cubeCornerY = point.y / cubeSize * cubeSize;
            figureOverlapedCubes.Add((cubeCornerX, cubeCornerY));
        }

        foreach (var cubeCorner in figureOverlapedCubes)
        {
            cellsSet.Clear();
            var cubeNotFullyFilled =false;
            
            for (int y = 0; y < cubeSize; y++)
            {
                for (int x = 0; x < cubeSize; x++)
                {
                    (int x, int y) cubeCellInGrid = (cubeCorner.x + x, cubeCorner.y + y);
                    if (!ClosestPoints.Contains(cubeCellInGrid) && !SetPoints.Contains(cubeCellInGrid))
                    {
                        cubeNotFullyFilled = true;
                        break;
                    }

                    cellsSet.Add(cubeCellInGrid);
                }
                
                if(cubeNotFullyFilled)
                    break;
            }
            
            if(cellsSet.Count==cubeSize*cubeSize)
                ComboHighlights.UnionWith(cellsSet);
        }
    }

    
    private void ConsumeComboSet()
    {
        Vector3 comboPopupPosition;
        var scoreCellsCount = ComboHighlights.Count > 0 ? ComboHighlights.Count : ClosestPoints.Count;
        
        if (ComboHighlights.Count == 9) //default combo
        {
            var comboBounds = new Bounds(gridPoints[ComboHighlights.First()], Vector3.one);
            foreach (var comboCell in ComboHighlights)
                comboBounds.Encapsulate(gridPoints[comboCell]);
            comboPopupPosition = comboBounds.center;
        }
        else
            comboPopupPosition = gridBounds.center;
        
        UpdateMultiLifeMode();
        SetPoints.ExceptWith(ComboHighlights);
        scorePopup.transform.position = comboPopupPosition;
        GameManager.Instance.GameScore.AddScore(scoreCellsCount);
        if(GameManager.Instance.ChallengeProgress>=1)
            CoroutineSheduler.Instance.InvokeWithDelay(GameManager.Instance.Win,1f);
    }

    private void UpdateMultiLifeMode()
    {
        if (multiLifePoints.Count > 0 && ComboHighlights.Overlaps(multiLifePoints.Keys))
        {
            HashSet<(int x, int y)> tmpCombo = new HashSet<(int x, int y)>(ComboHighlights);
            ComboHighlights.ExceptWith(multiLifePoints.Keys);
            
            foreach (var comboCell in tmpCombo)
            {
                if (multiLifePoints.ContainsKey(comboCell))
                {
                    var cellLife = --multiLifePoints[comboCell];
                    if (cellLife == 1)
                        multiLifePoints.Remove(comboCell);
                }
            }
            UpdateMultiLifeCellsText();
        }
    }

    private void GenerateGrid(GameMode gameMode)
    {
        ClosestPoints.Clear();
        SetPoints.Clear();
        ComboHighlights.Clear();
        multiLifePoints.Clear();
        gridRenderer.SetVerticesDirty();
        gridText.Clear();
        
        if(!gameMode.IsChallengeMode)
            return;
        
        //todo:set point to win!
        foreach (var gridCell in gameMode.CurrentChallenge.GridSet)
        {
            SetPoints.Add(gridCell.CellIndex);
            multiLifePoints.Add(gridCell.CellIndex,gridCell.LifePoints);
        }
        
        UpdateMultiLifeCellsText();
    }

    private void UpdateMultiLifeCellsText()
    {
        Vector2[] lifePointsTextPositions = new Vector2[multiLifePoints.Count];
        int k = 0;
        foreach (var cell in multiLifePoints.Keys)
        {
            lifePointsTextPositions[k] = gridPoints[cell];
            k++;
        }
        gridText.UpdateText(multiLifePoints.Values.ToArray(),lifePointsTextPositions);
    }
}
