using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class that manages the complete status of the game
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public bool GameRunning { get => gameRunning; }
    public byte Coins { get => score; }

    [Header("Character")]
    [SerializeField]
    private GameObject characterPrefab;

    [Header("Coins")]
    [SerializeField]
    private GameObject coinPrefab;

    [Header("CountDown")]
    [SerializeField]
    [Range(1, 3540)] // from 1s to 59 mins
    private ushort countdownSeconds = 60;
    private float timeRemaining;
    private float elapsedTime;

    [Header("Chest")]
    [SerializeField]
    private GameObject chestPrefab;

    [SerializeField]
    [Range(0, 100)]
    private float chestProbability = 25.0f;

    [SerializeField]
    private float chestLifeTime = 8.0f;

    private byte score = 0;
    private byte coinsTaken;
    private byte totalCoins;

    private List<KeyValuePair<int, int>> emptyPlaces;

    private GameObject character;

    private bool gameRunning = false;

    Logger LOGGER;


    private void Awake()
    {
        AddListeners();

        emptyPlaces = new List<KeyValuePair<int, int>>();

        LOGGER = new Logger(gameObject);
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }

    private void Update()
    {
        if(gameRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime;
                elapsedTime += Time.deltaTime;
                GameEventManager.TimeChanged((ushort)timeRemaining); 

                if(elapsedTime >= 1.0f)
                {
                    elapsedTime %= 1.0f;

                    if (coinsTaken < totalCoins)
                    {
                        GenerateObject(chestPrefab); 
                    }
                    else
                    {
                        GenerateRandomObject();
                    }
                }
            }
            else
            {
                GameEventManager.GameOver();
                if(!PlayerPrefs.HasKey("BestScore"))
                {
                    PlayerPrefs.SetString("BestScore", score.ToString());
                }
                else if (score > int.Parse(PlayerPrefs.GetString("BestScore")))
                {
                    PlayerPrefs.SetString("BestScore", score.ToString()); 
                }

                gameRunning = false;
            }
        }
    }

    /// <summary>
    /// Adds listeners to events
    /// </summary>
    private void AddListeners()
    {
        GameEventManager.OnMapLoaded += StartGame;
        GameEventManager.OnRetry += ResetGame;
    }

    /// <summary>
    /// removes listeners to events
    /// </summary>
    private void RemoveListeners()
    {
        GameEventManager.OnMapLoaded -= StartGame;
        GameEventManager.OnRetry -= ResetGame;
    }

    /// <summary>
    /// Begins the game
    /// </summary>
    private void StartGame()
    {
        InitCharacter();
        GenerateCoins();
        InitTimeCounter();
        InitCoinCounter();

        gameRunning = true;
    }

    /// <summary>
    /// Generates a random object
    /// </summary>
    private void GenerateRandomObject()
    {
        switch(Random.Range(0,1))
        {
            case 0:
                GenerateObject(coinPrefab);
                break;

            case 1:
                GenerateObject(chestPrefab);
                break;
        }
    }

    /// <summary>
    /// Generates coins in the scene
    /// </summary>
    private void GenerateCoins()
    {
        foreach (var cell in MapManager.Instance.EmptyCells)
        {
            Instantiate(coinPrefab, cell.Transform.position, Quaternion.identity).GetComponent<Coin>().Index = cell.Index;
            totalCoins++;
        }
    }

    /// <summary>
    /// Instantiates a new chest in some empty place
    /// </summary>
    private void GenerateObject(GameObject prefab)
    {

        if (emptyPlaces.Count > 0)
        {
            var comp = prefab.GetComponent<SpawnableObject>();

            var indexEmptyPlace = Random.Range(0, emptyPlaces.Count - 1);
            Cell emptyCell = MapManager.Instance.EmptyCells.Where(x => x.Index.Equals(emptyPlaces[indexEmptyPlace])).First();

            var newPos = emptyCell.Transform.position;

            switch (comp.GetType())
            {
                case
                    var coin when coin == typeof(Coin):
                    {
                        // Spawning coin
                        var obj = Instantiate(prefab, newPos, Quaternion.identity).GetComponent<Coin>();
                        obj.Index = emptyCell.Index;

                        emptyPlaces.Remove(obj.Index);
                    }
                    break;

                case
                    var chest when chest == typeof(Chest):
                    {
                        // this random is basic, but this is a prototype :)
                        if (Random.Range(0f, 100f) <= chestProbability)
                        {
                            //Spawning chest
                            var obj = Instantiate(prefab, newPos, Quaternion.identity).GetComponent<Chest>();
                            obj.Index = emptyCell.Index;

                            obj.Lifetime = chestLifeTime;
                            obj.Ready();

                            emptyPlaces.Remove(obj.Index);
                        }
                    }
                    break;
            } 
        }
    }

    /// <summary>
    /// Sets up character
    /// </summary>
    private void InitCharacter()
    {
        character = Instantiate(characterPrefab, MapManager.Instance.InitPosition, Quaternion.identity);
    }

    /// <summary>
    /// Inits the coins counter
    /// </summary>
    private void InitCoinCounter()
    {
        GameEventManager.CoinsTaken(score);
    }

    /// <summary>
    /// Inits the clock
    /// </summary>
    private void InitTimeCounter()
    {
        timeRemaining = countdownSeconds;
        GameEventManager.TimeChanged((ushort)timeRemaining);
    }

    /// <summary>
    /// Resets the entire game
    /// </summary>
    private void ResetGame()
    {
        score = 0;
        coinsTaken = 0;
        totalCoins = 0;
        Destroy(character);

        StartGame();
    }

    /// <summary>
    /// Updates the coins counter
    /// </summary>
    public void CoinTaken(SpawnableObject coin)
    {
        score++;
        coinsTaken++;
        GameEventManager.CoinsTaken(score);
        emptyPlaces.Add(coin.Index);
        coin.Taken();

        LOGGER.Log("Coin taken!");
    }

    /// <summary>
    /// Updates the coins counter in a 10%
    /// </summary>
    public void ChestTaken(SpawnableObject chest)
    {
        score += (byte)(score * 1.1f);
        GameEventManager.CoinsTaken(score);
        emptyPlaces.Add(chest.Index);
        chest.Taken();

        LOGGER.Log("Chest taken!");
    }
}
