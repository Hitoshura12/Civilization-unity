using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GraphSearch 
{
   public static Dictionary<Vector2Int,Vector2Int?> BFS
        (MapGrid mapGraph, Vector2Int startPoint,int movementPoints)
    {
        Dictionary<Vector2Int, Vector2Int?> visitedNodes =
            new Dictionary<Vector2Int, Vector2Int?>();
        Dictionary<Vector2Int, int> costSoFar =
            new Dictionary<Vector2Int, int>();
        Queue<Vector2Int> nodesToVisitQueue = 
            new Queue<Vector2Int>();

        nodesToVisitQueue.Enqueue(startPoint);
        costSoFar.Add(startPoint, 0);
        visitedNodes.Add(startPoint,null);
        while (nodesToVisitQueue.Count>0)
        {
            Vector2Int currentNode = nodesToVisitQueue.Dequeue();
            foreach (Vector2Int neighborPosition in mapGraph.GetNeighboursFor(currentNode))
            {
                if (mapGraph.CheckIfPositionValid(neighborPosition) == false)
                    continue;

                int nodeCost = mapGraph.GetMovementCost(neighborPosition);
                int currentCost = costSoFar[currentNode];
                int newCost = currentCost + nodeCost;

                if (newCost <= movementPoints)
                {
                    if (!visitedNodes.ContainsKey(neighborPosition))
                    {
                        visitedNodes[neighborPosition] = currentNode;
                        costSoFar[neighborPosition] = newCost;
                        nodesToVisitQueue.Enqueue(neighborPosition);
                    }
                    else if(costSoFar[neighborPosition] > newCost)
                    {
                        costSoFar[neighborPosition] = newCost;
                        visitedNodes[neighborPosition] = currentNode;
                    }
                }
            }
        }
        return visitedNodes;
    }
}
