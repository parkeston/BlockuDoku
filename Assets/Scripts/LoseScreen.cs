using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsCount;
    [SerializeField] private Button newGameButton;

    private void Awake()
    {
        gameObject.SetActive(false);
        newGameButton.onClick.AddListener(()=>SceneManager.LoadScene(0));
    }

    public void Show(string points)
    {
        pointsCount.text = points;
        gameObject.SetActive(true);
    }
}
