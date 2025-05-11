using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    public static int levelGems = 0;

    [SerializeField] private TextMeshProUGUI gemsText;
    [SerializeField] private AudioSource collectSoundEffect;

    private void Start()
    {
        levelGems = 0;
        UpdateUI();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gems")) 
        {
            if (collectSoundEffect != null)
            {
                collectSoundEffect.Play();
            }
            
            Destroy(collision.gameObject);
            levelGems++;
            
            if(ScoreCount.Instance != null)
            {
                ScoreCount.Instance.AddGem();
            }
            
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        gemsText.text = "Gems: " + levelGems + "/7";
    }
}