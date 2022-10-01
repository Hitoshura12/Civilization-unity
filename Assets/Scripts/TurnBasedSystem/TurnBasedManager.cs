using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TurnBasedManager : MonoBehaviour
{
    private Queue<EnemyTurnTaker> enemyQueue;
    public UnityEvent OnBlockPlayerInput, OnUnblockPlayerInput;

    public void NextTurn()
    {
        Debug.Log("...Waiting");
        OnBlockPlayerInput?.Invoke();
        SystemTurn();
        EnemyTurn();

    }

    private void SystemTurn()
    {
        foreach (SystemTurnTaker item in FindObjectsOfType<SystemTurnTaker>())
        {
            item.WaitTurn();
        }
    }

    private void EnemyTurn()
    {
        enemyQueue = new Queue<EnemyTurnTaker>(FindObjectsOfType<EnemyTurnTaker>());
        StartCoroutine(EnemyTakeTurn(enemyQueue));
    }

    private IEnumerator EnemyTakeTurn(Queue<EnemyTurnTaker> enemyQueue)
    {
        yield return new WaitForSeconds(0.2f);
        while (enemyQueue.Count > 0)
        {
            //EnemyTurnTaker testTaker = enemyQueue.ToList().Find((turnTaker) => turnTaker.name == "Saad");
            //EnemyTurnTaker[] arrays = new EnemyTurnTaker[5] { testTaker, null, testTaker, null, null };
            //Debug.Log(arrays.GetLength(0));
            //arrays.Distinct();
            //foreach (var turners in arrays)
            //{
            //    Debug.Log(turners);
            //}
            //Debug.Log(testTaker+ "Saad Found!!!");
            EnemyTurnTaker turnTaker = enemyQueue.Dequeue();
            turnTaker.TakeTurn();
            yield return new WaitUntil(turnTaker.IsTurnFinished);
            turnTaker.Reset();
           

        }
        Debug.Log("PlayerTurn READY!");
        PlayerTurn();

    }

    private void PlayerTurn()
    {
        foreach (PlayerTurnTaker turnTaker in FindObjectsOfType<PlayerTurnTaker>())
        {
            turnTaker.WaitTurn();
            Debug.Log($"unit {turnTaker.name} is waiting.");
        }
        Debug.Log("Turn ready!");
        OnUnblockPlayerInput?.Invoke();
    }
}

public interface ITurnDependant
{
    void WaitTurn();
}
