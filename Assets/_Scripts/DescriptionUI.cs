using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Text;

public class DescriptionUI : MonoBehaviour
{
    public EntityHandler entityHandler;

    [Header("Multiple GameObjects return")]
    public Transform descriptionContainer;
    public GameObject descriptionPrefab;

    [Header ("One string return")]
    public TextMeshProUGUI fullDescription;

    public void GenerateOneString()
    {
        entityHandler.Generate();
        UpdateSingleGenerationText();
    }

    public void GenerateGameObjects()
    {
        UpdateMultipleGenerationText();
    }

    private void UpdateSingleGenerationText()
    {
        string[] generationDescriptions = entityHandler.DescribeMe();
        StringBuilder descriptionBuilder = new StringBuilder();

        foreach (string generationDescription in generationDescriptions)
        {
            descriptionBuilder.AppendLine(generationDescription + ".");
        }

        string description = descriptionBuilder.ToString().TrimEnd();
        this.fullDescription.text = description;
    }

    private void UpdateMultipleGenerationText()
    {
        string[] generationDescriptions = entityHandler.DescribeMe(); 

        // remove previous elements
        foreach (Transform child in descriptionContainer)
        {
            Destroy(child.gameObject);
        }

        // add ui prefab for each description
        foreach (string description in generationDescriptions)
        {
            AddDescriptionUI(description);
        }
    }

    private void AddDescriptionUI(string description)
    {
        GameObject descriptionUI = Instantiate(descriptionPrefab, descriptionContainer);

        // set ui text
        TextMeshProUGUI descriptionText = descriptionUI.GetComponentInChildren<TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }
    }
}
