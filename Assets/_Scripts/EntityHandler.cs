using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class EntityHandler : MonoBehaviour
{
    public bool UseCustomSeed
    {
        get { return _useCustomSeed; }
        set { _useCustomSeed = value; }
    }
    public int Seed
    {
        get { return _seed; }
        set { _seed = Generator.FindNearestValidSeed(value); }
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
                Property currentProperty = _entity.Properties[i];
                log += "Property : " + currentProperty.Name + " => ";
                for (int j = 0; j < currentProperty.Values.Count; ++j)
                {
                    if (j > 0) log += " | ";
                    Value currentValue = currentProperty.Values[j];
                    bool isSelected = currentValue == _generatedValues[i];
                    if (isSelected)
                    {
                        log += "[ ";
                    }
                    log += currentValue.Description + $" ({_probabilities[i][j]:0.00})";
                    if (isSelected)
                    {
                        log += " ]";
                    }
                }
                log += "\n";
            }
        }
        else
        {
            log += "No data generated yet !";
        }
        Debug.Log(log);
    }

    public void RerollProperty(int propertyIndex)
    {
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

    private void DescribeMe()
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
