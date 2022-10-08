using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInjector : MonoBehaviour
{
    private GridWorld gridWorld;
    private GridVisual gridVisual;
    private Pathfinding pathfinding;
    private GameplayController gameplayController;
    private void Awake()
    {
        InitializeGrid();
        gameplayController = GetComponentInChildren<GameplayController>();// change this
    }

    private void Start()
    {
        gameplayController.Initialize(gridWorld, gridVisual, pathfinding);
    }


    private void InitializeGrid()
    {
        gridWorld = new GridWorld();
        gridVisual = new GridVisual(gridWorld, GetNodePrefab());
        pathfinding = new Pathfinding(gridWorld, gridVisual);

        GameObject GetNodePrefab()
        {
            return Resources.Load("Prefab/Node") as GameObject;
        }
    }

    private void OnDrawGizmos()
    {
        if (gridWorld != null && gridWorld.grid != null)
        {
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(gridWorld.GetXSize(), gridWorld.GetYSize(), 0));

            foreach (Node n in gridWorld.grid)
            {
                Gizmos.color = (n.IsWalkable()) ? Color.green : Color.red;

                Gizmos.DrawWireCube(n.WorldPosition, Vector3.one * gridWorld.debugNodeRadius * 2);
            }

        }
    }
}
