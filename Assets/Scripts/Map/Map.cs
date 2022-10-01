using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    private Dictionary<Vector3Int, GameObject> buildings = new Dictionary<Vector3Int, GameObject>();

    [SerializeField]
    private Tilemap islandCollidersTilemap, seaTilemap, forestTilemap, mountainTilemap;

    private List<Vector2Int> islandTiles, forestTiles, mountainTiles, emptyTiles;

    public Dictionary<Vector2Int,Vector2Int?> GetMovementRange(Vector3 worldPosition, int currentMovementPoints)
    {
        Vector3Int cellWorldPosition = GetCellWorldPositionFor(worldPosition);
       return GraphSearch.BFS(mapGrid,(Vector2Int)cellWorldPosition, currentMovementPoints);
    }

    private bool isShowing;
    [SerializeField]
    private bool showEmpty, showForest, showMountains;

    private MapGrid mapGrid;

    private void Awake()
    {
        islandTiles = GetTilemapWorldPositionsFrom(islandCollidersTilemap);
        forestTiles = GetTilemapWorldPositionsFrom(forestTilemap);
        mountainTiles = GetTilemapWorldPositionsFrom(mountainTilemap);
        emptyTiles = GetEmptyTiles(islandTiles, forestTiles.Concat(mountainTiles).ToList());

        PrepareMapGrid();

    }

    private void PrepareMapGrid()
    {
        mapGrid = new MapGrid();
        mapGrid.AddToGrid(forestTilemap.GetComponent<TerrainTypeReference>().GetTerrainData()
            , forestTiles);
        mapGrid.AddToGrid(mountainTilemap.GetComponent<TerrainTypeReference>().GetTerrainData()
            , mountainTiles);
        mapGrid.AddToGrid(islandCollidersTilemap.GetComponent<TerrainTypeReference>().GetTerrainData()
            , emptyTiles);
    }

    public int GetMovementCost(Vector2Int cellWorldPosition)
    {
        return mapGrid.GetMovementCost(cellWorldPosition);
    }

    //public bool CanIMoveTo(Vector2 unitPosition, Vector2 direction)
    //{
    //    Vector2Int unitTilePosition = Vector2Int.FloorToInt(unitPosition + direction);

    //    List<Vector2Int> neighbors = mapGrid.GetNeighboursFor(Vector2Int.FloorToInt(unitPosition));

    //    foreach (Vector2Int cellPosition in neighbors)
    //    {
    //        Debug.Log($"neighbors are : {cellPosition}");
    //    }

    //    return neighbors.Contains(unitTilePosition) && mapGrid.CheckIfPositionValid(unitTilePosition);
    //}

    private List<Vector2Int> GetEmptyTiles(List<Vector2Int> islandTiles, List<Vector2Int> nonEmptyTiles)
    {
        HashSet<Vector2Int> emptyTilesHashset = new HashSet<Vector2Int>(islandTiles);
        emptyTilesHashset.ExceptWith(nonEmptyTiles);
        return new List<Vector2Int>(emptyTilesHashset);
    }
    private List<Vector2Int> GetTilemapWorldPositionsFrom(Tilemap tilemap)
    {
        List<Vector2Int> tempList = new List<Vector2Int>();
        foreach (Vector2Int cellPosition in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.HasTile((Vector3Int)cellPosition) == false)
                continue;

            Vector3Int worldPosition = GetWorldPositionFor(cellPosition);
            tempList.Add((Vector2Int)worldPosition);

        }
        return tempList;
    }

    private Vector3Int GetCellWorldPositionFor(Vector3 worldPosition)
    {
        return Vector3Int.CeilToInt(islandCollidersTilemap.
            CellToWorld(islandCollidersTilemap.WorldToCell(worldPosition)));
    }

    private Vector3Int GetWorldPositionFor(Vector2Int worldPosition)
    {
        return Vector3Int.CeilToInt(islandCollidersTilemap.CellToWorld((Vector3Int)worldPosition));
    }

    public void AddStructure(Vector3 worldPosition, GameObject structure)
    {
        Vector3Int position = GetCellWorldPositionFor(worldPosition);
        if (buildings.ContainsKey(position))
        {
            Debug.LogError($"There already exist a building here!{worldPosition}");
            return;
        }
        buildings[position] = structure;
    }
    public bool IsPositionInvalid(Vector3 worldPosition)
    {
        return buildings.ContainsKey(GetCellWorldPositionFor(worldPosition));
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;
        DrawGizmosOf(emptyTiles, Color.white, showEmpty);
        DrawGizmosOf(forestTiles, Color.yellow, showForest);
        DrawGizmosOf(mountainTiles, Color.red, showMountains);
    }

    private void DrawGizmosOf(List<Vector2Int> tiles, Color color, bool isShowing)
    {
        if (isShowing)
        {
            Gizmos.color = color;
            foreach (Vector2Int pos in tiles)
            {
                Gizmos.DrawSphere(new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f), 0.3f);
            }

        }
    }
}
