using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float hSpeed = 10f;
    public float vSpeed = 6f;

    public float originalSpeedH;
    public float originalSpeedV;
    
    private Rigidbody rb;
    [SerializeField] public bool canMove = true;

    [SerializeField] private bool facingRight = true;
    [Range(0, 1.0f)] [SerializeField] float movementSmothing;

    private SpriteRenderer sr;

    Player player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = GetComponent<Player>();

        originalSpeedH = hSpeed;
        originalSpeedV = vSpeed;
    }

    public void Move(float hMove, float vMove, bool jump)
    {

   
        if (canMove)
        {
            Vector3 targetMove = new Vector3(hMove * hSpeed,0, vMove * vSpeed);

            rb.velocity = targetMove;

            //transform.Translate(targetMove * Time.deltaTime);

            if (hMove != 0 || vMove != 0)
                player.animator.SetBool("isWalking", true);
            else
            {
                player.animator.SetBool("isWalking", false);
            }


            //Flip character
            if (hMove > 0 && !facingRight)
            {
                Flip();
            }
            else if(hMove < 0 && facingRight)
            {
                Flip();
            }
        }


    }
   

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 tempScale = transform.localScale;
        tempScale = new Vector3(-tempScale.x, tempScale.y, tempScale.z);
        transform.localScale = tempScale;

        //bool currentSpriteFlip = sr.flipX;
        //sr.flipX = !currentSpriteFlip;

        //transform.Rotate(0, 180, 0);
    }

}
