using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LinkData", fileName = "NewLinkData")]
public class LinkData : ScriptableObject
{
    [SerializedDictionary("Value", "probability")]
    public SerializedDictionary<Value, float> Links = new();
}
