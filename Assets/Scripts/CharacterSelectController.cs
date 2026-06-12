using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    public Image characterImage;

    public Sprite[] characterSprites;

    private int currentCharacter = 0;

    void Start()
    {
        UpdateCharacter();
    }

    void UpdateCharacter()
    {
        characterImage.sprite = characterSprites[currentCharacter];
    }

    public void NextCharacter()
    {
        currentCharacter++;

        if (currentCharacter >= characterSprites.Length)
        {
            currentCharacter = 0;
        }

        UpdateCharacter();
    }

    public void PreviousCharacter()
    {
        currentCharacter--;

        if (currentCharacter < 0)
        {
            currentCharacter = characterSprites.Length - 1;
        }

        UpdateCharacter();
    }

    public void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacter);

        SceneManager.LoadScene("WorldMap");
    }
}