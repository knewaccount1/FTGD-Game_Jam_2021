using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField]private CharacterMovement charMovement;
    float horizontalMove;
    float verticalMove;

    private void Start()
    {
        charMovement = FindObjectOfType<CharacterMovement>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");

      

        if (Input.GetButtonDown("Fire1"))
        {
          
            charMovement.GetComponent<Player>().characterAttack.Attack();
        }
    }

    private void FixedUpdate()
    {
        charMovement.Move(horizontalMove, verticalMove, false);
    }
}
