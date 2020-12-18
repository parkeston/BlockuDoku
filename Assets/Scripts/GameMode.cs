using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode
{
    private int multiLifeCellsCount;
    private int maxCellLife;

    public int MultiLifeCellsCount => multiLifeCellsCount;
    public int MaXCellLife => maxCellLife;
    
    public void SetDefaultMode()
    {
        multiLifeCellsCount = 0;
        maxCellLife = 1;
    }

    //todo: get actual challenge data
    public void SetChallengeMode()
    {
        multiLifeCellsCount = 10;
        maxCellLife = 3;
    }
}
