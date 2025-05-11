using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public PlayerLife player;

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (player != null && player.maxHealth > 0)
        {
            healthBar.fillAmount = (float)player.health / player.maxHealth;
        }
    }
}
