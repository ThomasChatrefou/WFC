using UnityEngine;
using TMPro;

public class DescriptionUI : MonoBehaviour
{
    public TextMeshProUGUI generationText;
    public EntityHandler entityHandler;


    public void OnGenerateButtonClicked()
    {
        entityHandler.Generate();
        UpdateGenerationText();
    }

    private void UpdateGenerationText()
    {
        string generationDescription = entityHandler.DescribeMe();
        generationText.text = generationDescription;
    }
}
