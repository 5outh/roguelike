using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float turnDelay = 0.2f;
    public static GameManager instance = null;

    public BoardManager boardScript;

    private int level = 1;
    public int playerFoodPoints = 100;
    private List<Enemy> enemies;
    public float levelStartDelay;
    private Text levelText;
    private GameObject levelImage;
    public GameObject playerPrefab;
    [HideInInspector] public Phase phase;

    public Text turnText;
    public int turn = 1;

    public enum Phase
    {
        PLAYER,
        ENEMIES,
        SETUP,
        ENEMY_INTENT
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
        phase = Phase.SETUP;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Level " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        turnText = GameObject.Find("TurnText").GetComponent<Text>();
        turnText.text = "Turn: " + turn;

        enemies.Clear();
        boardScript.SetupScene(level);
        ChangePhase();
    }


    public void KillEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        enemy.gameObject.SetActive(false);
		enemy.DestroyAttacks();
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        phase = Phase.PLAYER;
    }

    void OnLevelWasLoaded(int _level)
    {
        // TODO: Refactor (this is deprecated)
        level++;
        InitGame();
    }

    IEnumerator MoveEnemies()
    {
        // Wait for the Player to move before moving enemies.
        yield return new WaitForSeconds(turnDelay * 2);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        foreach(Enemy enemy in enemies)
        {
            enemy.MoveEnemy();

            yield return new WaitForSeconds(enemy.moveTime);
        }

        ChangePhase();
    }

    public void GameOver()
    {
        levelText.text = "You died! Score: " + level;
        levelImage.SetActive(true);
        enabled = false;
    }

    public Phase NextPhase()
    {
        switch (phase)
        {
            case Phase.SETUP:
                return Phase.ENEMY_INTENT;
            case Phase.ENEMY_INTENT:
                return Phase.PLAYER;
            case Phase.PLAYER:
                return Phase.ENEMIES;
            case Phase.ENEMIES:
                return Phase.ENEMY_INTENT;
            default:
                throw new System.Exception("Did not recognize phase");

        }
    }

    private IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void ChangePhase()
    {
        phase = NextPhase();

        StartCoroutine(Wait(turnDelay));

        switch (phase)
        {
            case Phase.SETUP:
                break;
            case Phase.PLAYER:
                break;
            case Phase.ENEMIES:
                ClearObjectsWithTag("EnemyTarget");
                ClearObjectsWithTag("Arrows");
                turn++;
                turnText = GameObject.Find("TurnText").GetComponent<Text>();
                turnText.text = "Turn: " + turn;
                StartCoroutine(MoveEnemies());
                break;
            case Phase.ENEMY_INTENT:
				GameObject[] enemyAttacks = GameObject.FindGameObjectsWithTag("EnemyAttack");
                foreach (GameObject enemyAttack in enemyAttacks)
				{
					Destroy(enemyAttack);
				}

                foreach (Enemy enemy in enemies)
                {
                    print(enemy);
                    enemy.Intend();
                }
                ChangePhase();
                break;
            default:
                break;
        }
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    private void ClearObjectsWithTag(string theTag)
    {
        GameObject[] enemyTargets = GameObject.FindGameObjectsWithTag(theTag);

        foreach (GameObject enemyTarget in enemyTargets)
        {
            Destroy(enemyTarget);
        }
    }
}
