using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurnTaker : MonoBehaviour
{
    private bool turnFinished;

    private IEnemyAI enemyAI;

    private void Start()
    {
        enemyAI = GetComponent<IEnemyAI>();
        enemyAI.OnTurnFinished += () => turnFinished = true;
    }

    public bool IsTurnFinished() => turnFinished;

    public void Reset()
    {
        turnFinished = false;
    }

    public void TakeTurn()
    {
        enemyAI.StartTurn();
    }
}

public interface IEnemyAI
{
    event Action OnTurnFinished;
    void StartTurn();
}