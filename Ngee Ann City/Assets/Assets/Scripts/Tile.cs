using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a tile in the game.
/// </summary>
public class Tile : MonoBehaviour 
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    // Define isOffset as a public or serialized private field
    public bool isOffset;  // Add this line if you need to access it from other scripts

    /// <summary>
    /// Initializes the tile with the specified offset value.
    /// </summary>
    /// <param name="isOffset">The offset value for the tile.</param>
    public void Init(bool isOffset)
    {
        this.isOffset = isOffset;  // Assign the passed value to the tile's isOffset property
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    /// <summary>
    /// Called when the mouse enters the tile's collider.
    /// </summary>
    void OnMouseEnter()
    {
        _highlight.SetActive(true);
    }

    /// <summary>
    /// Called when the mouse exits the tile's collider.
    /// </summary>
    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    /// <summary>
    /// Called when the tile is clicked.
    /// </summary>
    public void OnMouseDown()
    {
        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y;
        UIManagerHelper.SetInputFieldsForManager(posX, posY);
    }
}
