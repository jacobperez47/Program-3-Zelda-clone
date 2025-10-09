using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockback : knockback
{
    // Start is called before the first frame update
    void Start()
    {
        
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
            EnemyScript enemy = this.GetComponent<EnemyScript>();
            

            if (hit != null && enemy != null)
            {
                if (player.currentState != PlayerStates.stagger && hit.IsTouching(enemy.attackHitboxes))
                {
                    Vector2 difference = player.transform.position - transform.position;
                    Vector2 force = difference.normalized * thrust;
                    Debug.Log("Player knocked back");
                    player.currentState = PlayerStates.stagger;
                    player.Knock(force, .2f);
                }
            }
        }
    }
}
