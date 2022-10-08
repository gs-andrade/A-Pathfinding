using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodePrefabInstance : MonoBehaviour
{
    private SpriteRenderer renderer;
    private TextMeshPro text;
    public void Setup(Vector2 worldPosition, float x, float y)
    {
        transform.position = worldPosition;
        renderer = GetComponent<SpriteRenderer>();

        text = GetComponentInChildren<TextMeshPro>(true);
        if (text != null)
            text.text = $"{x},{y}";

    }

    public void SetColor(Color color)
    {
        renderer.color = color;
    }
}
