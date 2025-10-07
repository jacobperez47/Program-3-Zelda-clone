using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScript : EnemyScript


{
    private Transform target;


    public float speed;

    public int baseDamage;

    public CircleCollider2D vision;

    public CircleCollider2D attack;

    private bool isVisible;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isVisible = false;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (isVisible && target && rb)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            rb.MovePosition(transform.position + direction * (speed * Time.fixedDeltaTime));
        }
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
                Debug.Log("Player in vision range");
            }

            if (GetComponent<CircleCollider2D>() == attack)
            {
                Debug.Log("Player in attack range");
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

            Debug.Log("Player out of vision range");
        }
    }
}