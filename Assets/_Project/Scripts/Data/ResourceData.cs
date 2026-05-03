using UnityEngine;

[CreateAssetMenu(fileName = "ResourceData", menuName = "Game/ResourceData")]
public class ResourceData : ScriptableObject
{
    public string resourceName;
    public Sprite sprite;
    public ResourceType resourceType;
}

public enum ResourceType
{
    Gold,
    Oil,
    Diamond,
    Uranium,
    Wood,
    Stone,
    Food
}