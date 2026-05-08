using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    public void SelectTurtle()
    {
        GameData.selectedCharacter = "Turtle";
        SceneManager.LoadScene("WorldMap");
    }

    public void SelectWhale()
    {
        GameData.selectedCharacter = "Whale";
        SceneManager.LoadScene("WorldMap");
    }

    public void SelectCoral()
    {
        GameData.selectedCharacter = "Coral";
        SceneManager.LoadScene("WorldMap");
    }
}