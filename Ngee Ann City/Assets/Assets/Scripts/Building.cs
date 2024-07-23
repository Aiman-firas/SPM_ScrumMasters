using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuildingType
{
    Residential,
    Industry,
    Commercial,
    Park,
    Road,
    Base
}

public class Building : MonoBehaviour
{
    // Correct placement of SerializeField within the class and marked as private
    [SerializeField] private GameObject highlight;

    public BuildingType buildingType;

    void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    public void OnMouseDown()
    {
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;
        UIManagerHelper.SetInputFieldsForManager(posX, posY);
    }

}
