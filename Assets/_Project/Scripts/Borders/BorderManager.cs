using System;
using UnityEngine;

public class BorderManager : MonoBehaviour
{
    public  static BorderManager Instance { get; private set; }
    
    [SerializeField] private Border[] borders;
    [SerializeField] private GameObject fullLandGameObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        fullLandGameObject?.SetActive(false);
    }

    public void CollapseBorders()
    {
        foreach (var border in borders)
        {
            border.Collapse();
        }
        fullLandGameObject?.SetActive(true);
    }
}
