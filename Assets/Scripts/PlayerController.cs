﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public GameObject gun;
    public Animator shootanim;
    public static bool shooting;
    public static bool ADS;
    public static bool OverHeat;
    private bool CanCover;
    private bool CanCrouch;
    private bool Covering;
    private bool showText;
    public Slider HeatSlider;
    public Text CoverPopUp;
    public float crouchingSpeed;
    public float CspeedUp;
    public float GunHeat;

	// Use this for initialization
	void Start () {
        shooting = false;
        ADS = false;
        crouchingSpeed = 0.1f;
        CspeedUp = crouchingSpeed;
        GunHeat = 0.0f;
        showText = false;
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<Collider>().name == "Cover")
        {
            Debug.Log("I'm Working");
            showText = true;
        }
    }
    // Update is called once per frame
    void Update () {
        //Shooting mechanics
        if (Input.GetMouseButtonDown(0) && OverHeat == false)
        {
            if (!ADS)
            {
                Debug.Log("Pressed left click.");
                gun.GetComponent<Animator>().Play("Gun_Shoot");
                //shooting = true;
            }
            else if (ADS)
            {
                Debug.Log("Pressed left click.");
                gun.GetComponent<Animator>().Play("GunADS_Shoot");
                //shooting = true;
            }
            GunHeat += Time.deltaTime * 20;
        }

        //Aim mechanics
        if (Input.GetMouseButtonDown(1))
        {
            if (!ADS && !shooting)
            {
                Debug.Log("Pressed right click.");
                gun.GetComponent<Animator>().Play("GunToADS");
                ADS = true;
            } 
            else if (ADS && !shooting)
            {
                Debug.Log("Pressed right click.");
                gun.GetComponent<Animator>().Play("GunFromADS");
                ADS = false;
            }
        }

        //Crouching
        if (Input.GetKey("k"))
        {
            CspeedUp -= crouchingSpeed * (Time.deltaTime * 10);
            if (CspeedUp < -0.6f)
            {
                CspeedUp = -0.6f;
            }
        }

        else
        {
            
            CspeedUp += crouchingSpeed * (Time.deltaTime * 10);
            if (CspeedUp > 0.0f)
            {
                CspeedUp = 0.0f;
            }
        }
        transform.localPosition = new Vector3(0, CspeedUp, 0);
        GunHeat -= Time.deltaTime / 2;

        if (GunHeat < 0.0f)
        {
            GunHeat = 0.0f;
            OverHeat = false;
        }
        if (GunHeat > 1.5f) { OverHeat = true; }
        HeatSlider.value = GunHeat;
    }

    void OnGUI()
    {
        if (showText) {
        GUIStyle Coverstyle = new GUIStyle();
        Coverstyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height - 30, 400, 30), "Press L for cover.", Coverstyle);
        }
    }
}
