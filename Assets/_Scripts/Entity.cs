using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Entity", fileName = "NewEntity")]
public class Entity : ScriptableObject
{
    public List<Property> Properties { get { return _properties; } }

    [SerializeField] private List<Property> _properties = new();

}
