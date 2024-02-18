using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Property", fileName = "NewProperty")]
public class Property : ScriptableObject
{
    public List<Value> Values { get { return _values; } }
    public bool IsIdentifier { get { return _isIdentifier; } }
    public string Name { get { return _name; } }
    public string Description { get { return _description; } }
    public Description DescriptionTemplate { get { return _descriptionTemplate; } }


    [SerializeField]
    private bool _isIdentifier;
    [SerializeField]
    private List<Value> _values = new();
    [SerializeField]
    private string _name = null;
    private Description _descriptionTemplate;
    [SerializeField][TextArea]
    private string _description = null;
}
