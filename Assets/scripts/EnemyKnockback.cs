using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    public float thrust;

    private EnemyScript enemy;
    private Rigidbody2D enemyRigidbody;
    public float damage;
    
    // Start is called before the first frame update
    void Start()
    {

        enemy = GetComponentInParent<EnemyScript>();
        if (enemy != null)
        {
            enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            

            if (hit != null && enemy != null && player != null)
            {
                // CRITICAL NULL CHECK: The previous version crashed here if the enemy was missing a Rigidbody2D.
                if (enemyRigidbody == null)
                {
                    Debug.LogError("Cannot stop enemy movement: enemyRigidbody is NULL. Check Enemy GameObject for Rigidbody2D.");
                    return; // Exit to prevent crash
                }
               
                // Stop enemy movement
                enemyRigidbody.velocity = Vector2.zero;
                enemyRigidbody.angularVelocity = 0f;

                // Check player state and hitbox collision
                if (player.currentState != PlayerStates.stagger && hit.IsTouching(enemy.attackHitboxes))
                {
                    Vector2 difference = player.transform.position - transform.position;
                    Vector2 force = difference.normalized * thrust;
                    
                    Debug.Log("Player hit detected, calling Knock.");
                    
                    // We let the player.Knock() function handle the state change and coroutine management
                    player.Knock(force, .25f, damage);
                }
            }
        }
    }
}
