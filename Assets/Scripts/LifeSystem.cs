using UnityEngine;
using TMPro;

public class LifeSystem : MonoBehaviour
{
    public int lives = 3;

    public TMP_Text lifeText;

    void Start()
    {
        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;

        UpdateUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    void UpdateUI()
    {
        lifeText.text = "Lives: " + lives;
    }
}