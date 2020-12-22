using System;
using UnityEngine;

[Serializable]
public class Challenge
{
    [SerializeField] private int pointsToPass;
    [SerializeField] private GridCell[] gridSet;
    
    [Serializable]
    public struct GridCell
    {
        [SerializeField] private Vector2Int cellIndex;
        [SerializeField] private int lifePoints;

        public (int x, int y) CellIndex => (cellIndex.x,cellIndex.y);
        public int LifePoints => lifePoints;
    }

    public int PointsToPass => pointsToPass;
    public GridCell[] GridSet => gridSet;
}
