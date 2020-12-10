using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiguresPool : MonoBehaviour
{
    [SerializeField] private LayoutGroup layoutGroup;
    [SerializeField] private Figure[] figurePrefabs;
    
    private List<Figure> figuresPool;
    private List<Figure> currentFigures;
    
    private void Awake()
    {
        figuresPool = new List<Figure>();
        currentFigures = new List<Figure>();

        int k = 0;
        for (int i = 0; i <figurePrefabs.Length; i++)
        {
            figuresPool.Add(Instantiate(figurePrefabs[i], transform));
            figuresPool.Add(Instantiate(figurePrefabs[i], transform));
            
            figuresPool[k].gameObject.SetActive(false);
            figuresPool[k+1].gameObject.SetActive(false);

            k += 2;
        }
        
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
            currentFigures.Add(figure);
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
        currentFigures.Remove(figure);
        figuresPool.Add(figure);

        if (currentFigures.Count==0)
            GetFigureSet(3);
    }
}
