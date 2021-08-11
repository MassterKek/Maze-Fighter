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
        Direction();
    }

    void Direction() {
        string player_direction = PlayerControl.direction;

        if (player_direction.Equals("up")) {
            rb.velocity = transform.up * stone_speed * Time.deltaTime;
        } 

        if (player_direction.Equals("left")) {
            rb.velocity = -transform.right * stone_speed * Time.deltaTime;
        } 

        if (player_direction.Equals("down")) {
            rb.velocity = -transform.up * stone_speed * Time.deltaTime;
        } 

        if (player_direction.Equals("right")) {
            rb.velocity = transform.right * stone_speed * Time.deltaTime;
        } 
    }

    private void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.tag == "Enemy") {
            Debug.Log("DAMAGE\n-20 of health");
        }

        if (hit.gameObject.name == "WallStone(Clone)(Clone)") {
            Debug.Log("HIT A WALL");
            Destroy(gameObject, 0.0f);
        }
    }
}
