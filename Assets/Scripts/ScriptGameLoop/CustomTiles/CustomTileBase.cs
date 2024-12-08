using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTileBase : TileBase
{
    // Propriedades personalizadas
    public Sprite sprite;
    public GameObject customTilePrefab;  // Prefab do GameObject associado ao tile
    public GameObject plowedTilePrefab;  // Prefab do GameObject associado ao solo arado
    public Color color = Color.white;
    public bool isPlantable = false;  // O tile começa não plantável

    // Nutrientes NPK no tile
    public int nitrogen = 1000;
    public int phosphorus = 1000;
    public int potassium = 1000;
    public int humidity = 1000;

    // Estado de rotação (0 = 0°, 1 = 90°, 2 = 180°, 3 = 270°)
    public int rotationState = 0;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            TileInfo info = new TileInfo(
                isPlantable,
                nitrogen,
                phosphorus,
                potassium,
                humidity
            );

            tilemapManager.SetTileInfo(position, info);

            if (!tilemapManager.HasInstantiatedTile(position) && customTilePrefab != null)
            {
                Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

                Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

                // Determina a rotação com base no estado de rotação
                Quaternion rotation = Quaternion.Euler(0, rotationState * 90f, 0);

                GameObject instantiatedTile = Instantiate(customTilePrefab, worldPosition, rotation, parent);
                instantiatedTile.transform.position = worldPosition;
                instantiatedTile.name = $"CustomTile_{position.x}_{position.y}_{position.z}";

                tilemapManager.SetInstantiatedTile(position, instantiatedTile);
            }
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }

        return true;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.color = color;
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        tilemap.RefreshTile(position);
    }

    public void DisplayTileInfo()
    {
        Debug.Log($"Tile Info: Plantable = {isPlantable}, Nitrogênio = {nitrogen}, Fósforo = {phosphorus}, Potássio = {potassium}, Umidade = {humidity}");
    }

    public void ConsumeNutrients(int nAmount, int pAmount, int kAmount)
    {
        nitrogen = Mathf.Max(0, nitrogen - nAmount);
        phosphorus = Mathf.Max(0, phosphorus - pAmount);
        potassium = Mathf.Max(0, potassium - kAmount);
    }

    public void ChangeToPlowedState(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (plowedTilePrefab != null && tilemapManager != null)
        {
            GameObject currentTile = tilemapManager.GetInstantiatedTile(position);
            if (currentTile != null)
            {
                Destroy(currentTile);
            }

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            // Determina a rotação com base no estado de rotação
            Quaternion rotation = Quaternion.Euler(0, rotationState * 90f, 0);

            GameObject plowedTile = Instantiate(plowedTilePrefab, worldPosition, rotation, parent);
            plowedTile.transform.position = worldPosition;
            plowedTile.name = $"PlowedTile_{position.x}_{position.y}_{position.z}";

            tilemapManager.SetInstantiatedTile(position, plowedTile);

            TileInfo tileInfo = tilemapManager.GetTileInfo(position);
            if (tileInfo != null)
            {
                tileInfo.isPlantable = true;
                tilemapManager.SetTileInfo(position, tileInfo);
            }
        }
        else
        {
            Debug.LogWarning("PlowedTilePrefab ou TilemapManager não atribuído!");
        }
    }

    public void RevertToCustomTileState(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (customTilePrefab != null && tilemapManager != null)
        {
            GameObject currentTile = tilemapManager.GetInstantiatedTile(position);
            if (currentTile != null)
            {
                Destroy(currentTile);  // Remove o tile atual (o arado)
            }

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            Transform parent = tilemapManager.parentTransform != null ? tilemapManager.parentTransform : tilemapManager.transform;

            // Cria o tile customizado novamente
            GameObject customTile = Instantiate(customTilePrefab, worldPosition, Quaternion.identity, parent);
            customTile.transform.position = worldPosition;
            customTile.name = $"CustomTile_{position.x}_{position.y}_{position.z}";

            tilemapManager.SetInstantiatedTile(position, customTile);

            // Atualiza as informações do tile para refletir que ele voltou ao estado original (não arado)
            TileInfo tileInfo = tilemapManager.GetTileInfo(position);
            if (tileInfo != null)
            {
                tileInfo.isPlantable = this.isPlantable;  // Restaura o valor de plantabilidade
                tilemapManager.SetTileInfo(position, tileInfo);
            }
        }
        else
        {
            Debug.LogWarning("CustomTilePrefab ou TilemapManager não atribuído!");
        }
    }



    public bool HasNearbyWaterTile(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();
        if (tilemapManager == null)
        {
            Debug.LogError("TilemapManager não encontrado!");
            return false;
        }

        // Verifica em um raio de 2 tiles em torno da posição (X e Y)
        for (int xOffset = -2; xOffset <= 2; xOffset++)
        {
            for (int yOffset = -2; yOffset <= 2; yOffset++)
            {
                if (xOffset == 0 && yOffset == 0) continue; // Ignora a posição central

                Vector3Int checkPosition = new Vector3Int(position.x + xOffset, position.y + yOffset, position.z);
                TileBase nearbyTile = tilemapManager.tilemap.GetTile(checkPosition);

                if (nearbyTile is CustomTileBase customTile)
                {
                    // Verifica se é um "WaterTile" com base no nome ou em outro critério
                    if (customTile.name == "WaterTile") // Substitua "WaterTile" pelo critério que identifica os tiles de água
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

}
