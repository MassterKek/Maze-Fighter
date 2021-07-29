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
        //rb.velocity = transform.up * stone_speed * Time.deltaTime;
        Direction();
    }

    void Direction() {
        string player_direction = PlayerControl.direction;

        if (player_direction.Equals("up")) {
            rb.velocity = transform.up * stone_speed * Time.deltaTime;
            //transform.position = transform.position + new Vector3(0f, 4.5f, 0f);
        } 

        if (player_direction.Equals("left")) {
            rb.velocity = -transform.right * stone_speed * Time.deltaTime;
            //transform.position = -transform.right + new Vector3(4.5f, 0f, 0f);
        } 

        if (player_direction.Equals("down")) {
            rb.velocity = -transform.up * stone_speed * Time.deltaTime;
            //transform.position = transform.position + new Vector3(0f, -4.5f, 0f);
        } 

        if (player_direction.Equals("right")) {
            rb.velocity = transform.right * stone_speed * Time.deltaTime;
            //transform.position = transform.position + new Vector3(-4.5f, 0f, 0f);
        } 
    }

    // private void OnTriggerEnter2D(Collider2D hit) {
    //     if (EnemyControl.enemy != null) {
    //         enemy.TakeDamage(damage);
    //     }
    // }
}
