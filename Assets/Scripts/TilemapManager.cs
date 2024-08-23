using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInfo
{
    public bool isPlantable;
    public int nutrienteX;
    public int nutrienteY;

    // Você pode adicionar outros dados específicos do tile aqui
}

public class TilemapManager : MonoBehaviour
{
    public Tilemap tilemap;
    private Dictionary<Vector3Int, TileInfo> tileInfoDictionary = new Dictionary<Vector3Int, TileInfo>();

    void Start()
    {

    }

    public TileInfo GetTileInfo(Vector3Int position)
    {
        if (tileInfoDictionary.TryGetValue(position, out TileInfo info))
        {
            return info;
        }
        return null;
    }

    public void SetTileInfo(Vector3Int position, TileInfo info)
    {
        tileInfoDictionary[position] = info;
    }

    // Método de teste para verificar os dados de um tile
    public void TestTileData(Vector3Int position)
    {
        TileInfo info = GetTileInfo(position);
        if (info != null)
        {
            /*Debug.Log("Tile na posição " + position + " tem os seguintes dados:");
            Debug.Log("Is Plantable: " + info.isPlantable);
            Debug.Log("Nutriente X: " + info.nutrienteX);
            Debug.Log("Nutriente Y: " + info.nutrienteY);*/
        }
        else
        {
            Debug.Log("Nenhum dado encontrado para o tile na posição " + position);
        }
    }
}
