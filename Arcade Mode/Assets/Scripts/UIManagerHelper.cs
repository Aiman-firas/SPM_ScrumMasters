using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIManagerHelper
{
    public static void SetInputFieldsForManager(int posX, int posY)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetInputFields(posX, posY);
        }
        else if (LoadingUIManager.Instance != null)
        {
            LoadingUIManager.Instance.SetInputFields(posX, posY);
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

