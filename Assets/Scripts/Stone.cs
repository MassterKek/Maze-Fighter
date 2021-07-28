using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public float stone_speed;
    public static Rigidbody2D rb;
    public int damage;

    void Start()
    {
        damage = 20;
        rb.velocity = transform.right * stone_speed;
    }

    private void OnTriggerEnter2D(Collider2D hit) {

        // if (EnemyControl.enemy != null) {
        //     enemy.TakeDamage(damage);
        // }
    } 

    // public static void Invert() {
    //     if (Input.GetKey("up")) {
    //         rb.position = transform.Rotate(0f, 90f, 0f, 0f);
    //     }

    //     if (Input.GetKey("left")) {
    //         rb.position = transform.Rotate(-180f, 0f, 0f, 0f);
    //     }

    //     if (Input.GetKey("down")) {
    //         rb.position = transform.Rotate(0f, -90f, 0f, 0f);
    //     }

    //     if (Input.GetKey("right")) {
    //         rb.position = transform.Rotate(180f, 0f, 0f, 0f);
    //     }
    // }
}
