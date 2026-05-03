using System;
using UnityEngine;

public class BorderManager : MonoBehaviour
{
    public  static BorderManager Instance { get; private set; }
    
    [SerializeField] private Border[] borders;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CollapseBorders()
    {
        foreach (var border in borders)
        {
            border.Collapse();
        }
    }
}
