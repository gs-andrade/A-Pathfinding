using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridWorld : IGridWorldControl, IGridWorldGetData
{

    public Node[,] grid;
    private int gridSizeX;
    private int gridSizeY;

    private Vector2 gridWorldSize;
    public float debugNodeRadius;
    public void CreateGrid(Vector2 gridWorldSize, float nodeRadius)
    {
        debugNodeRadius = nodeRadius;

        this.gridWorldSize = gridWorldSize;

        var nodeDiameter = nodeRadius * 2;

        grid = GetGridArraySize();

        Vector2 gridBottomLeftPosition = GetGridBottomLeftWorldPosition();

        InstantiateGrid();

        CacheAdjacentNodes();

        Node[,] GetGridArraySize()
        {
            if (nodeRadius <= 0)
            {
                Debug.LogError("NodeRadius is 0! Setting to 1");
                nodeRadius = 1;
            }

            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

            return new Node[gridSizeX, gridSizeY];
        }

        Vector3 GetGridBottomLeftWorldPosition()
        {
            return 
            - Vector3.right * gridWorldSize.x / 2
            - Vector3.up * gridWorldSize.y / 2;
        }

        void InstantiateGrid()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector2 nodeWorldPosition = gridBottomLeftPosition
                        + Vector2.right * (x * nodeDiameter + nodeRadius)
                        + Vector2.up * (y * nodeDiameter + nodeRadius);

                    grid[x, y] = new Node(true, nodeWorldPosition, x, y);
                }
            }
        }

        void CacheAdjacentNodes()
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    var node = grid[x, y];
                    node.adjacentNodes = GetAdjacentNodes(node);
                }
            }

            List<Node> GetAdjacentNodes(Node node)
            {
                List<Node> adjacentNodes = new List<Node>();

                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (x == 0 && y == 0)
                            continue;

                        int checkX = node.keyX + x;
                        int checkY = node.keyY + y;

                        if (!IsNodeInsideGridArea(checkX, checkY))
                            continue;
                        else
                            adjacentNodes.Add(grid[checkX, checkY]);
                    }
                }

                return adjacentNodes;
            }

        }


    }

    public int GetXSize()
    {
        return gridSizeX;
    }


    public int GetYSize()
    {
        return gridSizeY;
    }

    public Node GetNode(int x, int y)
    {
        if (IsNodeInsideGridArea(x, y))
            return grid[x, y];

        return grid[0, 0];
    }

    public Node GetNode(Vector2 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        float singleNodePercentageValueX = 1f / gridSizeX;
        float singleNodePercentageValueY = 1f / gridSizeY;

        int x = Mathf.FloorToInt(percentX / singleNodePercentageValueX);
        int y = Mathf.FloorToInt(percentY / singleNodePercentageValueY);

        if (IsNodeInsideGridArea(x, y))
            return grid[x, y];

        return grid[0, 0];
    }

    private bool IsNodeInsideGridArea(int keyX, int keyY)
    {
        if (keyX < 0 || keyY < 0 || keyX >= grid.GetLength(0) || keyY >= grid.GetLength(1))
        {
            return false;
        }

        return true;
    }
}


