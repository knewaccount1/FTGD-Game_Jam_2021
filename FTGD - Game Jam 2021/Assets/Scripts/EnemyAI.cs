using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : Units
{
    public int health = 3;
    public int maxHealth = 3;
    public float healthRegenTimer = 5f;
    public string enemyName;
    public float aggroDistance = 5 ;
    public float reactionTime = .5f;
    public bool hasSpottedPlayer = false;

    public float attackRange;

    public float moveSpeedX = 4;
    public float moveSpeedZ = 3;
    public bool facingRight = false;

    public bool isDying;
    public Rigidbody rb;

    public bool enableChase;

    private Material matWhite;
    private Material[] matDefault;
    public SpriteRenderer[] sr;
    public EnemyAttack enemyAttack;

    public Animator animator;

    public ParticleSystem deathParticle;

    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip swingSound;

    /*Here you can add and remove states for the enemy, see the manual.txt for guidance!*/
    public enum EnemyState
    {
        INITIALIZING,
        IDLE,
        SAWPLAYER,
        CHASING,
        ATTACKING,
        FLEEING,
        DYING
    }
    /*This is the currentState of the Enemy, this is what you'll change in the child-Class*/
    public EnemyState currentState;

    public Player playerReference;

    void Start()
    {

        matDefault = new Material[2];

        matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;

        for (int i = 0; i < sr.Length; i++)
        {
            SpriteRenderer tempSR = sr[i];
            matDefault[i] = sr[i].material;
        }

        currentState = EnemyState.INITIALIZING;

        enemyAttack = GetComponentInChildren<EnemyAttack>();
        rb = GetComponent<Rigidbody>();

        health = maxHealth;
        animator = GetComponent<Animator>();
    }

    /*In here there is a switch-statement which handles which method that is going
    * to be updating, this is chosen by the currentState of the enemy.
     It is in here that you will add your own EnemyState.yourState-case and call for your own method below*/
    public virtual void Update()
    {

        switch (currentState)
        {
            case EnemyState.INITIALIZING:
                /*filling in the player reference for easier access*/
                playerReference = FindObjectOfType<Player>();
                currentState = EnemyState.IDLE;
                break;
            case EnemyState.IDLE:
                Idle();
                break;
            case EnemyState.SAWPLAYER:
                SawPlayer();
                break;
            case EnemyState.CHASING:
                enableChase = true;
                break;
            case EnemyState.ATTACKING:
                Attacking();
                break;
            case EnemyState.FLEEING:
                Fleeing();
                break;
            default:
                break;
        }


    }

    public virtual void FixedUpdate()
    {
        if(enableChase == true)
        {
            Chasing();
        }   
    }

    /*When you add your own methods here they need to be virtual, this is so you can in override them in your own
     class*/

    public virtual void Idle()
    {
    }
    public virtual void SawPlayer()
    {
    }
    public virtual void Chasing()
    {
    }
    public virtual void Attacking()
    {
    }
    public virtual void Fleeing()
    {
    }

    public virtual void Heal(int healAmt)
    {
        health += healAmt;
        
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }
    public virtual void TakeDamage(int dmg)
    {
        health -= dmg;
        if (!isDying)
        {
            //HitEffects();
            if (health <= 0d)
            {
                isDying = true;
                StartCoroutine(Die());
            }
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;

        Vector3 tempScale = transform.localScale;
        tempScale = new Vector3(-tempScale.x, tempScale.y, tempScale.z);
        transform.localScale = tempScale;
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


    public void Knockback(Vector3 attackerPos, float kbForce, float verticalForce = 0)
    {
        Vector3 forceVector = transform.position - attackerPos;
        
        rb.AddForce(new Vector3(forceVector.x, verticalForce, forceVector.y).normalized* kbForce, ForceMode.Impulse);

    }

    IEnumerator Die()
    {

        currentState = EnemyState.DYING;
        FindObjectOfType<GameManager>().EnemyDied();
        this.enabled = false;
        enemyAttack.enabled = false;

        Knockback(FindObjectOfType<Player>().transform.position, 10,1.5f);

        //Play animation
        animator.Play("dying");

        yield return new WaitForSeconds(1.5f);
        Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
        Destroy(gameObject);
    }

}
