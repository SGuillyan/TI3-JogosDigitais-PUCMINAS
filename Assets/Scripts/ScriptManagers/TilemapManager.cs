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
    public int humidity;

    // Construtor para facilitar a criação de TileInfo
    public TileInfo(bool isPlantable, int nitrogen, int phosphorus, int potassium, int humidity)
    {
        this.isPlantable = isPlantable;
        this.nitrogen = nitrogen;
        this.phosphorus = phosphorus;
        this.potassium = potassium;
        this.humidity = humidity;
    }
}

public class TilemapManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Transform parentTransform;  // Referência ao objeto pai na hierarquia
    public CustomTileBase defaultTile;  // Tile padrão para o terreno expandido
    public CustomTileBase riverTile;  // Tile padrão para o terreno expandido

    public CustomTileBase borderTileLeft;
    public CustomTileBase borderTileRight;
    public CustomTileBase borderTileUp;
    public CustomTileBase borderTileDown;
    public CustomTileBase cornerLarge;
    public CustomTileBase cornerSmall;

    public TreeTile treeTile;

    public Dictionary<Vector3Int, TileInfo> tileInfoDictionary = new Dictionary<Vector3Int, TileInfo>();
    private Dictionary<Vector3Int, GameObject> instantiatedTileDictionary = new Dictionary<Vector3Int, GameObject>(); // Adiciona controle de objetos instanciados

    void Start()
    {
        // Inicializações necessárias para o tilemap ou outras configurações
        // tilemap.SetTile(new Vector3Int(10, -10, 0), treeTile);
    }


    public void ExpandTerrain(Vector3Int bottomLeft, Vector3Int topRight)
    {
        if (defaultTile == null)
        {
            Debug.LogError("Default Tile não atribuído!");
            return;
        }

        // Define o retângulo para expandir o terreno baseado nos limites fornecidos
        for (int x = bottomLeft.x; x <= topRight.x; x++)
        {
            for (int y = bottomLeft.y; y <= topRight.y; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);

                // Verifica se a expansão é para o Sul e ajusta o rio

                if (y == -5 &&  x <= 19) // Parte inicial do rio (reta)
                {
                    tilemap.SetTile(position, riverTile);
                }
                else if (y < -16 && x <= 11) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y < -7 && x <= 3) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -16 && x < 11) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -17 && x < 12) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -18 && x < 13) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -19 && x < 13) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -20 && x < 14) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -21 && x < 14) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -22 && x < 15) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y == -23 && x < 16) 
                {
                    tilemap.SetTile(position, treeTile);
                }
                else if (y <= -5 && x == 19) 
                {
                    tilemap.SetTile(position, riverTile);
                }
                else if (y == -4 && x == 20) 
                {
                    tilemap.SetTile(position, cornerSmall);
                }
                else if (y == -6 && x == 18) 
                {
                    tilemap.SetTile(position, cornerLarge);
                }
                else if (y == -4 && x <= 19) 
                {
                    tilemap.SetTile(position, borderTileRight);
                }
                else if (y == -6 && x <= 19) 
                {
                    tilemap.SetTile(position, borderTileLeft);
                }
                else if (y < -4 && x == 18) 
                {
                    tilemap.SetTile(position, borderTileDown);
                }
                else if (y <= -5 && x == 20) 
                {
                    tilemap.SetTile(position, borderTileUp);
                }
                else
                {
                    tilemap.SetTile(position, defaultTile);
                }

                TileInfo tileInfo = new TileInfo(
                    defaultTile.isPlantable,
                    defaultTile.nitrogen,
                    defaultTile.phosphorus,
                    defaultTile.potassium,
                    defaultTile.humidity
                );
                SetTileInfo(position, tileInfo);
            }
        }

        // Analytics
        AnalyticsSystem.AddAnalyticLands_Hectare(this.name, (bottomLeft + topRight) / 2);

        Debug.Log($"Terreno expandido entre ({bottomLeft.x}, {bottomLeft.y}) e ({topRight.x}, {topRight.y})");
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

    public void LoadTile(Vector3Int position, string tileType, TileInfo tileInfo, string instantiatedObjectName)
    {
        // Definir o Tile no Tilemap
        TileBase tile = GetTileByName(tileType);
        tilemap.SetTile(position, tile);

        // Restaurar o TileInfo no dicionário
        SetTileInfo(position, tileInfo);

        // Verificar se o tile é plantável e alterar para o estado arado, se necessário
        if (tile is CustomTileBase customTile && tileInfo.isPlantable)
        {
            customTile.ChangeToPlowedState(position);
        }

        if (tile is CustomTileBase customTile2)
        {
            Debug.Log("CUSTOMTIEL2");
            customTile2.RecreateTileObjects(position);
        }

        // Verificar e limpar objetos antigos antes de instanciar novos
        GameObject existingObject = GetInstantiatedTile(position);
        if (existingObject != null)
        {
            Destroy(existingObject); // Remove o objeto antigo
        }

        // Restaurar o objeto instanciado, se aplicável
        if (!string.IsNullOrEmpty(instantiatedObjectName))
        {
            GameObject prefab = GetPrefabForTile(tileType);
            if (prefab != null)
            {
                // Instancia o objeto como filho do Tilemap
                Transform parent = tilemap.transform;

                GameObject instantiatedObject = Instantiate(prefab, parent);
                instantiatedObject.name = instantiatedObjectName;
                instantiatedObject.transform.position = tilemap.CellToWorld(position);

                // Atualiza o dicionário com o novo objeto
                SetInstantiatedTile(position, instantiatedObject);
            }
        }
    }



    public TileBase GetTileByName(string tileType)
    {
        switch (tileType)
        {
            case "FarmTile": return defaultTile;
            case "TreeTile": return treeTile;
            case "WaterTile": return riverTile;
            case "CornerTileSmall": return riverTile;
            case "CornerTileLarge": return riverTile;
            case "RiverTileLeft": return borderTileLeft;
            case "RiverTileRight": return borderTileRight;
            case "RiverTileUp": return borderTileUp;
            case "RiverTile": return borderTileDown;

            default: return null;
        }
    }

    public GameObject GetPrefabForTile(string tileType)
    {
        return null;
       /* switch (tileType)
        {
            case "FarmTile": return defaultTile;
            case "TreeTile": return treeTile;
            case "WaterTile": return riverTile;
            case "CornerTileSmall": return riverTile;
            case "CornerTileLarge": return riverTile;
            case "RiverTileLeft": return borderTileLeft;
            case "RiverTileRight": return borderTileRight;
            case "RiverTileUp": return borderTileUp;
            case "RiverTile": return borderTileDown;

            default: return null;
        } */
    }


    // Define as informações de um tile em uma posição específica
    public void SetTileInfo(Vector3Int position, TileInfo info)
    {
        tileInfoDictionary[position] = info;
    }

    // Associa um objeto instanciado a uma posição de tile
    public void SetInstantiatedTile(Vector3Int position, GameObject tileObject)
    {
        instantiatedTileDictionary[position] = tileObject;
    }

    // Obtém o objeto instanciado em uma posição de tile
    public GameObject GetInstantiatedTile(Vector3Int position)
    {
        instantiatedTileDictionary.TryGetValue(position, out GameObject tileObject);
        return tileObject;
    }

    // Verifica se existe um objeto instanciado em um tile
    public bool HasInstantiatedTile(Vector3Int position)
    {
        return instantiatedTileDictionary.ContainsKey(position);
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
