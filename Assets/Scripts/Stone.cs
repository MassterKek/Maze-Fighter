using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int damage;
    public Rigidbody2D rb;
    public float stone_speed;

    private void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.tag == "Enemy") {
            EnemyControl.health-=20;
            Destroy(gameObject, 0.0f);
            Debug.Log("Enemy health: "+EnemyControl.health);
        }

        if (hit.gameObject.name == "WallStone(Clone)(Clone)") {
            Debug.Log("HIT A WALL");
            Destroy(gameObject, 0.0f);
        }
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

    void Start()
    {
        damage = 20;
        Direction();
    }
}
