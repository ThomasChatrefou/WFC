using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class EntityHandler : MonoBehaviour
{
    public event Action<int> SeedChanged;

    public bool UseCustomSeed
    {
        get { return _useCustomSeed; }
        set { _useCustomSeed = value; }
    }
    public int Seed
    {
        get { return _seed; }
        set
        {
            _seed = Generator.FindNearestValidSeed(value);
            SeedChanged?.Invoke(_seed);
        }
    }


    [Button]
    public void Generate()
    {
        if (!_useCustomSeed) _seed = Generator.GenerateSeed();

        Generator.Generate(out _generatedValues, 
            new Generator.Input()
            {
                Properties = _entity.Properties,
                Seed = _seed
            });

        DebugDescribeMe();
    }

    public string[] DescribeMe()
    {
        List<string> descriptions = new List<string>();

        for (int i = 0; i < _generatedValues.Count; ++i)
        {
            string description = _entity.Properties[i].Description + " " + _generatedValues[i].Description;
            descriptions.Add(description);
        }

        return descriptions.ToArray();
    }

    private void DebugDescribeMe()
    {
        string result = string.Empty;
        for (int i = 0; i < _generatedValues.Count; ++i)
        {
            result += _entity.Properties[i].Description + " " + _generatedValues[i].Description + ".\n";
        }

        Debug.Log(result);
    }


    [SerializeField]
    private Entity _entity;
    [SerializeField]
    private bool _useCustomSeed;
    [SerializeField]
    [EnableIf("_useCustomSeed")]
    private int _seed;

    public List<Value> _generatedValues;
}
