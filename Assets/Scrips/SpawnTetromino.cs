using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public bool canSpawn = true;
    public GameObject[] tetrominos;
    public GameObject previewPosition;
    public int UIScale = 25; // A escala na UI precisa ser maior

    private int nextTetromino;
    private GameObject preview;
    
    // Start is called before the first frame update
    void Start()
    {
        SetNextTetromino();
        NewTetromino();
    }

    public void NewTetromino ()
    {
        if (canSpawn)
        {
            if (preview) Destroy(preview);
            Instantiate(tetrominos[nextTetromino], transform.position, Quaternion.identity, transform);
            SetNextTetromino();
        }
    }

    void SetNextTetromino ()
    {
        nextTetromino = GetRandomTetromino();
        preview = Instantiate(tetrominos[nextTetromino], previewPosition.transform.position, Quaternion.identity, previewPosition.transform);
        // Ajusta o tamanho
        preview.transform.localScale = new Vector3(UIScale, UIScale, 0);
        // Encontra o Script
        TetrisBlock tetrisBlock = preview.GetComponent<TetrisBlock>();
        // Desativa para parar movimentação
        tetrisBlock.enabled = false;
        // Ajusta o offset
        preview.transform.localPosition -= (tetrisBlock.rotationPoint * UIScale) - (tetrisBlock.previewOffset * UIScale);
        foreach (Transform children in preview.transform)
        {
            children.gameObject.layer = 5;
        }
    }

    int GetRandomTetromino ()
    {
        return Random.Range(0, tetrominos.Length);
    }
}
