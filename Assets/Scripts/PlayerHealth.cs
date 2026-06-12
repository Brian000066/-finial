using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // 最大血量
    public float maxHealth = 100f;

    // 目前血量
    public float currentHealth;

    // 每秒慢慢扣血
    public float drainSpeed = 2f;

    // 血條 UI
    public Image healthBarFill;
    private bool isDraining = true;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        // 持續扣血
        if (isDraining)
        {
            TakeDamage(drainSpeed * Time.deltaTime);
        }
    }

    // 扣血
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // 防止低於 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        UpdateHealthBar();

        // 死亡
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // 補血
    public void Heal(float amount)
    {
        currentHealth += amount;

        // 防止超過最大血量
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar();
    }

    // 更新血條
    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    // 死亡
    void Die()
    {
        Debug.Log("Game Over");

        GameOverManager gm =
            FindFirstObjectByType<GameOverManager>();

        gm.GameOver();
    }
    public void StopHealthDrain()
    {
        isDraining = false;
    }
}