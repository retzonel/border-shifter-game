using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryZone : MonoBehaviour
{
    public ResourceData acceptedResource;
    private bool delivered = false;
    [SerializeField] private Image icon;
    [SerializeField] private AudioClip onDeliverySFX;

    private void Start()
    {
        if (icon != null) icon.sprite = acceptedResource?.sprite;
        icon.transform.parent.transform.gameObject.SetActive(true);
    }

    public bool Accepts(ResourceData resource)
    {
        return !delivered && resource.resourceType == acceptedResource.resourceType;
    }

    public void Deliver(ResourceData resource)
    {
        //the resource has been delivered
        delivered = true;
        EventBus.GameE.OnResourceDelivered?.Invoke(resource);
        AudioManager.Instance?.PlaySound(onDeliverySFX);

        //wait a bit, animate with dotween and disanble the icon parent gameobject
        icon.gameObject.transform.parent?.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            if (icon.gameObject.transform.parent != null)
                icon.gameObject.transform.parent.gameObject.SetActive(false);
        });
    }
}