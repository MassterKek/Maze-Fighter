using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject lazer;

    public void shoot() {
        GameObject bullet = Instantiate(lazer, firePoint.position, firePoint.rotation);
        Destroy(bullet, 1.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && PlayerControl.cooldown > PlayerControl.cdTime) {
            PlayerControl.cooldown = 0;
            shoot();
        }
    }
}
