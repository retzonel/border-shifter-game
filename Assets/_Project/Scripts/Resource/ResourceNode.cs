using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ResourceNode : MonoBehaviour
{
    public ResourceData data;
    public bool isPickedUp = false;

    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (data != null && data.sprite != null) 
        {
            spriteRenderer.sprite = data.sprite;
        }
        else
        {
            Debug.LogWarning("nulref for " + gameObject.name);
        }
    }

    public void OnPickUp()
    {
        isPickedUp = true;
        gameObject.SetActive(false);
    }
}
