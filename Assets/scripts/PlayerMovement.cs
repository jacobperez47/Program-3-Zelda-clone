using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public enum PlayerStates
{
    walk,
    attack,
    interact
}

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float attackCooldown = 0.5f;
    public PlayerStates currentState;
    
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private bool attackPressed;
    private float nextAttackTime = 0f;

    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
     
    }

    // Update is called once per frame

    void Update()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) && currentState != PlayerStates.attack && 
            Time.time >= attackCooldown)
        {
            attackPressed = true;
        }
    }
    void FixedUpdate()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
       
        if (attackPressed && currentState != PlayerStates.attack)
        {
            attackPressed = false;
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(AttackCo());
        }

       else if (currentState == PlayerStates.walk)
        {
            UpdateCharacterAnimation();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerStates.attack;
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("attacking", false);
        float remainingCooldown = attackCooldown - 0.1f;
        if (remainingCooldown > 0f)
        {
            yield return new WaitForSeconds(remainingCooldown);
        }
        currentState = PlayerStates.walk;
        
    }
    private void UpdateCharacterAnimation()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
            animator.SetBool("moving", false);
    }

    void MoveCharacter()
    {
        myRigidbody.MovePosition(transform.position + change.normalized * (speed * Time.fixedDeltaTime));
    }
}