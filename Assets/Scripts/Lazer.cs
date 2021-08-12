using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public int damage;
    public Rigidbody2D rb;
    public float stone_speed;

    private void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.tag == "Player") {
            Debug.Log("DAMAGE\n-20 of health");
            PlayerControl.health-=20;
            Destroy(gameObject, 0.0f);
        }

        if (hit.gameObject.name == "WallStone(Clone)(Clone)") {
            Debug.Log("HIT A WALL");
            Destroy(gameObject, 0.0f);
        }
    }

    void Direction() {
        string enemy_direction = EnemyControl.direction;

        if (enemy_direction.Equals("up")) {
            rb.velocity = transform.up * stone_speed * Time.deltaTime;
        }

        if (enemy_direction.Equals("left")) {
            rb.velocity = -transform.right * stone_speed * Time.deltaTime;
        }

        if (enemy_direction.Equals("down")) {
            rb.velocity = -transform.up * stone_speed * Time.deltaTime;
        }

        if (enemy_direction.Equals("right")) {
            rb.velocity = transform.right * stone_speed * Time.deltaTime;
        }
    }

    void Start()
    {
        damage = 20;
        Direction();
    }
}

