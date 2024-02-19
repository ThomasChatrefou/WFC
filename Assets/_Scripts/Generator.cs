using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Generator
{
    public const float EPSILON = 0.001f;

    public struct Input
    {
        public List<Property> Properties;
        public List<Value> InitialValues;
        public HashSet<int> InitialValuesPropertyIndexes;
        public int Seed;
    }

    public static void Generate(out List<Value> outputValues, out List<List<float>> probabilities, Input input)
    {
        Random.InitState(input.Seed);

        // Start by adding the initial values so they will all participate in generation of the properties to generate
        outputValues = new() { Capacity = input.Properties.Count };
        outputValues.AddRange(input.InitialValues);

        List<int> outputIndexes = new() { Capacity = input.Properties.Count };
        outputIndexes.AddRange(input.InitialValuesPropertyIndexes);
        ListUtility.Sort(ref outputIndexes, outputIndexes);

        probabilities = new() { Capacity = input.Properties.Count };
        for (int i = 0; i < outputValues.Count; ++i)
        {
            int propertyIndex = outputIndexes[i];
            FillProbabilitiesForInitialValues(out List<float> probaPerValue, input.Properties[propertyIndex], outputValues[i]);
            probabilities.Add(probaPerValue);
        }

        int propertiesCount = input.Properties.Count;
        for (int i = 0; i < propertiesCount; i++)
        {
            if (input.InitialValuesPropertyIndexes.Contains(i))
                continue;
            
            outputValues.Add(ChooseValueForProperty(out List<float> probaPerValue, input.Properties[i], outputValues));
            outputIndexes.Add(i);
            probabilities.Add(probaPerValue);
        }

        ListUtility.Sort(ref outputValues, outputIndexes);
        ListUtility.Sort(ref probabilities, outputIndexes);
    }

    /// <summary>
    /// Single step of the generator algo. This can be used to reroll one single property.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="constraints"></param>
    /// <returns></returns>
    public static Value ChooseValueForProperty(out List<float> probaPerValue, Property property, List<Value> constraints)
    {
        probaPerValue = new() { Capacity = property.Values.Count };
        ComputeProba(ref probaPerValue, property, constraints);
        NormalizeProba(ref probaPerValue);
        int chosenIndex = Roll(probaPerValue);
        return property.Values[chosenIndex];
    }

    /// <summary>
    /// Formula of probabilities to choose any value in a single property.
    /// </summary>
    /// <param name="probaPerValue"></param>
    /// <param name="constraints"></param>
    /// <param name="property"></param>
    public static void ComputeProba(ref List<float> probaPerValue, Property property, List<Value> constraints)
    {
        foreach (Value candidate in property.Values)
        {
            float candidateProba = 0f;
            foreach (Value constraint in constraints)
            {
                // [TO THINK] Should we be adding a default probability that can be > 0 when there is no data for candidate key ?
                if (constraint.HasLinks && constraint.LinkData.Links.TryGetValue(candidate, out float linkProba))
                {
                    candidateProba += linkProba;
                }
            }
            probaPerValue.Add(candidateProba);
        }
    }

    /// <summary>
    /// Ensures that the sum of probabilities is 1.
    /// </summary>
    /// <param name="probaPerValue"></param>
    public static void NormalizeProba(ref List<float> probaPerValue)
    {
        float sum = 0f;
        foreach (float proba in probaPerValue)
        {
            sum += proba;
        }
        if (sum < EPSILON)
        {
            for (int i = 0; i < probaPerValue.Count; ++i)
            {
                probaPerValue[i] = 1f / probaPerValue.Count;
            }
        }
        else
        {
            for (int i = 0; i < probaPerValue.Count; ++i)
            {
                probaPerValue[i] /= sum;
            }
        }
    }

    /// <summary>
    /// Chooses an index in a Value list randomly according to the probabilities.
    /// </summary>
    /// <param name="probaPerValue">should be normalized</param>
    public static int Roll(List<float> probaPerValue)
    {
        List<float> cumulatedProba = new() { Capacity = probaPerValue.Count };

        float sum = 0f;
        foreach (float proba in probaPerValue)
        {
            sum += proba;
            cumulatedProba.Add(sum);
        }

        int index = 0;
        float u = Random.value;
        while (index < cumulatedProba.Count - 1 && u > cumulatedProba[index])
        {
            ++index;
        }

        return index;
    }

    public static int GenerateSeed()
    {
        return Random.Range(UInt16.MinValue, UInt16.MaxValue);
    }

    public static int FindNearestValidSeed(int seed)
    {
        return Mathf.Clamp(seed, UInt16.MinValue, UInt16.MaxValue - 1);
    }

    public static void FillProbabilitiesForInitialValues(out List<float> probaPerValue, Property property, Value value)
    {
        probaPerValue = new() { Capacity = property.Values.Count };
        foreach (Value candidate in property.Values)
        {
            if (candidate == value)
            {
                probaPerValue.Add(1.0f);
            }
            else
            {
                probaPerValue.Add(0.0f);
            }
        }
    }
}
