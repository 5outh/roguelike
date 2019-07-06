using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.5f;
    public static GameManager instance = null;

    public BoardManager boardScript;

    private int level = 1;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    public float levelStartDelay;
    private Text levelText;
    private GameObject levelImage;
    private bool doingSetup;
    public GameObject playerPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        print("setting up level image");
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        print("set up level image");
        Invoke("HideLevelImage", levelStartDelay);

        //Instantiate(playerPrefab, new Vector3(0, 0), Quaternion.identity);
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        print("unset level image");
        levelImage.SetActive(false);
        doingSetup = false;
    }

    void OnLevelWasLoaded(int _level)
    {
        // TODO: Refactor
        level++;
        InitGame();
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach(Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
}
