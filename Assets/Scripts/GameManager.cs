using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.1f;
    public static GameManager instance = null;

    public BoardManager boardScript;

    private int level = 3;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;
    private List<Enemy> enemies;
    private bool enemiesMoving;

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
        Instantiate(playerPrefab, new Vector3(0, 0), Quaternion.identity);
        enemies.Clear();
        boardScript.SetupScene(level);
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
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach(Enemy enemy in enemies)
        {
            enemy.MoveEnemy();
        }
        yield return new WaitForSeconds(turnDelay);

        playersTurn = true;
        enemiesMoving = false;
    }

    public void GameOver()
    {
        print("game over");
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
}
