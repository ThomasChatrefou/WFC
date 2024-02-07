using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Value", fileName = "NewValue")]
public class Value : ScriptableObject
{
    [SerializeField]
    [TextArea]
    private string _description = null;
}
