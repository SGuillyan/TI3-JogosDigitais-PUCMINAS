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
    public int humidity = 1000;

    // Instancia o GameObject ao invés de usar um sprite
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (!Application.isPlaying)
        {
            return true;
        }

        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Cria um objeto TileInfo com os dados deste CustomTileBase
            TileInfo info = new TileInfo(
                false,    // Inicialmente não plantável
                this.nitrogen,       // Parâmetro 'nitrogen'
                this.phosphorus,     // Parâmetro 'phosphorus'
                this.potassium,      // Parâmetro 'potassium'
                this.humidity        // Parâmetro 'humidity'
            );

            // Registra as informações no dicionário do TilemapManager
            tilemapManager.SetTileInfo(position, info);

            // Instancia o GameObject associado ao tile se ainda não estiver no dicionário
            if (!tilemapManager.HasInstantiatedTile(position) && customTilePrefab != null)
            {
                Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);  // Ajusta para centralizar o objeto
                GameObject instantiatedTile = Instantiate(customTilePrefab, worldPosition, Quaternion.identity);

                instantiatedTile.name = $"CustomTile_{position.x}_{position.y}_{position.z}";

                // Armazena a referência ao objeto instanciado
                tilemapManager.SetInstantiatedTile(position, instantiatedTile);
            }
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }

        return true;  // Indica que a inicialização foi bem-sucedida
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        // Mesmo que este Tile use um GameObject, ainda é necessário definir algumas propriedades do TileBase
        tileData.sprite = sprite;
        tileData.color = color;  // Defina a cor do tile no Tilemap
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        tilemap.RefreshTile(position);
    }

    // Exibe informações do tile para depuração
    public void DisplayTileInfo()
    {
        Debug.Log($"Tile Info: Plantable = {isPlantable}, Nitrogênio = {nitrogen}, Fósforo = {phosphorus}, Potássio = {potassium}, Humidade = {humidity}");
    }

    // Método para consumir nutrientes do solo
    public void ConsumeNutrients(int nAmount, int pAmount, int kAmount)
    {
        nitrogen = Mathf.Max(0, nitrogen - nAmount);
        phosphorus = Mathf.Max(0, phosphorus - pAmount);
        potassium = Mathf.Max(0, potassium - kAmount);
    }

    // Método para alternar para o estado de solo arado
    public void ChangeToPlowedState(Vector3Int position)
    {
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (plowedTilePrefab != null && tilemapManager != null)
        {
            // Obtém o GameObject associado ao tile atual
            GameObject currentTile = tilemapManager.GetInstantiatedTile(position);

            if (currentTile != null)
            {
                // Destrói o GameObject atual
                Destroy(currentTile);
            }

            // Converte a posição da célula do grid para uma posição no mundo
            Vector3 worldPosition = tilemapManager.tilemap.CellToWorld(position) + new Vector3(0.5f, 0, 0.5f);  // Ajusta para centralizar o objeto

            // Instancia o GameObject do solo arado
            GameObject plowedTile = Instantiate(plowedTilePrefab, worldPosition, Quaternion.identity);
            plowedTile.name = $"PlowedTile_{position.x}_{position.y}_{position.z}";

            // Atualiza a referência ao objeto instanciado no TilemapManager
            tilemapManager.SetInstantiatedTile(position, plowedTile);

            // Atualiza o estado para plantável após arar o solo
            TileInfo tileInfo = tilemapManager.GetTileInfo(position);
            if (tileInfo != null)
            {
                tileInfo.isPlantable = true;  // Agora o tile é plantável
                tilemapManager.SetTileInfo(position, tileInfo);
            }
        }
        else
        {
            Debug.LogWarning("PlowedTilePrefab ou TilemapManager não atribuído!");
        }
    }
}
