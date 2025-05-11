using UnityEngine;

public class ScoreCount : MonoBehaviour
{
    public static ScoreCount Instance;

    public int totalGemsCollected = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddGem()
    {
        totalGemsCollected++;
    }
}
