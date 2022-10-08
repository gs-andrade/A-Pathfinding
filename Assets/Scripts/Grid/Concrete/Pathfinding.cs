using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : IPathfinding
{
    private List<Node> path = new List<Node>();

    readonly private IGridWorldGetData gridWorld;
    readonly private IGridVisual gridVisual;

    public Pathfinding(IGridWorldGetData gridWorld, IGridVisual gridVisual)
    {
        this.gridWorld = gridWorld;
        this.gridVisual = gridVisual;
    }
    public void DebugFindPath(Vector2 DebugStartNode, Vector2 DebugEndNode)
    {
        var startNode = gridWorld.GetNode((int)DebugStartNode.x, (int)DebugStartNode.y);
        var endNode = gridWorld.GetNode((int)DebugEndNode.x, (int)DebugEndNode.y);

        if (startNode != null && endNode != null)
        {
            if (!endNode.IsWalkable())
            {
                Debug.Log("End node is not walkable");
                return;
            }

            FindPath(startNode, endNode);
            gridVisual.ResetNodeColors();

            if(path == null || path.Count == 0)
            {
                Debug.Log("Path not found");
                return;
            }


            for(int i = 0; i < path.Count; i++)
            {
                var path = this.path[i];
                
                this.path[i].SetColor(Color.blue);
            }
        }
        else
        {
            Debug.Log("Start or end node is null");
        }
    }
    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = gridWorld.GetNode(startPos);
        Node targetNode = gridWorld.GetNode(targetPos);

        return FindPath(startNode, targetNode);
    }

    public List<Node> FindPath(Node startNode, Node targetNode)
    {
        Heap<Node> openSet = new Heap<Node>(gridWorld.GetXSize() * gridWorld.GetYSize());
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        Node currentNode = null;

        while (openSet.Count > 0)
        {
            currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);


            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return path;
            }

            UpdateAdjacentNodeCostValues();

        }

        void UpdateAdjacentNodeCostValues()
        {
            var adjacentNodes = currentNode.adjacentNodes;

            for (int i = 0; i < adjacentNodes.Count; i++)
            {
                var adjacentNode = adjacentNodes[i];

                if (!adjacentNode.IsWalkable() || closedSet.Contains(adjacentNode))
                    continue;

                int newGCostOfAdjacentNode = currentNode.gCost +
                   GetNodeDistanceCost(currentNode, adjacentNode);

                if (newGCostOfAdjacentNode < adjacentNode.gCost ||
                    !openSet.Contains(adjacentNode))
                {
                    adjacentNode.gCost = newGCostOfAdjacentNode;
                    adjacentNode.hCost = GetNodeDistanceCost(adjacentNode, targetNode);
                    adjacentNode.parent = currentNode;

                    if (!openSet.Contains(adjacentNode))
                        openSet.Add(adjacentNode);
                }

            }
        }

        Debug.LogError("Path Not Found");
        return path;
    }

    private void RetracePath(Node startNode, Node endNode)
    {
        path.Clear();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
    }
    private int GetNodeDistanceCost(Node nodeA, Node nodeB)
    {
        var distanceX = Mathf.Abs(nodeA.keyX - nodeB.keyX);
        var distanceY = Mathf.Abs(nodeA.keyY - nodeB.keyY);

        if (distanceX > distanceY)
            return distanceY * 14 + 10 * (distanceX - distanceY);
        else
            return distanceX * 14 + 10 * (distanceY - distanceX);
    }

}
