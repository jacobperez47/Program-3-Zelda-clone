using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public enum PlayerStates
{
    walk,
    attack,
    interact,
    stagger,
    idle,
}

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public float attackCooldown = 0.5f;
    public PlayerStates currentState;
    public Signal playerHealthSignal;
    public FloatValue currentHealth;


    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    private bool attackPressed;
    private float nextAttackTime = 0f;
    private Coroutine knockbackRoutine;



    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth.initialValue = 6.0f;

    }
    void Start()
    {
        currentState = PlayerStates.walk;
      
        animator.SetFloat("moveX", 0f);
        animator.SetFloat("moveY", -1f);
    }

    // Update is called once per frame

    void Update()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space)) && currentState != PlayerStates.attack &&
            Time.time >= nextAttackTime)
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

        else if (currentState == PlayerStates.walk || currentState == PlayerStates.idle)
        {
            UpdateCharacterAnimation();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerStates.attack;
        yield return new WaitForSeconds(0.05f);
        animator.SetBool("attacking", false);
        // float remainingCooldown = attackCooldown - 0.1f;
        // if (remainingCooldown > 0f)
        // {
        //     yield return new WaitForSeconds(remainingCooldown);
        // }
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
        change.Normalize();
        myRigidbody.MovePosition(transform.position + change.normalized * (speed * Time.fixedDeltaTime));
    }

    public void Knock(Vector2 finalKnockVelocity, float knockTime, float damage)
    {
        Debug.Log(currentHealth.initialValue);
        // Safety check for Rigidbody
        if (myRigidbody == null) return;

        // --- 1. HEALTH AND DAMAGE LOGIC ---

        // CRITICAL NULL CHECK: Ensure the ScriptableObject is assigned before using it.
        if (currentHealth == null)
        {
            Debug.LogError("FATAL ERROR: currentHealth (FloatValue) is NULL! Check PlayerMovement Inspector.");
            return;
        }


        // Reduce health
        currentHealth.runtimeValue -= damage;


        if (currentHealth.runtimeValue > 0)
        {
            // Stop any running KnockbackCoroutine to prevent state conflicts
            if (knockbackRoutine != null)
            {
                StopCoroutine(knockbackRoutine);
            }
            
            playerHealthSignal.Raise();
            currentState = PlayerStates.stagger;
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(finalKnockVelocity, ForceMode2D.Impulse);

            knockbackRoutine = StartCoroutine(KnockbackCoroutine(myRigidbody, knockTime));
        }
        else
        {
            // Handle death if health <= 0
            Debug.Log("Player has died!");
            // You would typically call a death function here (e.g., DieCo()).
            // For now, let's just make sure the player stops moving and state is set to idle.
            currentState = PlayerStates.idle;
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockbackCoroutine(Rigidbody2D myRigidBody, float knockTime)
    {
        yield return new WaitForSeconds(knockTime);

        // Reset velocity and state
        if (myRigidBody != null)
        {
            myRigidBody.velocity = Vector2.zero;
        }

        currentState = PlayerStates.idle;
    }
}