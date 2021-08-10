using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float lazer_speed;
    public Rigidbody2D rb;
    public int damage;
    //public int hitCount = 0;
    //public EnemyControl enemy;

    void Start()
    {
        damage = 20;
        //rb.velocity = transform.up * lazer_speed * Time.deltaTime;
        Direction();
    }

    void Direction() {
        string enemy_direction = EnemyControl.direction;

        if (enemy_direction.Equals("up")) {
            rb.velocity = transform.up * lazer_speed * Time.deltaTime;
        } 

        if (enemy_direction.Equals("left")) {
            rb.velocity = -transform.right * lazer_speed * Time.deltaTime;
        } 

        if (enemy_direction.Equals("down")) {
            rb.velocity = -transform.up * lazer_speed * Time.deltaTime;
        } 

        if (enemy_direction.Equals("right")) {
            rb.velocity = transform.right * lazer_speed * Time.deltaTime;
        } 
    }

    private void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.tag == "Player") {
            Debug.Log("DAMAGE\n-20 of health");
            // hitCount++;

            // if (hitCount == 5) {
            //     Destroy(, 0f);
            // }
        }

        if (hit.gameObject.name == "WallStone(Clone)(Clone)") {
            Debug.Log("HIT A WALL");
            Destroy(gameObject, 0.0f);
        }
    }
}
