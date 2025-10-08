using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    idle,
    walk,
    attack,
    stagger,
}
public class EnemyScript : MonoBehaviour
{
    public int health;

    public string enemyName;

    public int baseAttack;
    
    public EnemyStates currentState;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}