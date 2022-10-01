using System;
using UnityEngine;
using UnityEngine.Events;

public class Unit : MonoBehaviour, ITurnDependant
{

    private int currentMovementPoints;

    public UnityEvent FinishedMoving;

    private UnitData unitData;

    [SerializeField]
    private LayerMask enemyDetectionLayer;

    private AudioSource stepaudio;
    public int CurrentMovementPoints { get => currentMovementPoints; set => currentMovementPoints = value; }
    public event Action OnMove;

    private void Awake()
    {
        unitData = GetComponent<UnitData>();
        stepaudio = GetComponent<AudioSource>();
    }
    void Start()
    {
        ResetMovementPoints();
    }

    public void ResetMovementPoints()
    {
        currentMovementPoints = unitData.Data.movementRange;
    }

    public bool CanStillMove()
    {
        return currentMovementPoints > 0;
    }
    public void WaitTurn()
    {
        ResetMovementPoints();
    }
    public void HandleMovement(Vector3 cardinalDirection, int movementCost)
    {
        if (currentMovementPoints - movementCost < 0)
        {
            Debug.LogError($"Not enough movement points { currentMovementPoints } to move{ movementCost}. ");
            return;
        }

        currentMovementPoints -= movementCost;
        GameObject enemyUnit = CheckIfEnemyUnitInDirection(cardinalDirection);
        if (enemyUnit == null)
        {
            stepaudio.Play();
            transform.position += cardinalDirection;
            OnMove?.Invoke();
        }
        else
        {
            PerformAttack(enemyUnit.GetComponent<Health>());
        }

        if (currentMovementPoints <= 0)
            FinishedMoving?.Invoke();


    }

    private GameObject CheckIfEnemyUnitInDirection(Vector3 cardinalDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, cardinalDirection, 1, enemyDetectionLayer);

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    private void PerformAttack(Health health)
    {
        health.GetHit(unitData.Data.attackStrength);
        currentMovementPoints = 0;
    }


    public void DestroyUnit()
    {
        FinishedMoving?.Invoke();
        Destroy(gameObject);
    }

    public void FinishMovement()
    {
        currentMovementPoints = 0;
        FinishedMoving?.Invoke();

    }
}
