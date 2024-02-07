using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Property", fileName = "NewProperty")]
public class Property : ScriptableObject
{
    [SerializeField]
    private bool _isIdentity;
    [SerializeField]
    private List<Value> _values = new();
    [SerializeField][TextArea]
    private string _description = null;
}
