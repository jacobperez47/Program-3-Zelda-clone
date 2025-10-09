using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knockback : MonoBehaviour
{
    public float thrust;

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
        // if (other.CompareTag("breakable")) 
        // {
        //     other.GetComponent<PotScript>().smash();
        // }
        if (other.CompareTag("enemy"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                Vector2 force = difference.normalized * thrust;
                
                {
                    if (hit.GetComponent<EnemyScript>().currentState != EnemyStates.stagger)
                    {
                        hit.GetComponent<EnemyScript>().currentState = EnemyStates.stagger;

                        other.GetComponent<EnemyScript>().Knock(hit, force, .2f);
                    }
                }

                


                }
            
            
        }
        if (other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            Vector2 difference = hit.transform.position - transform.position;
            Vector2 force = difference.normalized * thrust;
            if(hit.GetComponent<Collider2D>().IsTouching(hit.GetComponent<EnemyScript>().attackHitboxes)){
                hit.GetComponent<PlayerMovement>().currentState = PlayerStates.stagger;

                hit.GetComponent<PlayerMovement>().Knock(force, .2f);

            }

               
        }
    }
}