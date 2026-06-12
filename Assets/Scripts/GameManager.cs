using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int width = 10;
    public static int height = 21;
    public Transform[,] grid = new Transform[width, height];

    public int lives = 5;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI mainUIText;
    public bool isGameOver = false;

    public GameObject singleBlockPrefab;

    [Header("場景設定")]
    public string worldMapSceneName = "WorldMap";
    public float returnDelay = 2f;

    private bool isWin = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        UpdateBoardState();
    }

    void Update()
    {
        // P = 快速通關
        if (!isGameOver && Input.GetKeyDown(KeyCode.P))
        {
            WinGame();
        }

        if (isGameOver)
        {
            // R = 重玩
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }

            // M = 回 WorldMap
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene(worldMapSceneName);
            }
        }
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CheckLines()
    {
        int linesCleared = 0;

        for (int y = 0; y < height; y++)
        {
            if (HasLine(y))
            {
                DeleteLine(y);
                DecreaseRowsAbove(y);
                linesCleared++;
                y--;
            }
        }

        for (int i = 0; i < linesCleared; i++)
        {
            SpawnNeonGreenBlock();
        }

        UpdateBoardState();
    }

    bool HasLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null) return false;
        }

        return true;
    }

    void DeleteLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                if (grid[x, y].GetComponent<Block>().currentType == BlockType.NeonGreen)
                {
                    lives++;
                }

                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    void DecreaseRowsAbove(int y)
    {
        for (int i = y; i < height - 1; i++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, i + 1] != null)
                {
                    grid[x, i] = grid[x, i + 1];
                    grid[x, i].position += new Vector3(0, -1, 0);
                    grid[x, i + 1] = null;
                }
            }
        }
    }

    public void PushAllBlocksUp()
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, height - 1] != null)
            {
                Destroy(grid[x, height - 1].gameObject);
                grid[x, height - 1] = null;
            }
        }

        for (int y = height - 2; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y + 1] = grid[x, y];
                    grid[x, y + 1].position += new Vector3(0, 1, 0);
                    grid[x, y] = null;
                }
            }
        }
    }

    public void SpawnPenaltyRow(int emptyColIndex)
    {
        for (int x = 0; x < width; x++)
        {
            if (x == emptyColIndex) continue;

            GameObject newBlock = Instantiate(
                singleBlockPrefab,
                new Vector3(x, 0, 0),
                Quaternion.identity
            );

            grid[x, 0] = newBlock.transform;
            newBlock.GetComponent<Block>().SetType(BlockType.Normal);
        }
    }

    void SpawnNeonGreenBlock()
    {
        List<Block> normalBlocks = new List<Block>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y] != null)
                {
                    Block b = grid[x, y].GetComponent<Block>();

                    if (b.currentType == BlockType.Normal)
                    {
                        normalBlocks.Add(b);
                    }
                }
            }
        }

        if (normalBlocks.Count > 0)
        {
            int randIndex = Random.Range(0, normalBlocks.Count);
            normalBlocks[randIndex].SetType(BlockType.NeonGreen);
        }
    }

    public void ModifyLives(int amount)
    {
        if (isGameOver) return;

        lives += amount;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            UpdateBoardState();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        isWin = false;

        if (gameOverText != null)
        {
            gameOverText.fontSize = 40;
            gameOverText.text = "You have been contaminated.\nPress 'R' to restart\nPress 'M' to Map";
            gameOverText.color = Color.red;
            gameOverText.gameObject.SetActive(true);
        }
    }

    public void WinGame()
    {
        if (isGameOver) return;

        isGameOver = true;
        isWin = true;
        GameState.russiaCleared = true;
        if (gameOverText != null)
        {
            gameOverText.fontSize = 50;
            gameOverText.text = "You survived.";
            gameOverText.color = Color.green;
            gameOverText.gameObject.SetActive(true);
        }

        StartCoroutine(ReturnToWorldMapAfterDelay());
    }

    IEnumerator ReturnToWorldMapAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);

        SceneManager.LoadScene(worldMapSceneName);
    }

    public int GetBlackBlockCount()
    {
        int count = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (
                    grid[x, y] != null &&
                    grid[x, y].GetComponent<Block>().currentType == BlockType.Black
                )
                {
                    count++;
                }
            }
        }

        return count;
    }

    public void UpdateBoardState()
    {
        Spawner7 spawner = FindObjectOfType<Spawner7>();
        int blackCount = GetBlackBlockCount();
        int currentTurn = spawner != null ? spawner.GetSpawnCount() : 0;

        int turnsUntilBlack = 5 - (currentTurn % 5);

        if (turnsUntilBlack == 5 && currentTurn != 0)
        {
            turnsUntilBlack = 0;
        }

        if (mainUIText != null)
        {
            mainUIText.fontSize = 25;

            ShowMessage(
                $"Lives: {lives}\nCurrent Turn: {currentTurn} / 100\nNext Red Block: {turnsUntilBlack}\nRed Blocks: {blackCount}\nUpArrow: Rotation",
                Color.white
            );

            mainUIText.color = blackCount > 3 ? Color.red : Color.white;
        }

        if (spawner != null)
        {
            spawner.UpdatePreviewVisibility(blackCount);
        }
    }

    void ShowMessage(string msg, Color color)
    {
        if (mainUIText != null)
        {
            mainUIText.text = msg;
            mainUIText.color = color;
        }
    }
}