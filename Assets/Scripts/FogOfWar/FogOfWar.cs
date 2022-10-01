using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogOfWar : MonoBehaviour
{
    [SerializeField]
    private Tilemap seaTilemap, fowTilemap;

    [SerializeField]
    private TileBase fowTile;

    private void Awake()
    {
        fowTilemap.size = seaTilemap.size;

        fowTilemap.BoxFill(
            seaTilemap.cellBounds.min, fowTile,
            seaTilemap.cellBounds.xMin, seaTilemap.cellBounds.yMin,
            seaTilemap.cellBounds.xMax,seaTilemap.cellBounds.yMax);
    }

    public void ClearPositions(List<Vector2> positionsToClear)
    {
        foreach (Vector2 position in positionsToClear)
        {
            fowTilemap.SetTile(fowTilemap.WorldToCell(position), null);
        }
    }
}
