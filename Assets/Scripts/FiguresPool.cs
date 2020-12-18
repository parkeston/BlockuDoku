using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FiguresPool : MonoBehaviour
{
    [SerializeField] private LayoutGroup layoutGroup;
    [SerializeField] private Figure[] figurePrefabs;
    
    private List<Figure> figuresPool;
    public List<Figure> CurrentFigures { get; private set; }
    
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
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStarted += GenerateFigures;
    }
    
    private void OnDisable()
    {
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
        layoutGroup.enabled = true;
        
        for (int i = 0; i < figuresInSet; i++)
        {
            int index = Random.Range(0, figuresPool.Count); //add unique randomizing instead of removing picked for current set?
            Figure figure = figuresPool[index];
            figure.gameObject.SetActive(true);
            CurrentFigures.Add(figure);
            figuresPool.RemoveAt(index);
        }

        StartCoroutine(DisableLayoutGroupOnLayoutUpdate());
    }

    private IEnumerator DisableLayoutGroupOnLayoutUpdate()
    {
        yield return new WaitForEndOfFrame();
        layoutGroup.enabled = false;
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
