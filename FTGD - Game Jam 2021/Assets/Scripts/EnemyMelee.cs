using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyAI
{
    // Use this for initialization


    /*Now you will need to override the methods you want to use for the enemy, these can be
     * found in the bottom of the EnemyBehaviour-class*/

    public override void Idle()
    {
        /*Add your idle-behaviour here*/

        /*could be:*/
        /*Update Patrolling the area*/
        /* if in range of the player*/
        /*Raycast toward player*/
        /*if raycast hit - change currentState to sawPlayer*/

        //currentState = EnemyState.sawPlayer;
        animator.SetBool("isWalking", false);

        if (Vector3.Distance(transform.position, playerReference.transform.position) < aggroDistance)
        {
            currentState = EnemyState.SAWPLAYER;
        }
    }
    public override void SawPlayer()
    {
        /*Add your SawPlayer-behaviour here*/

        /*could be:*/
        /* face the player */
        /* play surprise animation */
        /* when animation is done - change currentState to chasing*/

        //currentState = EnemyState.chasing;
        if (!hasSpottedPlayer)
        {
            /*visualisation of enemy animation*/

            animator.SetTrigger("Surprised");
            //transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.x * 2, transform.localScale.x * 2);
            hasSpottedPlayer = true;
        }

        if (reactionTime < 0)
        {
            reactionTime = .5f;
            currentState = EnemyState.CHASING;
        }
        {
            reactionTime -= Time.deltaTime;
        }

    }
    public override void Chasing()
    {
        /*Add your Chasing-behaviour here*/

        /*could be:*/
        /* if hp < 30 - change currentState to Flee*/
        //currentState = EnemyState.fleeing;

        /* if not close enough to attack the player - move towards the player. It is here you choose
         * if the enemy is either melee or ranged */

        /*else change currentState to attack*/
        
        //currentState = EnemyState.attacking; 
        
        //transform.LookAt(playerReference.transform);
        float distanceDelta = Vector3.Distance(transform.position, playerReference.transform.position);
        if (distanceDelta > attackRange)
        {
            animator.SetBool("isWalking", true);

            Vector3 moveTowards = playerReference.transform.position - transform.position;
            moveTowards.Normalize();
            
            //transform.Translate(new Vector3(moveTowards.x * moveSpeedX, 0, moveTowards.z * moveSpeedZ)  * Time.deltaTime);
            rb.velocity = new Vector3(moveTowards.x * moveSpeedX, 0, moveTowards.z * moveSpeedZ);
            //= new Vector3(transform.position.x + moveTowards.x / dampening,
            //                                                transform.position.y,
            //                                                transform.position.z + moveTowards.z / dampening);

            //Flip character
            if (moveTowards.x > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveTowards.x < 0 && facingRight)
            {
                Flip();
            }
        }
        else
        {
            currentState = EnemyState.ATTACKING;
        }
    }


    public override void Attacking()
    {


        enemyAttack.Attack();

        if (health < 2)
        {
            currentState = EnemyState.FLEEING;
            
        }
        else
        {
            currentState = EnemyState.CHASING;
        }
    }
    public override void Fleeing()
    {
        /*Add your Fleeing-behaviour here*/

        /*could be:*/
        /* if EnemyHp < 30*/
        /*raycast to player and run the other way, simplest to implement*/
        /*else*/
        /*change state to chase*/

        float distanceDelta = Vector3.Distance(playerReference.transform.position, transform.position);

        Vector3 awayFromPlayer = transform.position - playerReference.transform.position;
        awayFromPlayer.Normalize();

        if (health < 2)
        {
            //float dampening = 30;
            //transform.position = new Vector3(transform.position.x + awayFromPlayer.x / dampening,
            //                                                transform.position.y,
            //                                                transform.position.z + awayFromPlayer.z / dampening);

            if(distanceDelta < aggroDistance + 4)
            {
                transform.Translate(new Vector3(awayFromPlayer.x * moveSpeedX/2, 0, awayFromPlayer.z * moveSpeedZ/2) * Time.deltaTime);

                animator.SetBool("isWalking", true);

                if (awayFromPlayer.x > 0 && !facingRight)
                {
                    Flip();
                }
                else if (awayFromPlayer.y < 0 && facingRight)
                {
                    Flip();
                }
            }
            
  
            healthRegenTimer -= Time.deltaTime;

            if (healthRegenTimer <= 0)
            {
                Heal(1);
                healthRegenTimer = 6;

                //play particle here
            }
        }
        else
        {
            healthRegenTimer = 6;
            currentState = EnemyState.CHASING;
        }
    }
}
