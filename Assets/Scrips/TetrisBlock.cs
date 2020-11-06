using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{

    public Vector3 rotationPoint;
    public Vector3 previewOffset; // Subtrai ao rotationPoint (uma bela gambiarra)
    private float previousTime;
    private float fallTime = 0.8f;
    private SpawnTetromino spawnTetromino;

    // Start is called before the first frame update
    void Start()
    {
        spawnTetromino = FindObjectOfType<SpawnTetromino>();
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            GameManager.instance.SetGridPos(roundedX, roundedY, children);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 previousPosition = transform.position;
        var previousRotation = transform.rotation;
        bool movedDown = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
        } 
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }

        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
            movedDown = true;
        }

        // Bateu numa parede ou etc, volta ao estado anterior
        if (!ValidMove())
        {
            transform.position = previousPosition;
            transform.rotation = previousRotation;
            if (movedDown) // Chegou no chão
            {
                if (transform.position == spawnTetromino.gameObject.transform.position)
                {
                    GameManager.instance.GameOver();
                    this.enabled = false;
                }
                else
                {
                    AddToGrid();
                    GameManager.instance.CheckLines();
                    this.enabled = false;
                    spawnTetromino.NewTetromino();
                }
            }
        }
    }

    bool ValidMove ()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= GameManager.instance.width || roundedY < 0 || roundedY >= GameManager.instance.height)
            {
                return false;
            }

            if (GameManager.instance.GetGridPos(roundedX, roundedY) != null)
            {
                return false;
            }
        }
        return true;
    }
}
