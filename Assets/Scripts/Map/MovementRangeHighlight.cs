using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementRangeHighlight : MonoBehaviour
{
    [SerializeField]
    private Tilemap highlightTilemap;

    [SerializeField]
    private TileBase highlightTile;

    public void ClearHighlights()
    {
        highlightTilemap.ClearAllTiles();
    }

    public void HighlightTiles(IEnumerable<Vector2Int> cellPositions)
    {
        ClearHighlights();
        foreach (Vector2Int cell in cellPositions)
        {
            highlightTilemap.SetTile((Vector3Int)cell, highlightTile);
        }
    }


}
