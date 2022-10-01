using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicEnemyAI : MonoBehaviour, IEnemyAI
{
    public event Action OnTurnFinished;

    private Unit unit;
    private CharacterMovement characterMovement;
    private UnitData unitData;

    private FinishedMovingFeedBack finishedMovingFeedBack;

    [SerializeField]
    private AgentOutlineFeedBack outlineFeedBack;

    [SerializeField]
    private FlashFeedBack selectionFeedback;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        characterMovement = FindObjectOfType<CharacterMovement>();
        selectionFeedback = GetComponent<FlashFeedBack>();
        finishedMovingFeedBack = GetComponent<FinishedMovingFeedBack>();
        outlineFeedBack = GetComponent<AgentOutlineFeedBack>();
    }
    public void StartTurn()
    {
        Debug.Log($"Enemy {gameObject.name} start turn");
        selectionFeedback.PlayFeedBack();
        outlineFeedBack.Select();
        //finishedMovingFeedBack.StopFeedBack();
        
        Dictionary<Vector2Int,Vector2Int?> movementRange =
             characterMovement.GetMovementRangeFor(unit);

        List<Vector2Int> path = GetPathforRandom(movementRange);

        Queue<Vector2Int> movementQueue = new Queue<Vector2Int>(path);
        
        StartCoroutine(MoveUnit(movementQueue));
    }

    private List<Vector2Int> GetPathforRandom(Dictionary<Vector2Int, Vector2Int?> movementRange)
    {
  
        List<Vector2Int> possibleDestination = movementRange.Keys.ToList();
        possibleDestination.Remove(Vector2Int.RoundToInt(transform.position));
        Vector2Int selectedDestination = possibleDestination[UnityEngine.Random.Range(0, possibleDestination.Count)];
        List<Vector2Int> listToReturn = GetPathTo(selectedDestination, movementRange);
        return listToReturn;
    }

    private List<Vector2Int> GetPathTo(Vector2Int destination, Dictionary<Vector2Int, Vector2Int?> movementRange)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(destination);
        while (movementRange[destination]!=null)
        {
            path.Add(movementRange[destination].Value);
            destination = movementRange[destination].Value;
        }
        path.Reverse();
        return path.Skip(1).ToList();
    }

    private IEnumerator MoveUnit(Queue<Vector2Int> path)
    {
        yield return new WaitForSeconds(0.5f);
        if (unit.CanStillMove()==false || path.Count<=0)
        {
            FinishMovement();
            yield break;
        }
        Vector2Int pos = path.Dequeue();
        Vector3Int direction = Vector3Int.RoundToInt(new Vector3(pos.x +0.5f, pos.y + 0.5f,0) -transform.position);
        unit.HandleMovement(direction,0);
        yield return new WaitForSeconds(0.3f);
        if (path.Count>0)
        {
            finishedMovingFeedBack.StopFeedBack();
            StartCoroutine(MoveUnit(path));
        }
        else
        {
            FinishMovement();
        }
    }

    private void FinishMovement()
    {
        selectionFeedback.StopFeedBack();
        outlineFeedBack.Deselect();
        finishedMovingFeedBack.PlayFeedBack();
        unit.WaitTurn();
        OnTurnFinished?.Invoke();
    }
}
