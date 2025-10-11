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
               
                    enemyRigidbody.velocity = Vector2.zero;
                    enemyRigidbody.angularVelocity = 0f;
                if (player.currentState != PlayerStates.stagger && hit.IsTouching(enemy.attackHitboxes))
                {
                    Vector2 difference = player.transform.position - transform.position;
                    Vector2 force = difference.normalized * thrust;
                    Debug.Log("Player knocked back");
                    player.currentState = PlayerStates.stagger;
                    player.Knock(force, .25f,damage);
                }
            }
        }
    }
}
