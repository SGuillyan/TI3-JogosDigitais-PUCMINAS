using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSelector : MonoBehaviour
{
    public Tilemap tilemap;  // Referência ao Tilemap
    public Tile highlightTile;  // Tile a ser usado como destaque
    private Vector3Int previousPosition;  // Armazena a última posição selecionada
    private TileBase previousTile;  // Armazena o tile original
    private bool hasPreviousTile = false;  // Verifica se já houve um tile selecionado

    void SelectTile()
    {
        if (Input.GetMouseButtonDown(0))  // Detecta clique do mouse
        {
            // Converte a posição do mouse na tela para uma posição no mundo
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = 0;

            // Converte a posição no mundo para uma célula do Tilemap
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);

            // Obter o TileBase na posição clicada
            TileBase clickedTile = tilemap.GetTile(gridPosition);

            if (clickedTile != null && clickedTile is CustomTileBase)
            {
                CustomTileBase customTile = (CustomTileBase)clickedTile;

                // Exibir informações do tile no console
                // customTile.DisplayTileInfo();

                // Aqui você pode adicionar lógica específica para interagir com o tile
            }
            else if(clickedTile != null)
            {
                // DisplayTileInfo(gridPosition, clickedTile);
            }
            else
            {
               // Debug.Log("Nenhum tile na posição: " + gridPosition);
            }
        }
    }


    void DisplayTileInfo(Vector3Int gridPosition, TileBase clickedTile)
    {
        // Exibe as informações básicas do tile no console
        Debug.Log("Tile selecionado na posição: " + gridPosition);
        Debug.Log("Nome do Tile: " + clickedTile.name);

        // Tenta obter informações adicionais do Tile (se for um Tile 2D do tipo padrão)
        Tile tileData = tilemap.GetTile<Tile>(gridPosition);
        if (tileData != null)
        {
            Debug.Log("Sprite do Tile: " + tileData.sprite.name);
            Debug.Log("Cor do Tile: " + tilemap.GetColor(gridPosition));
            // Adicione aqui qualquer outra propriedade do Tile que você queira exibir
        }
    }

    private void Update()
    {
        SelectTile();
    }
}
