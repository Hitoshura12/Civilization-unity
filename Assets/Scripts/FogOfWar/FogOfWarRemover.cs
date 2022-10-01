using System.Collections.Generic;
using UnityEngine;

public class FogOfWarRemover : MonoBehaviour
{
    private Unit unit;
    public static FogOfWar fogOfWar;
    private int range = 4;

    private void Start()
    {
        unit = GetComponent<Unit>();
        if (fogOfWar == null)
        {
            fogOfWar = FindObjectOfType<FogOfWar>();
        }
        ClearPositions();
        unit.OnMove += ClearPositions;
    }
    private void ClearPositions()
    {
        List<Vector2> positionsToClear = CalculatePositionsAroundUnit();
        fogOfWar.ClearPositions(positionsToClear);
    }

    private List<Vector2> CalculatePositionsAroundUnit()
    {
        List<Vector2> positions = new List<Vector2>();
        Vector2 centerPosition = new Vector2(transform.position.x, transform.position.y);
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector2 tempPosition = centerPosition + new Vector2(x, y);
                if (Vector2.Distance(centerPosition, tempPosition) <= range)
                {
                    positions.Add(tempPosition);
                }
            }
        }
        return positions;
    }
}
