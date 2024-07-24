using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for managing UI input fields.
/// </summary>
public static class UIManagerHelper
{
    /// <summary>
    /// Sets the input fields for the UI manager at the specified position.
    /// </summary>
    /// <param name="posX">The X position of the input fields.</param>
    /// <param name="posY">The Y position of the input fields.</param>
    public static void SetInputFieldsForManager(int posX, int posY)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetInputFields(posX, posY);
        }
        else if (FPUIManager.Instance != null)
        {
            FPUIManager.Instance.SetInputFields(posX, posY);
        }
        else
        {
            Debug.LogError("No suitable UI manager found to handle input.");
        }
    }
}

