using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardsScreen : MonoBehaviour
{
    //todo: make reusable tab system
    [SerializeField] private Toggle challengesAwardsToggle;
    [SerializeField] private Toggle eventsAwardsToggle;

    [SerializeField] private ScrollRect challengesAwardsList;
    [SerializeField] private ScrollRect eventsAwardsList;

    [SerializeField] private GameObject challengesAwardPrefab;
    [SerializeField] private GameObject eventsAwardPrefab;

    private void Awake()
    {
        SubscribeListToToggle(challengesAwardsToggle,challengesAwardsList.gameObject);
        SubscribeListToToggle(eventsAwardsToggle,eventsAwardsList.gameObject);

        challengesAwardsToggle.isOn = true;
    }

    private void OnEnable()
    {
        //todo: get actual active challenges data
        for (int i = 0; i < 3; i++)
            Instantiate(challengesAwardPrefab, challengesAwardsList.content, false);

        for (int i = 0; i < 3; i++)
            Instantiate(eventsAwardPrefab, eventsAwardsList.content, false);
    }

    private void SubscribeListToToggle(Toggle toggle, GameObject list)
    {
        toggle.onValueChanged.AddListener(list.SetActive);
    }
}
