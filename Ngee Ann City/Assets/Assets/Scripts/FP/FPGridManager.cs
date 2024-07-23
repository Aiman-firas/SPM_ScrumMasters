using System; // This namespace is necessary for Math functions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPGridManager : GridManager
{
    public int GridWidth => _width; // Add these properties
    public int GridHeight => _height; // Add these propertie
    public static new FPGridManager Instance { get; protected set; }

    protected override void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
    }

    public void GenerateGrid(int width, int height)
    {
        Debug.Log($"Generating grid with width: {width}, height: {height}");
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
                Debug.Log($"Spawned tile at position: {x}, {y}");
            }
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -20);
        Debug.Log($"Camera position set to: {_cam.transform.position}");
    }

    public void SetGridSize(int width, int height)
    {
        Debug.Log($"Setting grid size to width: {width}, height: {height}");
        _width = width;
        _height = height;
    }


    public override void Build(int x, int y, int buildingIndex)
    {
        Vector2 position = new Vector2(x, y);

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
        CalculateScore(newBuilding, position);

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

    private void ExpandGrid()
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

        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (var dir in directions)
        {
            Vector2 adjacentPos = position + dir;
            if (_buildings.ContainsKey(adjacentPos))
            {
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

        if (buildingComponent.buildingType == BuildingType.Industry)
        {
            localScore += 1;
        }

        scoreCount += localScore;
        base.UpdateScoreUI();
    }

    protected override void CheckGameEnd()
    {
        base.CheckGameEnd();
    }

    protected override void EndGame()
    {
        base.EndGame();
        SavePlayerScore();
    }

    protected override void IncrementTurnCount()
    {
        turnCount++;
        UpdateTurnCountUI();
    }

    protected override void SavePlayerScore()
    {
        FPSaveManager saveManager = FindObjectOfType<FPSaveManager>();
        if (saveManager != null)
        {
            saveManager.SavePlayerScore();
        }
        else
        {
            Debug.LogError("SaveManager not found in the scene.");
        }
    }

    public void BuildWithoutConditions(int x, int y, int buildingIndex)
    {
        Vector2 position = new Vector2(x, y);

        ClearPosition(position);

        GameObject prefabToBuild = buildingPrefabs[buildingIndex];
        GameObject newBuilding = Instantiate(prefabToBuild, new Vector3(x, y, 0), Quaternion.identity);
        newBuilding.name = $"Tile {x} {y}";
        _buildings[position] = newBuilding;
    }


    public virtual void ResetGame()
    {
        scoreCount = 0;
        turnCount = 0;

        UpdateScoreUI();
        UpdateTurnCountUI();

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

        GenerateGrid();
    }
}
