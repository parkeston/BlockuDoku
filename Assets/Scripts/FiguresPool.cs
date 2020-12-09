using System.Collections.Generic;
using UnityEngine;

public class FiguresPool : MonoBehaviour
{
    [SerializeField] private Figure[] figurePrefabs;
    
    private List<Figure> figuresPool;
    private List<Figure> currentFigures;

    private int currentFiguresCount;
    
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

    public void GetFigureSet(int figuresInSet)
    {
        currentFiguresCount = figuresInSet;
        
        for (int i = 0; i < figuresInSet; i++)
        {
            int index = Random.Range(0, figuresPool.Count);
            Figure figure = figuresPool[index];
            figure.gameObject.SetActive(true);
            figure.GetComponent<FigureRenderer>().enabled = true; //temp
            currentFigures.Add(figure);
            figuresPool.RemoveAt(index);
        }
    }

    public void DisposeFigure(Figure figure)
    {
        currentFiguresCount--;
        //figure.gameObject.SetActive(false); todo: do not disable figure, horizontal layout broken, make own horizontals layout!
        figure.GetComponent<FigureRenderer>().enabled = false;
        //currentFigures.Remove(figure);
        //figuresPool.Add(figure);

        if (currentFiguresCount == 0)
        {
            foreach (var currentFigure in currentFigures)
                currentFigure.gameObject.SetActive(false);
            
            figuresPool.AddRange(currentFigures);
            currentFigures.Clear();
            GetFigureSet(3);
        }
    }
}
