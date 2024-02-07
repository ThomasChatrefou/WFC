using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Value", fileName = "NewValue")]
public class Value : ScriptableObject
{
    public int Data { get { return _data; } }
    public LinkData LinkData { get { return _linkData; } }

    [SerializeField]
    [TextArea]
    private string _description = null;

    [SerializeField] private int _data;
    [SerializeField] private LinkData _linkData;
}