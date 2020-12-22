using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FiguresPool : MonoBehaviour
{
    [SerializeField] private RectTransform figuresSpawnRect;
    [SerializeField] private float figuresSpawnSpacing;
    
    [SerializeField] private Figure[] figurePrefabs;
    
    private List<Figure> figuresPool;
    public List<Figure> CurrentFigures { get; private set; }

    private Vector3 figuresSpawnPoint;
    
    private void Awake()
    {
        figuresPool = new List<Figure>();
        CurrentFigures = new List<Figure>();

        int k = 0;
        for (int i = 0; i <figurePrefabs.Length; i++)
        {
            figuresPool.Add(Instantiate(figurePrefabs[i], transform));
            figuresPool.Add(Instantiate(figurePrefabs[i], transform));
            
            figuresPool[k].gameObject.SetActive(false);
            figuresPool[k+1].gameObject.SetActive(false);

            k += 2;
        }
        
        Bounds attachPointBounds = new Bounds();
        figuresSpawnRect.GetBounds(ref attachPointBounds);
        figuresSpawnPoint = attachPointBounds.center;
        
        GameManager.Instance.OnGameStarted += GenerateFigures;
    }

    private void GenerateFigures(GameMode gameMode)
    {
        foreach (var currentFigure in CurrentFigures)
        {
            currentFigure.gameObject.SetActive(false);
            figuresPool.Add(currentFigure);
        }
        CurrentFigures.Clear();
        GetFigureSet(3);
    }
    
    private void GetFigureSet(int figuresInSet)
    {
        for (int i = 0; i < figuresInSet; i++)
        {
            int index = Random.Range(0, figuresPool.Count); //add unique randomizing instead of removing picked for current set?
            Figure figure = figuresPool[index];
            figure.gameObject.SetActive(true);
            CurrentFigures.Add(figure);
            figuresPool.RemoveAt(index);
        }
        SetCurrentFiguresPositions();
    }

    private void SetCurrentFiguresPositions()
    {
        //in case of 3 figures, in other case need layout algorithm like built-in unity component
        CurrentFigures[0].transform.position = figuresSpawnPoint + Vector3.left * (CurrentFigures[0].GetSize().x / 2 
            + CurrentFigures[1].GetSize().x / 2+figuresSpawnSpacing);
        CurrentFigures[2].transform.position = figuresSpawnPoint + Vector3.right * (CurrentFigures[1].GetSize().x / 2
            + CurrentFigures[2].GetSize().x / 2+figuresSpawnSpacing);
        CurrentFigures[1].transform.position = figuresSpawnPoint;
    }

    public void DisposeFigure(Figure figure)
    {
        figure.gameObject.SetActive(false);
        CurrentFigures.Remove(figure);
        figuresPool.Add(figure);

        if (CurrentFigures.Count==0)
            GetFigureSet(3);
    }
}
