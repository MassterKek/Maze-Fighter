using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public float stone_speed;
    public Rigidbody2D rb;
    public int damage;
    
    void Start()
    {
        damage = 20;
        rb.velocity = transform.right * stone_speed;
    }

    private void OnTriggerEnter2D(Collider2D hit) {

        // if (enemy != null) {
        //     enemy.TakeDamage(damage);
        // }
    }

    void update() {
       // Destroy(Gun.stone, 5);
    }
}
