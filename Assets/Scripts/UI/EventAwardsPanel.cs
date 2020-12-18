using UnityEngine;
using UnityEngine.UI;

public class EventAwardsPanel : UIPanel
{
    [SerializeField] private GameObject eventsAwardPrefab;
    [SerializeField] private ScrollRect eventsAwardsList;

    public override void Init()
    {
        for (int i = 0; i < 3; i++)
            Instantiate(eventsAwardPrefab, eventsAwardsList.content, false);
    }
}
