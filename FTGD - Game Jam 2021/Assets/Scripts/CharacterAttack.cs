using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public int dmg;
    public float attSpeed;
    public float knockBackForce = 50f;
    [SerializeField]private float timeBtwAtt;
    public Collider hitbox;

    public float hitBoxActiveWait = 0.05f;
    public float attackRecovery = 0.3f;
    public float hitDelay = .5f; //this is used for timing hit animation;
    private Player player;

    public ParticleSystem hitParticle;



    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        timeBtwAtt -= Time.deltaTime;
    }
    public void Attack()
    {
        if (timeBtwAtt <= 0)
        {

            player.audioSource.PlayOneShot(player.swingSound);
            timeBtwAtt = attSpeed;
            player.characterMovement.hSpeed = player.characterMovement.originalSpeedH;
            player.characterMovement.vSpeed = player.characterMovement.originalSpeedV;
            StartCoroutine(AttackCoroutine());
     
            player.animator.SetTrigger("attack");

     
        }
    }

    IEnumerator AttackCoroutine()
    {



        player.characterMovement.hSpeed = 0;
        player.characterMovement.vSpeed = 0;

        //Insert attack logic here
        yield return new WaitForSeconds(hitDelay);
        hitbox.enabled = true;


        yield return new WaitForSeconds(hitBoxActiveWait);
        hitbox.enabled = false;


        yield return new WaitForSeconds(attackRecovery);
        player.characterMovement.hSpeed = player.characterMovement.originalSpeedH;
        player.characterMovement.vSpeed = player.characterMovement.originalSpeedV;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAI tempEnemy = other.GetComponentInParent<EnemyAI>();

            Instantiate(hitParticle.gameObject, new Vector3 (tempEnemy.transform.position.x, tempEnemy.transform.position.y, tempEnemy.transform.position.z -0.5f), hitParticle.transform.rotation);
            tempEnemy.TakeDamage(dmg);

            player.audioSource.PlayOneShot(player.hitSound);
            player.GM.ShakeCamera(0.2f, 1.5f, 0.1f);
            tempEnemy.Knockback(player.transform.position, knockBackForce);
        }
    }
}
