using System;
using UnityEngine;

public class BorderManager : MonoBehaviour
{
    public static BorderManager Instance { get; private set; }

    [SerializeField] private Border borders;
    [SerializeField] private GameObject fullLandGameObject;
    [SerializeField] private Explodable explodable;
    
    [SerializeField] private AudioClip[] borderCrashSFX;

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
        fullLandGameObject?.SetActive(true);
        explodable.explode();
        ExplosionForce explosionForce = GameObject.FindFirstObjectByType<ExplosionForce>();
        if (explosionForce != null) explosionForce.doExplosion(borders.transform.position);

        foreach (var sfx in borderCrashSFX)
        {
            AudioManager.Instance?.PlaySound(sfx);
        }
    }
}