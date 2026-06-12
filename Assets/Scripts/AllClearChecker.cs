using UnityEngine;
using UnityEngine.SceneManagement;

public class AllClearChecker : MonoBehaviour
{
    public string endSceneName = "EndScene";

    void Start()
    {
        if (GameState.alaboCleared &&
            GameState.chinaCleared &&
            GameState.indiaCleared &&
            GameState.nilsilanCleared &&
            GameState.australiaCleared &&
            GameState.southAfricaCleared &&
            GameState.antarcticaCleared &&
            GameState.basiCleared &&
            GameState.russiaCleared &&
            GameState.usaCleared &&
            GameState.italyCleared &&
            GameState.egyptCleared)
        {
            SceneManager.LoadScene(endSceneName);
        }
    }
}