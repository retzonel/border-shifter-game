using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class VFX_Manager : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private void Start()
    {
        EventBus.GameE.PlayVFX += PlayVFX;
        EventBus.GameE.OnScreenShake += OnScreenShake;
        
    }

    private void OnDestroy()
    {
        EventBus.GameE.PlayVFX -= PlayVFX;
        EventBus.GameE.OnScreenShake -= OnScreenShake;
    }

    private void OnScreenShake(float intensity = 1f)
    {
        impulseSource.GenerateImpulse(intensity);
    }

    private void PlayVFX(GameObject vfxPrefab, Vector3 position)
    {
        if (vfxPrefab == null) return;

        GameObject vfx = Instantiate(vfxPrefab, position, Quaternion.identity);

        StartCoroutine(DisableObject(vfx));
    }

    private IEnumerator DisableObject(GameObject obj)
    {
        yield return new WaitForSeconds(2f);

        obj.SetActive(false);
    }
}