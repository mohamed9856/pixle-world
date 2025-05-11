using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class EndMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;

    void Start()
    {
        ShowScore();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        if(ScoreCount.Instance != null)
        {
            ScoreCount.Instance.totalGemsCollected = 0;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 6);
    }

    public void ShowScore()
    {
        if (ScoreCount.Instance != null)
        {
            score.text = "Score: " + ScoreCount.Instance.totalGemsCollected;
        }
        else
        {
            score.text = "Score: 0";
        }
    }
}
