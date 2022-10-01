using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private Map map;

    List<Vector2Int> movementRange;

    private Unit selectedUnit;

    [SerializeField]
    private MovementRangeHighlight rangeHighlight;

    public void HandleSelection(GameObject detectedObject)
    {
        if (detectedObject == null)
        {
            ResetCharacterMovement();
            return;
        }

        if (detectedObject.CompareTag("Player"))
            selectedUnit = detectedObject.GetComponent<Unit>();
        else
            selectedUnit = null;

        if (selectedUnit == null)
            return;

        if (selectedUnit.CanStillMove())
            PrepareMovementHighlight();
        else
            rangeHighlight.ClearHighlights();
     

    }

    private void PrepareMovementHighlight()
    {
        movementRange =
             GetMovementRangeFor(selectedUnit).Keys.ToList();
        rangeHighlight.HighlightTiles(movementRange);
    }

    public Dictionary<Vector2Int, Vector2Int?> GetMovementRangeFor(Unit selectedUnit)
    {
        return map.GetMovementRange
                     (selectedUnit.transform.position, selectedUnit.CurrentMovementPoints);
    }

    public void ResetCharacterMovement()
    {
        rangeHighlight.ClearHighlights();
        selectedUnit = null;
    }

    public void HandleMovement(Vector3 endPosition)
    {
        if (selectedUnit == null)
            return;
        if (selectedUnit.CanStillMove() == false)
            return;

        Vector2 direction = CalculateMovementDirection(endPosition);

        Vector2Int unitTilePosition = 
            Vector2Int.FloorToInt((Vector2)selectedUnit.transform.position + direction);

        if (movementRange.Contains(unitTilePosition))
        {
            int cost = map.GetMovementCost(unitTilePosition);
            selectedUnit.HandleMovement(direction, cost);
            if (selectedUnit.CanStillMove())
            {
                PrepareMovementHighlight();
            }
            else
            {
                rangeHighlight.ClearHighlights();
            }
           
        }
        else
        {
            Debug.Log($"Cannot move to {direction}");
        }

    }

    private Vector2 CalculateMovementDirection(Vector3 endPosition)
    {
        Vector2 direction = (endPosition - selectedUnit.transform.position);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            float sign = Mathf.Sign(direction.x);
            direction = Vector2.right * sign;
        }
        else
        {
            float sign = Mathf.Sign(direction.y);
            direction = Vector2.up * sign;
        }

        return direction;
    }
}