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
    
        if (other.CompareTag("enemy") || other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 difference = hit.transform.position - transform.position;
                Vector2 force = difference.normalized * thrust;
                if (other.CompareTag("enemy"))
                {
                    if (hit.GetComponent<EnemyScript>().currentState != EnemyStates.stagger)
                    {
                        hit.GetComponent<EnemyScript>().currentState = EnemyStates.stagger;

                        other.GetComponent<EnemyScript>().Knock(hit, force, .2f);
                    }
                }

                if (other.CompareTag("Player") && this.CompareTag("enemy") &&
                    hit.GetComponent<PlayerMovement>().currentState != PlayerStates.stagger)
                {
                    if (!this.CompareTag("attack") && !this.CompareTag("vision"))
                    {
                        hit.GetComponent<PlayerMovement>().currentState = PlayerStates.stagger;

                        other.GetComponent<PlayerMovement>().Knock(hit, force, .3f);
                        ;
                    }
                }

                //StartCoroutine(KnockbackCoroutine(hit));
            }
        }
    }
}