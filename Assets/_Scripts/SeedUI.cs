using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedUI : MonoBehaviour
{
    public TMP_InputField seedInputField;
    public Toggle useCustomSeedToggle;
    public EntityHandler entityHandler;

    private int currentSeed;

    void Start()
    {
        LoadSeed();
        useCustomSeedToggle.onValueChanged.AddListener(OnUseCustomSeedToggle);
    }

    void OnUseCustomSeedToggle(bool value)
    {
        seedInputField.interactable = value;
        entityHandler.UseCustomSeed = value;
    }

    public void LoadSeed()
    {
        if (useCustomSeedToggle.isOn)
        {
            // use the user's seed input
            if (int.TryParse(seedInputField.text, out int customSeed))
            {
                currentSeed = customSeed;
                entityHandler.Seed = currentSeed;
            }
            else
            {
                Debug.LogError("Error: Invalid seed!");
            }
        }
        else
        {
            currentSeed = entityHandler.Seed;
        }

        // update seed display
        seedInputField.text = currentSeed.ToString();
    }

    public void SetEntityHandlerSeed()
    {
        if (int.TryParse(seedInputField.text, out int customSeed))
        {
            entityHandler.Seed = customSeed;
        }
        else
        {
            Debug.LogError("Error: Invalid seed!");
        }
    }

    public void GetSeedFromEntityHandler()
    {
        seedInputField.text = entityHandler.Seed.ToString();
        Debug.Log("SEED text input changed");
    }
}
