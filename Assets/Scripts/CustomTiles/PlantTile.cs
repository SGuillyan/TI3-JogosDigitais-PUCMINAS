using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

[CreateAssetMenu(menuName = "Tiles/Plant Tile")]
public class PlantTile : Tile
{
    public Sprite[] growthSprites;  // Array para armazenar os sprites de cada fase de crescimento
    public float[] growthTimes;  // Array para armazenar o tempo necessário para cada fase de crescimento
    public bool isPlanted = false;  // Indica se a planta foi plantada
    public bool isFullyGrown = false;  // Indica se a planta atingiu o estágio final de crescimento

    private int growthStage = 0;  // Fase de crescimento atual

    // Método para iniciar o crescimento ao plantar a semente
    public void Plant(Tilemap tilemap, Vector3Int position, MonoBehaviour caller)
    {
        isPlanted = true;
        isFullyGrown = false;
        growthStage = 0;
        UpdateSprite(tilemap, position);
        caller.StartCoroutine(Grow(tilemap, position));  // Inicia a corrotina de crescimento através de um MonoBehaviour
    }

    private IEnumerator Grow(Tilemap tilemap, Vector3Int position)
    {
        while (growthStage < growthSprites.Length - 1)
        {
            yield return new WaitForSeconds(growthTimes[growthStage]);
            growthStage++;
            UpdateSprite(tilemap, position);
            tilemap.RefreshTile(position);  // Atualiza o tile no Tilemap
        }

        // Quando o crescimento estiver completo
        isFullyGrown = true;
    }

    // Método para atualizar o sprite conforme a fase de crescimento
    private void UpdateSprite(Tilemap tilemap, Vector3Int position)
    {
        if (growthSprites != null && growthSprites.Length > 0)
        {
            this.sprite = growthSprites[growthStage];
        }
    }

    // Método para coletar a planta quando estiver completamente crescida
    public void Collect(Tilemap tilemap, Vector3Int position)
    {
        if (isFullyGrown)
        {
            // Aqui você pode implementar a lógica para coletar a planta
            // Exemplo: restaurar o tile original, adicionar itens ao inventário, etc.
            tilemap.SetTile(position, null);  // Remove o tile (ou substitui por outro tile, se necessário)
        }
    }

    // Método para resetar a planta
    public void ResetGrowth()
    {
        isPlanted = false;
        isFullyGrown = false;
        growthStage = 0;
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = this.sprite;
        tileData.color = Color.white;
    }
}
