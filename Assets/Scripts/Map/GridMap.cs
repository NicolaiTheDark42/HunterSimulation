using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{

    // Guarda a referência para o Tile
    public GameObject tile;
    // Guarda o tamanho do mapa
    public Vector2Int mapSize = new Vector2Int(30,30);

    // Guarda a posição de cada tile do mapa
    public Tile[,] tiles = new Tile[30, 30]; 

    void Awake()
    {
        GenerateMap();     
    }

    // Cria o mapa
    void GenerateMap()
    {
        // Coloca os tiles como child do objeto em que esse script está anexado
        Transform mapHolder = new GameObject("Tiles").transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2f + 0.5f + x, 0f, -mapSize.y / 2f + 0.5f + y);
                GameObject newTile = Instantiate(tile, tilePosition, Quaternion.identity);

                newTile.transform.localScale *= 0.95f;
                newTile.transform.parent = mapHolder;

                tiles[x, y] = newTile.GetComponent<Tile>();
                tiles[x, y].pos = new Vector2Int(x, y);
            }
        }
    }
}
