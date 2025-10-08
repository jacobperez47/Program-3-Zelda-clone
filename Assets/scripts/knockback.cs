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
        if (other.CompareTag("enemy"))
        {
            Rigidbody2D enemy = other.GetComponent<Rigidbody2D>();
            LogScript log = other.GetComponent<LogScript>();
            if (enemy != null)
            {
                enemy.GetComponent<EnemyScript>().currentState = EnemyStates.stagger;
                StartCoroutine(KnockbackCoroutine(enemy));
                log.StartKnockBack(0.2f);
                
            }
        }
    }

    private IEnumerator KnockbackCoroutine(Rigidbody2D enemy)
    {
        if (!enemy)
        {
            yield break;
        }
        
        Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
        if (!enemyRb)
        {
            yield break;       
        }
        Vector2 difference = enemyRb.transform.position - transform.position;
        Vector2 force = difference.normalized * thrust;
        
        enemyRb.velocity = force;
        yield return new WaitForSeconds(0.2f);


        if (!enemyRb)
        {
            yield break;
        } 
        
        enemy.velocity = new Vector2();

        enemy.GetComponent<EnemyScript>().currentState = EnemyStates.idle;

    }
}
