using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridWorldGetData 
{
    int GetXSize();

    int GetYSize();

    Node GetNode(int keyX, int keyY);

    Node GetNode(Vector2 worldPosition);
}
