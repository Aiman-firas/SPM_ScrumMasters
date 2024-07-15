using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    public Button[] allButtons; // Assign all 5 buttons in the inspector

    void Start()
    {
        SelectRandomButtons();
    }

    public void SelectRandomButtons()
    {
        // First, disable all buttons and hide them
        foreach (var button in allButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Shuffle the array and take the first two to enable
        List<Button> shuffledButtons = allButtons.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < 2; i++)
        {
            shuffledButtons[i].gameObject.SetActive(true);
        }
    }
}
