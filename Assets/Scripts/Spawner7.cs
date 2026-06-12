using UnityEngine;
using System.Collections.Generic;

public class Spawner7 : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;
    public Transform nextPieceSpawnPoint;

    private GameObject nextTetromino;
    private int spawnCount = 0;
    private List<int> bag = new List<int>();

    void Start()
    {
        FillBag();
        GenerateNextTetromino();
        SpawnPiece();
    }

    // 新增：提供給 GameManager 讀取目前回合數
    public int GetSpawnCount()
    {
        return spawnCount;
    }

    void FillBag()
    {
        bag.Clear();
        for (int i = 0; i < tetrominoPrefabs.Length; i++)
        {
            bag.Add(i); bag.Add(i);
        }
        for (int i = 0; i < bag.Count; i++)
        {
            int temp = bag[i];
            int randomIndex = Random.Range(i, bag.Count);
            bag[i] = bag[randomIndex];
            bag[randomIndex] = temp;
        }
    }

    void GenerateNextTetromino()
    {
        if (spawnCount >= 100) return;

        int index = bag[0];
        bag.RemoveAt(0);
        if (bag.Count == 0) FillBag();

        nextTetromino = Instantiate(tetrominoPrefabs[index], nextPieceSpawnPoint.position, Quaternion.identity);
        nextTetromino.GetComponent<Tetromino>().enabled = false;

        int nextTurn = spawnCount + 1;
        if (nextTurn % 5 == 0)
        {
            ApplyBlackBlocksToTetromino(nextTetromino, nextTurn);
        }

        UpdatePreviewVisibility(GameManager.instance.GetBlackBlockCount());
    }

    public void SpawnPiece()
    {
        if (GameManager.instance.isGameOver) return;

        spawnCount++;

        // 每次生成新方塊時，通知 GameManager 刷新整合後的 UI 文字
        GameManager.instance.UpdateBoardState();

        if (spawnCount > 100)
        {
            GameManager.instance.WinGame();
            return;
        }

        if (spawnCount % 5 == 0)
        {
            ApplyBlackBlockPenalty();
        }

        nextTetromino.transform.position = transform.position;
        nextTetromino.GetComponent<Tetromino>().enabled = true;

        foreach (Renderer r in nextTetromino.GetComponentsInChildren<Renderer>())
        {
            r.enabled = true;
        }

        if (!nextTetromino.GetComponent<Tetromino>().IsValidGridPos())
        {
            GameManager.instance.GameOver();
            return;
        }

        GenerateNextTetromino();
    }

    void ApplyBlackBlocksToTetromino(GameObject tetromino, int turn)
    {
        Block[] blocks = tetromino.GetComponentsInChildren<Block>();
        
        for (int i = 0; i < blocks.Length; i++)
        {
            Block temp = blocks[i];
            int randomIndex = Random.Range(i, blocks.Length);
            blocks[i] = blocks[randomIndex];
            blocks[randomIndex] = temp;
        }

        int blackCount = 1;
        if (turn >= 31 && turn <= 65) blackCount = 2;
        else if (turn >= 66) blackCount = 3;

        for (int i = 0; i < blackCount && i < blocks.Length; i++)
        {
            blocks[i].SetType(BlockType.Black);
        }
    }

    void ApplyBlackBlockPenalty()
    {
        int blackBlockCount = GameManager.instance.GetBlackBlockCount();
        if (blackBlockCount > 0)
        {
            int emptyCol = Random.Range(0, GameManager.width);
            for (int i = 0; i < blackBlockCount; i++)
            {
                GameManager.instance.PushAllBlocksUp();
                GameManager.instance.SpawnPenaltyRow(emptyCol);
                GameManager.instance.ModifyLives(-1);
            }
            GameManager.instance.UpdateBoardState();
        }
    }

    public void UpdatePreviewVisibility(int blackCountOnBoard)
    {
        if (nextTetromino == null) return;
        bool isVisible = blackCountOnBoard <= 3;
        foreach (Renderer r in nextTetromino.GetComponentsInChildren<Renderer>())
        {
            r.enabled = isVisible;
        }
    }
}