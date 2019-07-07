﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 0.5f;

    private Animator animator;
    private int food;
    public Text foodText;

    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip gameOverSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.tag);

        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        } else if (other.tag == "Food")
        {
            SoundManager.instance.RandomizeSfx(eatSound1,eatSound2);
            foodText.text = "Food: " + food + "(+ " + pointsPerFood + ")"; 
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        } else if (other.tag == "Soda")
        {
            SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

            foodText.text = "Food: " + food + "(+ " + pointsPerSoda + ")";
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }

    }

    private void Restart()
    {
        // NB. Only one scene, because procedural generation
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "Food: " + food + "(- " + loss + ")";
        CheckIfGameOver();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        foodText.text = "Food: " + food;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.playersTurn) return;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            // Player's turn begins
            food--;
            foodText.text = "Food: " + food;
            CheckIfGameOver();

            Vector2 end = GetMoveCoordinates(horizontal, vertical);
            Transform hitTransform = CanMove(end);

            if (hitTransform == null)
            {
                MoveSmooth(end);
                SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            } else
            {
                Wall hitWall = hitTransform.GetComponent<Wall>();
                Enemy hitEnemy = hitTransform.GetComponent<Enemy>();

                if (hitWall != null)
                {
                    Debug.Log("Player hit a wall");
                    hitWall.DamageWall(wallDamage);
                    animator.SetTrigger("playerChop");
                }
                else if (hitEnemy != null)
                {
                    Debug.Log("Player hit an enemy");
                    // Crush the enemy immediately
                    //GameManager.instance.KillEnemy(hitEnemy);
                } else
                {
                    Debug.Log("Player hit something.");
                }
            }

            GameManager.instance.playersTurn = false;
        }
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}