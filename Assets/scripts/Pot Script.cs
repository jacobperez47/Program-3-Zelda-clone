using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotScript : MonoBehaviour
{
    
    private Animator anim;

    private BoxCollider2D collider;
    // Start is called before the first frame update
    void Start()
    {
     anim = GetComponent<Animator>();   
     collider = GetComponent<BoxCollider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void smash()
    {
        anim.SetBool("smashed", true);
        collider.enabled = false;
        
    }
}
