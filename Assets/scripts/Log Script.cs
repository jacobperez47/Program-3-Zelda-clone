using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScript : EnemyScript


{
    private Transform target;


    public float speed;
    
    public Animator animator;

    // public CircleCollider2D vision;
    //
    // public CircleCollider2D attack;

    private bool isVisible;
    private Rigidbody2D rb;
    private bool isAttacking;
    private bool isKnockedBack = false;

    // Start is called before the first frame update
    void Start()
    {
        currentState = EnemyStates.idle;
        rb = GetComponent<Rigidbody2D>();
        isVisible = false;
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        Debug.Log(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (isVisible && target && rb && !isAttacking && !isKnockedBack)
        {
            if ((currentState == EnemyStates.idle || currentState == EnemyStates.walk) &&
                currentState != EnemyStates.stagger)
            {
                MoveToTarget();
                changeState(EnemyStates.walk);
                animator.SetBool("wakeup", true);
            }
            
        }
        else if(!isVisible)
        {
            animator.SetBool("wakeup", false);
        }
    }

    private void MoveToTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        
        

        rb.MovePosition(transform.position + direction * (speed * Time.fixedDeltaTime));
        changeAnim(direction);
    }

    public void StartKnockBack(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StartKnockBackRoutine(duration));
    }

    private IEnumerator StartKnockBackRoutine(float knockbackDuration)
    {
        isKnockedBack = true;

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;


        // Vector3 direction = (target.position - transform.position).normalized;
        //
        // rb.MovePosition(transform.position + direction * (speed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (target == null)
            {
                target = other.transform;
            }

            if (other.IsTouching(vision))
            {
                isVisible = true;
            }

            if (attack != null && other.IsTouching(attack))
            {
                isAttacking = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (vision != null && !other.IsTouching(vision))
            {
                isVisible = false;
            }

            if (attack != null && !other.IsTouching(attack))
            {
                isAttacking = false;
            }

        }
    }

    private void changeAnim(Vector2 direction)
    {
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", 0);
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetFloat("moveX", 1 );
            }
            else if (direction.x < 0)
            {
                animator.SetFloat("moveX", -1 );
            }
        }
        else if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if (direction.y > 0)
            {
                animator.SetFloat("moveY", 1 );
            }
            else if (direction.y < 0)
            {
                animator.SetFloat("moveY", -1 );
            }
        }
    }

    private void changeState(EnemyStates newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    
    
}