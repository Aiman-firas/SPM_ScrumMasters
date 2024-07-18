using System; // This namespace is necessary for Math functions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPGridManager : MonoBehaviour
{
    public static FPGridManager Instance { get; private set; }

    [SerializeField] private int _width, _height;
    [SerializeField] public Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] public GameObject baseTilePrefab;
    [SerializeField] public GameObject[] buildingPrefabs;
    [SerializeField] private GameObject uiCanvas;  // This will hold the reference to your Canvas

    public TMP_Text feedbackMessage; // Reference to a Text component used for feedback
    public TMP_Text endMessage; // Reference to a Text component used for Game End

    public Dictionary<Vector2, Tile> _tiles;
    public Dictionary<Vector2, GameObject> _buildings;

    [SerializeField] private TMP_Text turnCountText;
    [SerializeField] private TMP_Text scoreText;

    public int turnCount;
    public int scoreCount;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        _buildings = new Dictionary<Vector2, GameObject>();
        turnCount = 0;
        scoreCount = 0;
        UpdateTurnCountUI();
        UpdateScoreUI();
        GenerateGrid(5, 5); // Start with the smallest grid size
    }

    void GenerateGrid(int width, int height)
    {
        _width = width;
        _height = height;
        _tiles.Clear();
        _buildings.Clear();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.Init((x + y) % 2 == 0);
                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -20);
    }

    public void Build(int x, int y, int buildingIndex)
    {
        Vector2 position = new Vector2(x, y);

        // Check if a building already exists at this position
        if (_buildings.ContainsKey(position))
        {
            Debug.LogError("A building already exists at this position.");
            feedbackMessage.text = "A building already exists at this position.";
            feedbackMessage.color = Color.red;
            return;
        }

        if (!CanBuildAtPosition(position))
        {
            Debug.LogError("Cannot build at this position. It must be connected to existing buildings.");
            feedbackMessage.text = "Cannot build at this position. It must be connected to existing buildings.";
            feedbackMessage.color = Color.red;
            return;
        }

        ClearPosition(position);

        GameObject prefabToBuild = buildingPrefabs[buildingIndex];
        GameObject newBuilding = Instantiate(prefabToBuild, new Vector3(x, y, 0), Quaternion.identity);
        newBuilding.name = $"Tile {x} {y}";
        _buildings[position] = newBuilding;

        IncrementTurnCount();

        // Calculate score when the building is placed
        CalculateScore(newBuilding, position);

        // Check if the building is placed at the edge of the grid and expand the grid if necessary
        if (IsEdgePosition(position))
        {
            ExpandGrid();
        }

        CheckGameEnd();
    }

    private bool IsEdgePosition(Vector2 position)
    {
        return position.x == 0 || position.y == 0 || position.x == _width - 1 || position.y == _height - 1;
    }

    public void ExpandGrid()
    {
        int newWidth = _width;
        int newHeight = _height;

        if (_width == 5 && _height == 5)
        {
            newWidth = 15;
            newHeight = 15;
        }
        else if (_width == 15 && _height == 15)
        {
            newWidth = 25;
            newHeight = 25;
        }
        else
        {
            Debug.LogError("Grid has reached its maximum size.");
            return;
        }

        for (int x = 0; x < newWidth; x++)
        {
            for (int y = 0; y < newHeight; y++)
            {
                if (!_tiles.ContainsKey(new Vector2(x, y)))
                {
                    var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.name = $"Tile {x} {y}";
                    spawnedTile.Init((x + y) % 2 == 0);
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }
        }

        _width = newWidth;
        _height = newHeight;
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -20);
    }


    private void CalculateScore(GameObject newBuilding, Vector2 position)
    {
        Building buildingComponent = newBuilding.GetComponent<Building>();
        if (buildingComponent == null) return;

        int localScore = 0;

        bool hasAdjacentBuildings = false;

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (var dir in directions)
        {
            Vector2 adjacentPos = position + dir;
            if (_buildings.ContainsKey(adjacentPos))
            {
                hasAdjacentBuildings = true;
                Building adjacentBuilding = _buildings[adjacentPos].GetComponent<Building>();
                if (adjacentBuilding == null) continue;

                switch (buildingComponent.buildingType)
                {
                    case BuildingType.Residential:
                        if (adjacentBuilding.buildingType == BuildingType.Industry)
                        {
                            localScore = Math.Max(localScore, 1);
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Residential)
                        {
                            localScore += 2;
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Commercial)
                        {
                            localScore += 1;
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Park)
                        {
                            localScore += 2;
                        }
                        break;

                    case BuildingType.Industry:
                        break;

                    case BuildingType.Commercial:
                        if (adjacentBuilding.buildingType == BuildingType.Commercial)
                        {
                            localScore += 2;
                        }
                        if (adjacentBuilding.buildingType == BuildingType.Residential)
                        {
                            localScore += 1;
                        }
                        break;

                    case BuildingType.Park:
                        if (adjacentBuilding.buildingType == BuildingType.Industry)
                        {
                            localScore += 0;
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Residential)
                        {
                            localScore += 2;
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Commercial)
                        {
                            localScore += 0;
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Park)
                        {
                            localScore += 2;
                        }
                        else if (adjacentBuilding.buildingType == BuildingType.Road)
                        {
                            localScore += 0;
                        }
                        break;

                    case BuildingType.Road:
                        localScore += CalculateConnectedRoadScore(position);
                        break;
                }
            }
        }

        // Ensure Industry scores 1 point even without adjacent buildings on the first turn
        if (buildingComponent.buildingType == BuildingType.Industry)
        {
            localScore += 1;
        }

        // Update global counters and UI
        scoreCount += localScore;
        UpdateScoreUI();
    }

    private int CalculateConnectedRoadScore(Vector2 roadPosition)
    {
        int score = 1; // Start with 1 to include the current road tile itself

        // Check above
        Vector2 abovePos = new Vector2(roadPosition.x, roadPosition.y + 1);
        if (_buildings.ContainsKey(abovePos) && _buildings[abovePos].GetComponent<Building>().buildingType == BuildingType.Road)
        {
            return 0;
        }

        // Check below
        Vector2 belowPos = new Vector2(roadPosition.x, roadPosition.y - 1);
        if (_buildings.ContainsKey(belowPos) && _buildings[belowPos].GetComponent<Building>().buildingType == BuildingType.Road)
        {
            return 0;
        }

        // Check left
        for (int x = (int)roadPosition.x - 1; x >= 0; x--)
        {
            Vector2 pos = new Vector2(x, roadPosition.y);
            if (_buildings.ContainsKey(pos) && _buildings[pos].GetComponent<Building>().buildingType == BuildingType.Road)
                score++;
            else
                break;
        }

        // Check right
        for (int x = (int)roadPosition.x + 1; x < _width; x++)
        {
            Vector2 pos = new Vector2(x, roadPosition.y);
            if (_buildings.ContainsKey(pos) && _buildings[pos].GetComponent<Building>().buildingType == BuildingType.Road)
                score++;
            else
                break;
        }

        return score; // Return the total score including the current tile
    }

    public void DestroyBuilding(int x, int y)
    {
        Vector2 position = new Vector2(x, y);

        // Check if there is a building to destroy at this position
        if (!_buildings.ContainsKey(position))
        {
            Debug.LogError("No building exists at this position to destroy.");
            feedbackMessage.text = "No building exists at this position to destroy.";
            feedbackMessage.color = Color.red;
            return;
        }

        ClearPosition(position);

        // Replace with base tile
        GameObject baseTile = Instantiate(baseTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
        baseTile.name = $"Tile {x} {y}";
        _tiles[position] = baseTile.GetComponent<Tile>();

        IncrementTurnCount();

        CheckGameEnd();
    }

    private void ClearPosition(Vector2 position)
    {
        if (_tiles.ContainsKey(position) && _tiles[position] != null)
        {
            Destroy(_tiles[position].gameObject);
            _tiles.Remove(position);
        }

        if (_buildings.ContainsKey(position))
        {
            Destroy(_buildings[position]);
            _buildings.Remove(position);
        }
    }

    private bool CanBuildAtPosition(Vector2 position)
    {
        if (turnCount == 0) return true;  // Can build anywhere on the first turn

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (var dir in directions)
        {
            Vector2 adjacentPos = position + dir;
            if (_buildings.ContainsKey(adjacentPos))
            {
                return true;
            }
        }
        return false;
    }

    private void IncrementTurnCount()
    {
        turnCount++;  // Increment the turn counter
        UpdateTurnCountUI();  // Update the UI with the new turn count
    }

    private void UpdateTurnCountUI()
    {
        if (turnCountText != null)
        {
            turnCountText.text = "Turn Count: " + turnCount;
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + scoreCount;
        }
    }

    public void ResetGame()
    {
        // Reset scores and counts
        scoreCount = 0;
        turnCount = 0;

        // Update UI
        UpdateScoreUI();
        UpdateTurnCountUI();

        // Clear existing buildings and tiles
        foreach (var tile in _tiles.Values)
        {
            if (tile != null)
            {
                Destroy(tile.gameObject);
            }
        }
        _tiles.Clear();

        foreach (var building in _buildings.Values)
        {
            if (building != null)
            {
                Destroy(building);
            }
        }
        _buildings.Clear();

        // Optionally regenerate the grid or set up initial state
        GenerateGrid(5, 5); // Reset to the initial smallest grid size
    }

    public void IgnoreBuild(int x, int y, int buildingIndex)
    {
        Vector2 position = new Vector2(x, y);

        ClearPosition(position);

        GameObject prefabToBuild = buildingPrefabs[buildingIndex];
        GameObject newBuilding = Instantiate(prefabToBuild, new Vector3(x, y, 0), Quaternion.identity);
        newBuilding.name = $"Tile {x} {y}";
        _buildings[position] = newBuilding;
    }

    private void CheckGameEnd()
    {
        // Check if the board is full
        bool isBoardFull = true;
        foreach (var tile in _tiles)
        {
            if (!_buildings.ContainsKey(tile.Key))
            {
                isBoardFull = false;
                break;
            }
        }

        // If the board is full, end the game
        if (isBoardFull)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        // Display the final score
        endMessage.text = "Game Over! Final Score: " + scoreCount;
        endMessage.color = Color.green;

        // Optionally, disable further actions in the game
        // Disable build and destroy buttons, etc.
    }
}
