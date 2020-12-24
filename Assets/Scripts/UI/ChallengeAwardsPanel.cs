using UnityEngine;
using UnityEngine.UI;

public class ChallengeAwardsPanel : UIPanel
{
    [SerializeField] private ChallengeAwardListItem challengeAwardListItem;
    [SerializeField] private ScrollRect challengesAwardsList;

    public override void Init()
    {
        var availableChallenges = GameManager.Instance.GameMode.GetAvailableChallenges();
        foreach (var challenge in availableChallenges)
        {
            Instantiate(challengeAwardListItem, challengesAwardsList.content, false)
                .SetAwardData(challenge);
        }
    }
}
