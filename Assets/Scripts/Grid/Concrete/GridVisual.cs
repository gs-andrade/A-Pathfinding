using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisual : IGridVisual
{
    private GameObject nodePrefab;
    private Transform gridHolder;

    private IGridWorldGetData gridWorld;

    public GridVisual( IGridWorldGetData gridWorld, GameObject nodePrefab)
    {
        this.nodePrefab = nodePrefab;
        this.gridWorld = gridWorld;

        gridHolder = new GameObject("GridHolder").transform;
    }

    public void ResetNodeColors()
    {
        ExecuteActionInAllNodes(InnerResetNodeColors);

        void InnerResetNodeColors(Node node)
        {
            node.SetColor(node.IsWalkable() ? Color.white : Color.red);
        }
    }

    public void CreateNodePrefabInstance()
    {
        ExecuteActionInAllNodes(InnerCreateNodesVisual);

        void InnerCreateNodesVisual(Node node)
        {
            var nodePrefabInstance = GameObject.Instantiate(nodePrefab, gridHolder).GetComponent<NodePrefabInstance>();

            nodePrefabInstance.Setup(node.WorldPosition, node.keyX, node.keyY);

            node.AddNodePrefabInstance(nodePrefabInstance);
        }

    }

    public void DeleteNodePrefabInstance()
    {
        ExecuteActionInAllNodes(InnerDestroyNode);

        void InnerDestroyNode(Node node)
        {
            GameObject.Destroy(node.NodePrefabInstance.gameObject);
        }
    }

    private void ExecuteActionInAllNodes(Action<Node> action)
    {
        int xSize = gridWorld.GetXSize();
        int ySize = gridWorld.GetYSize();

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                action(gridWorld.GetNode(x,y));
            }
        }
    }
    //TODO: ADD POOL FOR THIS


}
