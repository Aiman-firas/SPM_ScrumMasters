using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour 
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    // Define isOffset as a public or serialized private field
    public bool isOffset;  // Add this line if you need to access it from other scripts

    public void Init(bool isOffset)
    {
        this.isOffset = isOffset;  // Assign the passed value to the tile's isOffset property
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }


    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    public void OnMouseDown()
    {
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;
        UIManagerHelper.SetInputFieldsForManager(posX, posY);
    }

}
