using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the map
/// </summary>
public class MapManager : Singleton<MapManager>
{
    public Vector2 InitPosition { get => initCell.transform.position; }
    public List<Cell> EmptyCells { get => emptyCells; }

    private Logger LOGGER;

    /// <summary>
    /// 0 -> wall
    /// 1 -> path
    /// </summary>
    private readonly int[,] map = new int[4, 7]
    {
        { 1, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1 },
        { 0, 0, 1, 0, 1, 0, 1 },
        { 1, 1, 1, 0, 1, 0, 1 },
    };


    [Header("Content")]
    [SerializeField]
    private Transform content;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject pathPrefab;

    [SerializeField]
    private GameObject wallPrefab;

    private GameObject firstCell;

    private const string pathStr = "Path";
    private const string wallStr = "Wall";

    private Vector2 upperLeftScreen;

    private float currentHOffset = 0;
    private float currentVOffset = 0;

    private float hOffset;
    private float vOffset;

    private Transform initCell;

    private List<Cell> emptyCells;

    private void Start()
    {
        LOGGER = new Logger(gameObject);

        emptyCells = new List<Cell>();

        upperLeftScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.safeArea.xMin, Screen.safeArea.yMax));

        // Will help to recenter a little bit the scene
        upperLeftScreen += new Vector2(pathPrefab.GetComponent<SpriteRenderer>().bounds.size.x, 0);

        CreateRowWalls();
        GenerateMap();
        CreateRowWalls();

        GameEventManager.MapLoaded();
    }

    /// <summary>
    /// Generates the map instantiating sprites
    /// </summary>
    private void GenerateMap()
    {
        LOGGER.Log("Generating map...");

        for (int i = 0; i < map.GetLength(0); i++)
        {
            // Left limit
            var leftLimitCell = Instantiate(wallPrefab);
            ConfigureMapCell(ref leftLimitCell, LayerMask.NameToLayer(wallStr));

            for (int j = 0; j < map.GetLength(1); j++)
            {
                GameObject cell = null;
                
                // we allow to add more types of cells
                switch (map[i, j])
                {
                    case 0:
                        cell = Instantiate(wallPrefab);
                        ConfigureMapCell(ref cell, LayerMask.NameToLayer(wallStr));
                        break;

                    default:
                        cell = Instantiate(pathPrefab);
                        ConfigureMapCell(ref cell, LayerMask.NameToLayer(pathStr));

                        // if cell is the left bottom one
                        if (i == map.GetLength(0) - 1 && j == 0)
                        {
                            initCell = cell.transform;
                        }
                        else
                        {
                            emptyCells.Add(
                                new Cell() 
                                { 
                                    Index = new KeyValuePair<int, int>(i, j),
                                    Transform = cell.transform 
                                });
                        }

                        break;
                }
            }

            // Right limit
            var righttLimitCell = Instantiate(wallPrefab);
            ConfigureMapCell(ref righttLimitCell, LayerMask.NameToLayer(wallStr));

            currentHOffset = 0;
            currentVOffset -= vOffset;
        }

        LOGGER.Log("Map finished");
    }

    /// <summary>
    /// Sets the location of the cell
    /// </summary>
    /// <param name="cell"></param>
    private void ConfigureMapCell(ref GameObject cell, LayerMask layer)
    {
        cell.layer = layer;

        if (firstCell == null)
        {
            cell.transform.SetParent(content);
            ConfigureFirstCell(cell);
        }
        else
        {
            cell.transform.SetParent(firstCell.transform);
        }

        hOffset = cell.GetComponent<SpriteRenderer>().bounds.size.x;
        vOffset = cell.GetComponent<SpriteRenderer>().bounds.size.y;

        var vectorOffset = new Vector2(currentHOffset, currentVOffset);
        cell.transform.position = upperLeftScreen + vectorOffset;

        currentHOffset += hOffset;
    }

    /// <summary>
    /// Configures the very first cell as the parent of the rest of the cells
    /// </summary>
    /// <param name="cell"></param>
    private void ConfigureFirstCell(GameObject cell)
    {
        firstCell = cell;
        firstCell.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        firstCell.AddComponent<CompositeCollider2D>();
    }

    /// <summary>
    /// Creates a row of wall cells
    /// </summary>
    private void CreateRowWalls()
    {
        LOGGER.Log("Generating walls row...");

        // Left limit
        var leftLimitCell = Instantiate(wallPrefab);
        ConfigureMapCell(ref leftLimitCell, LayerMask.NameToLayer(wallStr));

        for (int i = 0; i < map.GetLength(1); i++)
        {
            var cell = Instantiate(wallPrefab);

            ConfigureMapCell(ref cell, LayerMask.NameToLayer(wallStr));
        }

        // Right limit
        var righttLimitCell = Instantiate(wallPrefab);
        ConfigureMapCell(ref righttLimitCell, LayerMask.NameToLayer(wallStr));

        currentHOffset = 0;
        currentVOffset -= vOffset;

        LOGGER.Log("Walls row finished");
    }
}
