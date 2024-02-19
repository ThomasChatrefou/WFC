using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Text;
using UnityEngine.UI;

public class DescriptionUI : MonoBehaviour
{
    public EntityHandler entityHandler;
    public Description descriptionTemplate;

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
        string[] generationDescriptions = entityHandler.DescribeMeWithBio();
        StringBuilder descriptionBuilder = new StringBuilder();

        foreach (string generationDescription in generationDescriptions)
        {
            descriptionBuilder.AppendLine(generationDescription);
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
            DescriptionPropertyController[] descriptionPropertyControllers = child.GetComponentsInChildren<DescriptionPropertyController>();
            foreach (DescriptionPropertyController controller in descriptionPropertyControllers)
            {
                controller.RerollClicked -= OnRerollClicked;
                controller.LockClicked -= OnLockClicked;
            }
            Destroy(child.gameObject);
        }

        // add ui prefab for each description
        for (int i = 0; i < generationDescriptions.Length; i++)
        {
            AddDescriptionUI(generationDescriptions[i], i);
        }
    }

    private void AddDescriptionUI(string description, int propertyIndex)
    {
        GameObject descriptionUI = Instantiate(descriptionPrefab, descriptionContainer);

        // set ui text
        TextMeshProUGUI descriptionText = descriptionUI.GetComponentInChildren<TextMeshProUGUI>();
        if (descriptionText != null)
        {
            descriptionText.text = description;
        }

        DescriptionPropertyController[] descriptionPropertyControllers = descriptionUI.GetComponentsInChildren<DescriptionPropertyController>();
        foreach (DescriptionPropertyController controller in descriptionPropertyControllers)
        {
            controller.Index = propertyIndex;
            controller.RerollClicked += OnRerollClicked;
            controller.LockClicked += OnLockClicked;
            controller.Locked = entityHandler.IsPropertyLocked(propertyIndex);
        }
    }

    private void OnRerollClicked(int propertyIndex)
    {
        entityHandler.RerollProperty(propertyIndex);
        _mainDescription.UpdateSingleGenerationText();
        GenerateGameObjects();
    }

    private void OnLockClicked(int propertyIndex)
    {
        entityHandler.ToggleLockProperty(propertyIndex);
    }

    [SerializeField]
    private DescriptionUI _mainDescription;
}
