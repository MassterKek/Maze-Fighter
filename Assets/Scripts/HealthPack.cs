using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D hit) {
        if (hit.gameObject.tag == "Player") {
            Debug.Log("RESTORE\n 50 health");
            if(PlayerControl.health > 50)
                PlayerControl.health = 100;
            else
                PlayerControl.health += 50;

            GridManager.availableHealth-=1;
            Destroy(gameObject, 0.0f);
        }
        else if (hit.gameObject.tag == "Enemy") {
            Debug.Log("RESTORE\n 50 health");
            if(EnemyControl.health > 50)
                EnemyControl.health = 100;
            else
                EnemyControl.health += 50;
            GridManager.availableHealth-=1;
            Destroy(gameObject, 0.0f);
        }
    }
}

