using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Sprite damageSprite;
    public int hp = 4;
    private SpriteRenderer spriteRenderer;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    public void DamageWall(int loss)
    {
        spriteRenderer.sprite = damageSprite;
        hp -= loss;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
        SoundManager.instance.RandomizeSfx(chopSound1, chopSound2);
    }
}