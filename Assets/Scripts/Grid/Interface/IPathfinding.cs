using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathfinding 
{
    void DebugFindPath(Vector2 DebugStartNode, Vector2 DebugEndNode);
    List<Node> FindPath(Vector2 startPos, Vector2 targetPos);

    List<Node> FindPath(Node startNode, Node targetNode);
}
