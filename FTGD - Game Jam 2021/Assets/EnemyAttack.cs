using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int dmg;
    public float attSpeed;
    public float knockBackForce = 50f;
    [SerializeField] private float timeBtwAtt;
    public Collider hitbox;

    public float hitBoxActiveWait = 0.2f;
    public float attackRecovery = 0.3f;
    public float hitDelay = .5f; //this is used for timing hit animation;
    public EnemyAI enemyAIRef;

    private void Start()
    {
        enemyAIRef = GetComponentInParent<EnemyAI>();
    }
    private void Update()
    {
        timeBtwAtt -= Time.deltaTime;   
    }
    public void Attack()
    {
        if (timeBtwAtt <= 0)
        {
            timeBtwAtt = attSpeed;
            if(enemyAIRef.swingSound != null)
                enemyAIRef.audioSource.PlayOneShot(enemyAIRef.swingSound);
            
            enemyAIRef.animator.SetTrigger("attack");

            Vector3 direction = enemyAIRef.playerReference.transform.position - transform.position;

            if (direction.x > 0 && !enemyAIRef.facingRight)
            {
                enemyAIRef.Flip();
            }
            else if (direction.x < 0 && enemyAIRef.facingRight)
            {
                enemyAIRef.Flip();
            }

            StartCoroutine(AttackCoroutine());


        }
        else
        {
        
            //enemyAIRef.currentState = EnemyAI.EnemyState.CHASING;
        }
    }

    IEnumerator AttackCoroutine()
    {
        float originalMoveX = enemyAIRef.moveSpeedX;
        float originalMoveZ = enemyAIRef.moveSpeedZ;


        enemyAIRef.moveSpeedZ = 0;
        enemyAIRef.moveSpeedX = 0;


        enemyAIRef.enableChase = false;
        //Insert attack logic here
        yield return new WaitForSeconds(hitDelay);
        hitbox.enabled = true;


        yield return new WaitForSeconds(hitBoxActiveWait);
        hitbox.enabled = false;


        yield return new WaitForSeconds(attackRecovery);
        enemyAIRef.moveSpeedZ = originalMoveZ;
        enemyAIRef.moveSpeedX = originalMoveX;

        enemyAIRef.currentState = EnemyAI.EnemyState.ATTACKING;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player tempPlayer = other.GetComponentInParent<Player>();

            Debug.Log("Attacked and hit: " + other.name);
            tempPlayer.TakeDamage(dmg);
            
            if(enemyAIRef.hitSound != null)
                enemyAIRef.audioSource.PlayOneShot(enemyAIRef.hitSound);
            //tempPlayer.Knockback(enemyAIRef.transform.position, knockBackForce);
        }
    }
}
