using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform firePoint;
    public GameObject stone;

    public void shoot() {
        GameObject bullet = Instantiate(stone, firePoint.position, firePoint.rotation);
        Destroy(bullet, 1.5f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && PlayerControl.weaponCount > 0) {
            //PlayerControl.weaponCount-=1;
            shoot();
        }
    }
}
