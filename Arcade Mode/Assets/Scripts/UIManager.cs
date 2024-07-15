using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this line to include the UI namespace
using TMPro; // This namespace is required for TextMesh Pro input fields

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TMP_InputField xInput, yInput; // References to the input fields for coordinates
    public TMP_Text feedbackMessage; // Reference to a Text component used for feedback
    public GridManager gridManager; // Reference to the GridManager

    private int selectedBuildingIndex = -1;

    void Awake()
    {
        Instance = this;
    }

    public void SetInputFields(int x, int y)
    {
        xInput.text = x.ToString();
        yInput.text = y.ToString();
        ValidateInputs();  // Optionally validate inputs right away
    }

    public void ValidateInputs()
    {
        if (int.TryParse(xInput.text, out int x) && int.TryParse(yInput.text, out int y))
        {
            if (x < 0 || y < 0 || x >= 20 || y >= 20)
            {
                feedbackMessage.text = "Coordinates out of bounds. Please enter valid values.";
                feedbackMessage.color = Color.red;
            }
            else
            {
                feedbackMessage.text = "Coordinates are valid.";
                feedbackMessage.color = Color.green;
            }
        }
        else
        {
            feedbackMessage.text = "Invalid input. Please enter numeric values.";
            feedbackMessage.color = Color.red;
        }
    }

    public void OnBuildingTypeSelected(int index)
    {
        selectedBuildingIndex = index;
    }

    public void OnBuildClicked()
    {
        if (int.TryParse(xInput.text, out int x) && int.TryParse(yInput.text, out int y))
        {
            if (selectedBuildingIndex >= 0 && selectedBuildingIndex <= 4)
            { 
                if (gridManager != null)
                    gridManager.Build(x, y, selectedBuildingIndex);
                else
                    Debug.LogError("GridManager is null!");
            }
            else
            {
                feedbackMessage.text = "Select a valid building type.";
                feedbackMessage.color = Color.red;
            }
        }
        else
        {
            feedbackMessage.text = "Enter valid x and y coordinates.";
            feedbackMessage.color = Color.red;
        }
    }

    public void OnDestroyClicked()
    {
        if (int.TryParse(xInput.text, out int x) && int.TryParse(yInput.text, out int y))
        {
            GridManager.Instance.DestroyBuilding(x, y);
        }
        else
        {
            Debug.LogError("Invalid input for coordinates.");
        }
    }

}
