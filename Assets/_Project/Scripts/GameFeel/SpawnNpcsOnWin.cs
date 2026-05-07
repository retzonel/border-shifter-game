using System;
using UnityEngine;

public class SpawnNpcsOnWin : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 5f;
    [SerializeField] private GameObject[] npcPrefab;
    [SerializeField] private int maxNpcCount = 6;
    [SerializeField] private int minNpcCount = 3;

    private void Start()
    {
        EventBus.GameE.OnWinLevel += SpawnNpcs;
    }

    private void OnDestroy()
    {
        EventBus.GameE.OnWinLevel -= SpawnNpcs;
    }
    
    private void SpawnNpcs()
    {
        int npcCount = UnityEngine.Random.Range(minNpcCount, maxNpcCount + 1);

        for (int i = 0; i < npcCount; i++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomCircle.x,
                transform.position.y + randomCircle.y, // Y not Z
                transform.position.z                   // Z stays fixed
            );

            GameObject npcToSpawn = npcPrefab[UnityEngine.Random.Range(0, npcPrefab.Length)];
            Instantiate(npcToSpawn, spawnPosition, Quaternion.identity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, spawnRadius);
    }
}