using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Property", fileName = "NewProperty")]
public class Property : ScriptableObject
{
    public List<Value> Values { get { return _values; } }
    public bool IsIdentifier { get { return _isIdentifier; } }
    public string Description { get { return _description; } }

    [SerializeField]
    private bool _isIdentifier;
    [SerializeField]
    private List<Value> _values = new();
    [SerializeField][TextArea]
    private string _description = null;
}
