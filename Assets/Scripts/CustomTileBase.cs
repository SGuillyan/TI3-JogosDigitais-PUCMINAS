using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTileBase : TileBase
{
    // Propriedades personalizadas
    public Sprite sprite;
    public Color color = Color.white;
    public bool isPlantable = true;  // Determina se o tile é plantável
    public int nutrienteX = 100;  // Nível de nutriente X
    public int nutrienteY = 100;  // Nível de nutriente Y

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.color = color;
        // Aqui, você pode definir outras propriedades do TileData conforme necessário
    }

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        tilemap.RefreshTile(position);
    }

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        // Inicializa o comportamento específico do tile e registra as informações no TilemapManager
        TilemapManager tilemapManager = Object.FindObjectOfType<TilemapManager>();

        if (tilemapManager != null)
        {
            // Cria um objeto TileInfo com os dados deste CustomTileBase
            TileInfo info = new TileInfo()
            {
                isPlantable = this.isPlantable,
                nutrienteX = this.nutrienteX,
                nutrienteY = this.nutrienteY
            };

            // Registra as informações no dicionário do TilemapManager
            Vector3Int gridPosition = position;
            tilemapManager.SetTileInfo(gridPosition, info);
        }
        else
        {
            Debug.LogWarning("TilemapManager não encontrado na cena.");
        }

        return true;  // Indica que a inicialização foi bem-sucedida
    }

    // Método para acessar as informações do tile
    public void DisplayTileInfo()
    {
        Debug.Log($"Custom Tile Info: isPlantable = {isPlantable}, NutrienteX = {nutrienteX}, NutrienteY = {nutrienteY}");
    }
}
