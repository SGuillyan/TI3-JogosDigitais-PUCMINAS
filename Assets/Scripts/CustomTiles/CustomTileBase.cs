using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Custom Tile", menuName = "Tiles/Custom Tile")]
public class CustomTileBase : TileBase
{
    // Propriedades personalizadas
    public Sprite sprite;
    public Color color = Color.white;
    public bool isPlantable = true;

    // Nutrientes NPK no tile
    public int nitrogen = 100;  // Nível de Nitrogênio (N)
    public int phosphorus = 100;  // Nível de Fósforo (P)
    public int potassium = 100;  // Nível de Potássio (K)

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = sprite;
        tileData.color = color;
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
            TileInfo info = new TileInfo(
                this.isPlantable,    // Parâmetro 'isPlantable'
                this.nitrogen,       // Parâmetro 'nitrogen'
                this.phosphorus,     // Parâmetro 'phosphorus'
                this.potassium       // Parâmetro 'potassium'
            );

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


    public void DisplayTileInfo()
    {
        Debug.Log($"Tile Info: Plantable = {isPlantable}, Nitrogênio = {nitrogen}, Fósforo = {phosphorus}, Potássio = {potassium}");
    }

    // Método para consumir nutrientes do solo
    public void ConsumeNutrients(int nAmount, int pAmount, int kAmount)
    {
        nitrogen = Mathf.Max(0, nitrogen - nAmount);
        phosphorus = Mathf.Max(0, phosphorus - pAmount);
        potassium = Mathf.Max(0, potassium - kAmount);
    }
}
