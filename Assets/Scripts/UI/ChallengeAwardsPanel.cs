using UnityEngine;
using UnityEngine.UI;

public class ChallengeAwardsPanel : UIPanel
{
    [SerializeField] private GameObject challengesAwardPrefab;
    [SerializeField] private ScrollRect challengesAwardsList;

    public override void Init()
    {
        //todo: get actual active challenges data
        for (int i = 0; i < 3; i++)
            Instantiate(challengesAwardPrefab, challengesAwardsList.content, false);
    }
}
