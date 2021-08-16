using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Units
{
    public CharacterAttack characterAttack;
    public CharacterMovement characterMovement;
    public Animator animator;
    public int health = 5;
    public int maxHealth;
    public bool isDying = false;
    Rigidbody rb;
    private Material matWhite;
    private Material[] matDefault;
    public SpriteRenderer[] sr;


    private float timeBtwHit;

    public GameManager GM;

    public AudioClip swingSound;
    public AudioClip hitSound;
    public AudioSource audioSource;

    private void Start()
    {
        matDefault = new Material[2];
        rb = GetComponent<Rigidbody>();
        health = maxHealth;

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;

        for (int i = 0; i < sr.Length; i++)
        {
            SpriteRenderer tempSR = sr[i];
            matDefault[i] = sr[i].material;
        }
    }

    private void Update()
    {
        timeBtwHit -= Time.deltaTime;
    }

    public void TakeDamage(int dmg)
    {
        if (timeBtwHit < 0)
        {
            health -= dmg;

            HitEffects();
            if (health <= 0)
            {
                isDying = true;
            }
            timeBtwHit = 1.5f;
        }

        if(health <= 0)
        {
            Die();
        }
        GM.UpdateHearts();


    }

    public void Die()
    {
        //Place dying algorithm here
    }

    public void Heal(int amt)
    {
        health += amt;

        if(health > maxHealth)
        {
            health = maxHealth; 
        }

        GM.UpdateHearts();
    }

    public void HitEffects()
    {
        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].material = matWhite;
        }
       

        Invoke("ResetHitFlash", 0.2f);
    }

    public void ResetHitFlash()
    {
        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].material = matDefault[i];
        }
    }

    public void Knockback(Vector3 attackerPos, float kbForce)
    {
        Vector3 forceVector = transform.position - attackerPos;
        rb.AddForce(new Vector3(forceVector.x, 0, forceVector.y).normalized * kbForce, ForceMode.Impulse);

    }


}
