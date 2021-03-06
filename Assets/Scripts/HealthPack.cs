using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.tag == "Player") {
            Debug.Log("Player Health restored to "+PlayerControl.health);
            if(PlayerControl.health > 50)
                PlayerControl.health = 100;
            else
                PlayerControl.health += 50;
            Destroy(gameObject, 0.0f);
        }
        else if (hit.gameObject.tag == "Enemy") {
            Debug.Log("Enemy Health restored to "+EnemyControl.health);
            if(EnemyControl.health > 50)
                EnemyControl.health = 100;
            else
                EnemyControl.health += 50;
            Destroy(gameObject, 0.0f);
        }
    }
}

