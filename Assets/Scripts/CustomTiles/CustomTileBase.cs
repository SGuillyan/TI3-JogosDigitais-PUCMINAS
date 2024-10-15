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
    public int nitrogen = 1000;  // Nível de Nitrogênio (N)
    public int phosphorus = 1000;  // Nível de Fósforo (P)
    public int potassium = 1000;  // Nível de Potássio (K)
    public int humidity = 1000;  // Nível de umidade

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Cria e registra as informações do tile no TilemapManager
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

                // Define o Transform pai (se disponível)
                Transform parent = tilemapManager.parentTransform != null ? 
                    tilemapManager.parentTransform : 
                    tilemapManager.transform;

                // Instancia o GameObject como filho do objeto pai
                GameObject instantiatedTile = Instantiate(customTilePrefab, worldPosition, Quaternion.identity, parent);

                // Garante a posição correta no mundo
                instantiatedTile.transform.position = worldPosition;

                instantiatedTile.name = $"CustomTile_{position.x}_{position.y}_{position.z}";

                // Armazena a referência do objeto instanciado
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
        // Define propriedades básicas para o TileBase
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
            // Destrói o GameObject atual associado ao tile, se existir
            GameObject currentTile = tilemapManager.GetInstantiatedTile(position);
            if (currentTile != null)
            {
                Destroy(currentTile);
            }

            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);

            // Define o Transform pai (se disponível)
            Transform parent = tilemapManager.parentTransform != null ? 
                tilemapManager.parentTransform : 
                tilemapManager.transform;

            // Instancia o GameObject do solo arado como filho do pai especificado
            GameObject plowedTile = Instantiate(plowedTilePrefab, worldPosition, Quaternion.identity, parent);

            // Garante a posição correta no mundo
            plowedTile.transform.position = worldPosition;

            plowedTile.name = $"PlowedTile_{position.x}_{position.y}_{position.z}";

            // Armazena a referência ao novo tile no TilemapManager
            tilemapManager.SetInstantiatedTile(position, plowedTile);

            // Atualiza o estado do tile para plantável
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
}
