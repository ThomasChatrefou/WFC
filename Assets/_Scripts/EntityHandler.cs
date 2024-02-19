using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
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

    public bool HasBeenGenerated { get; private set; } = false;

    [Button]
    public void Clear()
    {
        HasBeenGenerated = false;
        _generatedValues = new();
        _lockedPropertiesIndexes = new();
        _probabilities = new();
        Debug.Log("Data cleared");
    }

    [Button]
    public void Generate()
    {
        if (!_useCustomSeed) _seed = Generator.GenerateSeed();

        ComputeInitialValues(out List<Value> initialValues);

        Generator.Generate(out _generatedValues, out _probabilities,
            new Generator.Input()
            {
                Properties = _entity.Properties,
                InitialValues = initialValues,
                InitialValuesPropertyIndexes = _lockedPropertiesIndexes,
                Seed = _seed
            });

        HasBeenGenerated = true;
        DescribeMe();
        DebugDescribeMe();
    }

    [Button]
    public void RerollProperty()
    {
        RerollProperty(_propertyToRerollIndex);
    }

    [Button]
    public void LockProperties()
    {
        _lockedPropertiesIndexes.Clear();
        string log = string.Empty;
        foreach (int propertyIndex in _prelockedProperties)
        {
            _lockedPropertiesIndexes.Add(propertyIndex);
            log += "Property Locked : " + _entity.Properties[propertyIndex].Name + $" ({propertyIndex})\n";
        }
        Debug.Log(log);
    }

    [Button]
    public void PrintProbabilities()
    {
        string log = string.Empty;
        if (HasBeenGenerated)
        {
            for (int i = 0; i < _entity.Properties.Count; ++i)
            {
                log += "Trait : " + _entity.Properties[i].Name + "\n";
                log += PrintPropertyProbabilities(i);
            }
        }
        else
        {
            log += "No data generated yet !";
        }
        Debug.Log(log);
    }

    public string PrintPropertyProbabilities(int propertyIndex)
    {
        string log = string.Empty;
        if (HasBeenGenerated)
        {
            Property currentProperty = _entity.Properties[propertyIndex];
            for (int j = 0; j < currentProperty.Values.Count; ++j)
            {
                Value currentValue = currentProperty.Values[j];
                bool isSelected = currentValue == _generatedValues[propertyIndex];
                log += currentValue.Description + $" : {100 * _probabilities[propertyIndex][j]:0} %";
                if (isSelected)
                {
                    log += "  - selected !";
                }
                log += "\n";
            }
        }
        else
        {
            log = "You should start with a generation first !";
        }
        return log;
    }

    public void RerollProperty(int propertyIndex)
    {
        Debug.Log($"rerolling property {propertyIndex} : " + _entity.Properties[propertyIndex].Description);

        if (HasBeenGenerated)
        {
            _generatedValues.RemoveAt(propertyIndex);
            _probabilities.RemoveAt(propertyIndex);
        }
        
        Value rerolled = Generator.ChooseValueForProperty(out List<float> probaPerValue, _entity.Properties[propertyIndex], _generatedValues);

        if (HasBeenGenerated)
        {
            _generatedValues.Insert(propertyIndex, rerolled);
            _probabilities.Insert(propertyIndex, probaPerValue);
        }
        else
        {
            _generatedValues.Add(rerolled);
            _probabilities.Add(probaPerValue);
        }

        DescribeMe();
    }

    public void ToggleLockProperty(int propertyIndex)
    {
        if (_lockedPropertiesIndexes.Contains(propertyIndex))
        {
            _lockedPropertiesIndexes.Remove(propertyIndex);
        }
        else
        {
            _lockedPropertiesIndexes.Add(propertyIndex);
        }
    }

    public bool IsPropertyLocked(int propertyIndex)
    {
        return _lockedPropertiesIndexes.Contains(propertyIndex);
    }

    public string[] DescribeMe()
    {
        List<string> descriptions = new List<string>();

        for (int i = 0; i < _generatedValues.Count; ++i)
        {
            string log = "Trait : " + _entity.Properties[i].Description + "\n";
            log += PrintPropertyProbabilities(i);
            descriptions.Add(log);
        }

        return descriptions.ToArray();
    }

    public string[] DescribeMeWithBio()
    {
        List<string> descriptions = new List<string>();

        for (int i = 0; i < _generatedValues.Count; ++i)
        {
            Description template = _entity.Properties[i].DescriptionTemplate;
            // select random description
            string selectedTemplate = template.templates[UnityEngine.Random.Range(0, template.templates.Length)];

            // Replace {property} and {value} by real descriptions
            string description = selectedTemplate.Replace("{property}", $"<color=green>{_entity.Properties[i].Description}</color>")
                                                .Replace("{value}", $"<color=red>{_generatedValues[i].Description}</color>");

            descriptions.Add(description + "\n");
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

    private void ComputeInitialValues(out List<Value> initialValues)
    {
        initialValues = new() { Capacity = _lockedPropertiesIndexes.Count };
        for (int i = 0; i < _generatedValues.Count; ++i)
        {
            if (_lockedPropertiesIndexes.Contains(i))
            {
                initialValues.Add(_generatedValues[i]);
            }
        }
    }

    [SerializeField]
    private Entity _entity;
    [SerializeField]
    private bool _useCustomSeed;
    [SerializeField]
    [EnableIf("_useCustomSeed")]
    private int _seed;
    [SerializeField]
    private int _propertyToRerollIndex;
    [SerializeField]
    private List<int> _prelockedProperties = new();

    private List<Value> _generatedValues = new();
    private HashSet<int> _lockedPropertiesIndexes = new();
    private List<List<float>> _probabilities = new();
}
