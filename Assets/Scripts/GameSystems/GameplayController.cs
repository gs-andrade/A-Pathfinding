using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    public Vector2 GridWorldSize;
    public float NodeRadius;

    public Vector2 StartNode;
    public Vector2 EndNode;

    private IGridWorldControl gridWorldControl;
    private IGridVisual gridVisual;
    private IPathfinding pathfinding;
    private IGridWorldGetData gridWorlddata;
    public void Initialize(IGridWorldControl gridWorld, IGridVisual gridVisual, IPathfinding pathfinding)
    {
        gridWorldControl = gridWorld;
        this.gridVisual = gridVisual;
        this.pathfinding = pathfinding;


#if UNITY_EDITOR
        gridWorlddata = gridWorld as IGridWorldGetData;
        CreateWorld(false);
#endif
    }

    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.X))
        {
            CreateWorld();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            pathfinding.DebugFindPath(StartNode, EndNode);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var node = gridWorlddata.GetNode(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if(node != null)
            {
                Debug.Log($"Node:{node.keyX},{node.keyY}");
            }

        }
#endif

    }

    private void CreateWorld(bool destroyPrefabs = true)
    {
        if (destroyPrefabs)
            gridVisual.DeleteNodePrefabInstance();

        gridWorldControl.CreateGrid(GridWorldSize, NodeRadius);
        gridVisual.CreateNodePrefabInstance();
    }

}
