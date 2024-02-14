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
        public List<Value> InitialConditions;
        public int Seed;
    }

    public static void Generate(out List<Value> output, Input input)
    {
        output = new() { Capacity = input.Properties.Count };
        Random.InitState(input.Seed);

        foreach (Property property in input.Properties)
        {
            List<float> probaPerValue = new() { Capacity = property.Values.Count };
            ComputeProba(ref probaPerValue, output, property);
            NormalizeProba(ref probaPerValue);
            int chosenIndex = Roll(probaPerValue);
            output.Add(property.Values[chosenIndex]);
        }
    }

    /// <summary>
    /// Formula of probabilities to choose any value in a single property 
    /// </summary>
    /// <param name="probaPerValue"></param>
    /// <param name="output"></param>
    /// <param name="property"></param>
    public static void ComputeProba(ref List<float> probaPerValue, List<Value> output, Property property)
    {
        foreach (Value candidate in property.Values)
        {
            float candidateProba = 0f;
            foreach (Value generated in output)
            {
                // [TO THINK] Should we be adding a default probability that can be > 0 ?
                if (generated.LinkData.Links.TryGetValue(candidate, out float linkProba))
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
    /// Chooses an index in a Value list randomly according to the probabilities
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
}
