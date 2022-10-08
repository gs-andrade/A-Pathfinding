using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node : IHeapItem<Node>
{
    private bool isWalkable;
    public Vector2 WorldPosition;
    public NodePrefabInstance NodePrefabInstance;

    public int keyX;
    public int keyY;

    public int gCost; // distancia do ponto de partida
    public int hCost; // distancia até o final

    public Node parent;
    public List<Node> adjacentNodes;

    private int heapIndex;
    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }

    public Node(bool walkable, Vector2 worldPos, int keyX, int keyY)
    {
        isWalkable = walkable;
        WorldPosition = worldPos;

        this.keyX = keyX;
        this.keyY = keyY;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public void AddNodePrefabInstance(NodePrefabInstance nodePrefabInstance)
    {
        NodePrefabInstance = nodePrefabInstance;
    }

    public void SetColor(Color color)
    {
        NodePrefabInstance.SetColor(color);
    }

    public int CompareTo(Node other)
    {
        int compare = fCost.CompareTo(other.fCost);

        if(compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}

