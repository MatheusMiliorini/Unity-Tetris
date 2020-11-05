using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public bool canSpawn = true;
    public GameObject[] tetrominos;
    
    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    public void NewTetromino ()
    {
        if (canSpawn)
            Instantiate(tetrominos[Random.Range(0, tetrominos.Length)], transform.position, Quaternion.identity, transform);
    }
}
