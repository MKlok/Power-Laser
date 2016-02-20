﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class LaserGun : MonoBehaviour
{
    public Camera c;
    public GameObject laserparticles;
    public Transform particlerotation;

    LineRenderer line;
    Vector3 shotPoint;
    bool laserShot;
    float shotDelay;
    float speed;
    float alpha;

    int gunDamage;


    void Start()
    {

        line = GetComponent<LineRenderer>();
        line.enabled = false;
        laserShot = false;
        speed = 10;
        alpha = 1;
        shotDelay = 0;

        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        GameObject FirstPersonCharacter = GameObject.Find("FirstPersonCharacter");
        PlayerController playercontroller = FirstPersonCharacter.GetComponent<PlayerController>();

        //Gun damage
        //Currently pla


        //single shot pistol
        if (playercontroller.currentGun == 0)
        {
            gunDamage = 17;
            playercontroller.fireRate = 0.4f;
        }

        //burst fire pistol
        if (playercontroller.currentGun == 1)
        {
            gunDamage = 20;
            playercontroller.fireRate = 0.9f;
        }

        //revolver
        if (playercontroller.currentGun == 2)
        {
            gunDamage = 80;
            playercontroller.fireRate = 1.2f;
        }

        //semi automatic rifle
        if (playercontroller.currentGun == 3)
        {
            gunDamage = 40;
            playercontroller.fireRate = 0.6f;
        }

        //full automatic rifle
        if (playercontroller.currentGun == 4)
        {
            gunDamage = 20;
            playercontroller.fireRate = 0.1f;
        }

        //Bolt action rifle
        if (playercontroller.currentGun == 5)
        {
            gunDamage = 100;
            playercontroller.fireRate = 1.5f;
        }

        //pump action shotgun
        if (playercontroller.currentGun == 6)
        {
            gunDamage = 100;
            playercontroller.fireRate = 1.3f;
        }

        //semi automatic shotgun
        if (playercontroller.currentGun == 7)
        {
            gunDamage = 70;
            playercontroller.fireRate = 0.8f;
        }

        shotPoint = transform.position;
        if (Input.GetMouseButtonDown(0) && !PlayerController.shooting && !PlayerController.switchADS && !PlayerController.OverHeat) {
            StopCoroutine("ShootLaser");
            StartCoroutine("ShootLaser");
        }

        if (line.enabled && laserShot && shotDelay < 0.5)
        {
            alpha -= Time.deltaTime * speed;
            shotDelay += 1 * Time.deltaTime;
            Color start = Color.white;
            start.a = alpha;
            Color end = Color.black;
            end.a = alpha;
            line.SetColors(start, end);
        }
        if (line.enabled && laserShot && shotDelay >= 0.5)
        {
            line.enabled = false;
            alpha = 1;
            laserShot = false;
            shotDelay = 0;
        }
    }

    IEnumerator ShootLaser()
    {
        line.enabled = true;

        if (Input.GetMouseButtonDown(0))
        {
            line.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, Time.time);

            Ray ray = new Ray();
            ray = c.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            line.SetPosition(0, shotPoint);

            if (Physics.Raycast(ray, out hit, 100))
            {
                line.SetPosition(1, hit.point);
                if (hit.rigidbody && !hit.transform.GetComponent<TargetScript>().isAlive)
                {
                    hit.rigidbody.AddForceAtPosition(transform.forward * 1000, hit.point);
                }
                else if (hit.rigidbody)
                {
                    hit.rigidbody.AddForceAtPosition(transform.forward * 150, hit.point);
                    hit.transform.GetComponent<TargetScript>().GetHit(gunDamage);
                }
                Instantiate(laserparticles, hit.point, particlerotation.transform.rotation);
            }
            else
            {
                line.SetPosition(1, ray.GetPoint(100));
            }
            laserShot = true;
         yield return null;
        }

    PlayerController.shooting = true;
    }
}
