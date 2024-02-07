using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Entity", fileName = "NewEntity")]
public class Entity : ScriptableObject
{
    [SerializeField] private List<Property> _properties = new();
}
