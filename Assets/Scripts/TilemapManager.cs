using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInfo
{
    public bool isPlantable;
    
    // Nutrientes NPK
    public int nitrogen;
    public int phosphorus;
    public int potassium;

    // Construtor para facilitar a criação de TileInfo
    public TileInfo(bool isPlantable, int nitrogen, int phosphorus, int potassium)
    {
        this.isPlantable = isPlantable;
        this.nitrogen = nitrogen;
        this.phosphorus = phosphorus;
        this.potassium = potassium;
    }
}


public class TilemapManager : MonoBehaviour
{
    public Tilemap tilemap;
    private Dictionary<Vector3Int, TileInfo> tileInfoDictionary = new Dictionary<Vector3Int, TileInfo>();

    void Start()
    {
        // Aqui você pode inicializar o tilemap ou adicionar outras configurações iniciais
    }

    // Obtém as informações do tile a partir de sua posição
    public TileInfo GetTileInfo(Vector3Int position)
    {
        if (tileInfoDictionary.TryGetValue(position, out TileInfo info))
        {
            return info;
        }
        return null;
    }

    // Define as informações de um tile em uma posição específica
    public void SetTileInfo(Vector3Int position, TileInfo info)
    {
        tileInfoDictionary[position] = info;
    }

    // Método para verificar e exibir os dados de um tile
    public void TestTileData(Vector3Int position)
    {
        TileInfo info = GetTileInfo(position);
        if (info != null)
        {
            Debug.Log("Tile na posição " + position + " tem os seguintes dados:");
            Debug.Log("Is Plantable: " + info.isPlantable);
            Debug.Log("Nitrogênio (N): " + info.nitrogen);
            Debug.Log("Fósforo (P): " + info.phosphorus);
            Debug.Log("Potássio (K): " + info.potassium);
        }
        else
        {
            Debug.Log("Nenhum dado encontrado para o tile na posição " + position);
        }
    }

    // Método para consumir nutrientes de um tile
    public void ConsumeNutrients(Vector3Int position, int nitrogenAmount, int phosphorusAmount, int potassiumAmount)
    {
        TileInfo info = GetTileInfo(position);
        if (info != null)
        {
            info.nitrogen = Mathf.Max(0, info.nitrogen - nitrogenAmount);
            info.phosphorus = Mathf.Max(0, info.phosphorus - phosphorusAmount);
            info.potassium = Mathf.Max(0, info.potassium - potassiumAmount);
            Debug.Log($"Nutrientes consumidos no tile em {position}: N: {nitrogenAmount}, P: {phosphorusAmount}, K: {potassiumAmount}");
        }
        else
        {
            Debug.LogWarning("Nenhum dado encontrado para o tile na posição " + position);
        }
    }
}