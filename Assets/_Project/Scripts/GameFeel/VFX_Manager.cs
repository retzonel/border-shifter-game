using System;
using System.Collections;
using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    void Start()
    {
        EventBus.GameE.PlayVFX += PlayVFX;
    }

    private void OnDestroy()
    {
        EventBus.GameE.PlayVFX -= PlayVFX;
    }

    private void PlayVFX(GameObject vfxPrefab)
    {
        if (vfxPrefab == null)
        {
            Debug.LogWarning("VFX_Manager: Received null VFX prefab.");
            return;
        }

        var _ = Instantiate(vfxPrefab, Vector3.zero, Quaternion.identity);
        StartCoroutine(DisableObject(_));
    }

    IEnumerator DisableObject(GameObject obj)
    {
        yield return new  WaitForSeconds(2f);
        obj.SetActive(false);
    }
}