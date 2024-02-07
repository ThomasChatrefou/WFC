using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class EntityHandler : MonoBehaviour
{
    [SerializeField] private Entity _entity;

    [Button]
    public void Generate()
    {
        Generator.Generate(out _generatedValues, _entity.Properties);
        DescribeMe();
    }

    private void DescribeMe()
    {
        string result = string.Empty;
        for (int i = 0; i < _generatedValues.Count; ++i)
        {
            result += _entity.Properties[i].Description + " " + _generatedValues[i].Description + ".\n";
        }
        Debug.Log(result);
    }

    private List<Value> _generatedValues = null;
}
