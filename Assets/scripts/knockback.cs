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
        if (other.CompareTag("enemy") || other.CompareTag("Player"))
        {
            Rigidbody2D hit = other.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                if (other.CompareTag("enemy"))
                {
                    if (hit.GetComponent<EnemyScript>().currentState != EnemyStates.stagger)
                    {
                        hit.GetComponent<EnemyScript>().currentState = EnemyStates.stagger;
                        Vector2 difference = hit.transform.position - transform.position;
                        Vector2 force = difference.normalized * thrust;
                        other.GetComponent<EnemyScript>().Knock(hit, force,.2f);
                        
                    }
                }

                //StartCoroutine(KnockbackCoroutine(hit));
            }
        }
    }

    
}