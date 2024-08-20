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
        // Inicialize comportamentos específicos aqui
        // Como exemplo, poderíamos instanciar um GameObject ou configurar parâmetros iniciais
        return true;  // Retornar true indica que a inicialização foi bem-sucedida
    }

    // Método para acessar as informações do tile
    public void DisplayTileInfo()
    {
        Debug.Log($"Custom Tile Info: isPlantable = {isPlantable}, NutrienteX = {nutrienteX}, NutrienteY = {nutrienteY}");
    }
}
