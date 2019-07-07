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
    private List<Enemy> enemies;
    public float levelStartDelay;
    private Text levelText;
    private GameObject levelImage;
    public GameObject playerPrefab;
    [HideInInspector] public Phase phase = Phase.SETUP;

    public enum Phase
    {
        PLAYER,
        ENEMIES,
        SETUP
    };

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
        ChangePhase(Phase.SETUP);
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }


    public void KillEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemy.gameObject.SetActive(false);
    }

    private void HideLevelImage()
    {
        print("unset level image");
        levelImage.SetActive(false);
        phase = Phase.PLAYER;
    }

    void OnLevelWasLoaded(int _level)
    {
        // TODO: Refactor
        level++;
        InitGame();
    }

    IEnumerator MoveEnemies()
    {
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

        ChangePhase(Phase.PLAYER);
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    public void ChangePhase(Phase p)
    {
        phase = p;

        switch (p)
        {
            case Phase.SETUP:
                break;
            case Phase.PLAYER:
                break;
            case Phase.ENEMIES:
                StartCoroutine(MoveEnemies());
                break;
            default:
                break;
        }
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }
}
