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

    [Header("SouthAfrica")]
    public SpriteRenderer southAfricaFactory;
    public GameObject southAfricaForest;

    [Header("Antarctica")]
    public SpriteRenderer antarcticaFactory;
    public GameObject antarcticaForest;

    [Header("87")]
    public SpriteRenderer basiFactory;
    public GameObject basiForest;

    [Header("USA")]
    public SpriteRenderer usaFactory;
    public GameObject usaForest;

    [Header("Egypt")]
    public SpriteRenderer egyptFactory;
    public GameObject egyptForest;

    [Header("Italy")]
    public SpriteRenderer italyFactory;
    public GameObject italyForest;

    [Header("Russia")]
    public SpriteRenderer russiaFactory;
    public GameObject russiaForest;

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

        southAfricaFactory.enabled = !GameState.southAfricaCleared;
        southAfricaForest.SetActive(GameState.southAfricaCleared);

        antarcticaFactory.enabled = !GameState.antarcticaCleared;
        antarcticaForest.SetActive(GameState.antarcticaCleared);

        basiFactory.enabled = !GameState.basiCleared;
        basiForest.SetActive(GameState.basiCleared);

        usaFactory.enabled = !GameState.usaCleared;
        usaForest.SetActive(GameState.usaCleared);

        russiaFactory.enabled = !GameState.russiaCleared;
        russiaForest.SetActive(GameState.russiaCleared);

        italyFactory.enabled = !GameState.italyCleared;
        italyForest.SetActive(GameState.italyCleared);

        egyptFactory.enabled = !GameState.egyptCleared;
        egyptForest.SetActive(GameState.egyptCleared);
    }
}