using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Text gameOverText;
    public Text scoreValueText;
    public Text nextText; // Usado para posicionar o Tetromino do preview
    public int height { get; } = 20;
    public int width { get; } = 10;

    private Transform[,] occupiedGrid;
    private SpawnTetromino spawnTetromino;
    private int score = 0;

    private void Awake()
    {
        // Singleton
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        occupiedGrid = new Transform[width, height];
        spawnTetromino = FindObjectOfType<SpawnTetromino>();
        scoreValueText.text = this.score.ToString();
    }

    public Transform GetGridPos (int x, int y)
    {
        return occupiedGrid[x, y];
    }

    public void SetGridPos (int x, int y, Transform transform)
    {
        occupiedGrid[x, y] = transform;
    }

    public void GameOver ()
    {
        Debug.Log("Game Over!");
        spawnTetromino.canSpawn = false;
        gameOverText.gameObject.SetActive(true);
    }

    public void CheckLines()
    {
        // Cada linha que é apagada aumenta o multiplicador para fazer mais pontos
        int multiplier = 0;
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                multiplier++;
                AddScore(100 * multiplier);
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    public void AddScore (int score)
    {
        this.score += score;
        scoreValueText.text = this.score.ToString();
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (GetGridPos(j, i) == null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(GetGridPos(j, i).gameObject);
            SetGridPos(j, i, null);
        }
    }

    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (GetGridPos(j, y) != null)
                {
                    SetGridPos(j, y - 1, GetGridPos(j, y));
                    SetGridPos(j, y, null);
                    GetGridPos(j, y - 1).transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
}
