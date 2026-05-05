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
    Cocoa,
    Tea,
    Coffee,
    Uranium,
    Livestock,
    Fish,
    Bauxite,
    Grain,
    Timber,
    Diamond,
    Manganese,
    Cotton,
    Phosphate
}