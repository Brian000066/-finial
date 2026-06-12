using UnityEngine;

public class WorldMapEnvironment : MonoBehaviour
{
    [Header("Alabo")]
    public SpriteRenderer alaboFactory;
    public GameObject alaboForest;

    [Header("China")]
    public SpriteRenderer chinaFactory;
    public GameObject chinaForest;

    [Header("India")]
    public SpriteRenderer indiaFactory;
    public GameObject indiaForest;

    [Header("nilsilan")]
    public SpriteRenderer nilsilanFactory;
    public GameObject nilsilanForest;

    [Header("Australia")]
    public SpriteRenderer australiaFactory;
    public GameObject australiaForest;

    void Start()
    {
        alaboFactory.enabled = !GameState.alaboCleared;
        alaboForest.SetActive(GameState.alaboCleared);

        chinaFactory.enabled = !GameState.chinaCleared;
        chinaForest.SetActive(GameState.chinaCleared);

        indiaFactory.enabled = !GameState.indiaCleared;
        indiaForest.SetActive(GameState.indiaCleared);

        nilsilanFactory.enabled = !GameState.nilsilanCleared;
        nilsilanForest.SetActive(GameState.nilsilanCleared);

        australiaFactory.enabled = !GameState.australiaCleared;
        australiaForest.SetActive(GameState.australiaCleared);
    }
}