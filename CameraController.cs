using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject manager;
    private Vector3 pModePosition, bModePosition, vel, mouseInput;
    private Quaternion pModeRotation, bModeRotation;
    private bool setup, moveMode;
    private float speed, rotationSpeed, speedCap;


    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Game Manager");
        pModePosition = Vector3.zero; bModePosition = Vector3.zero; 
        vel = Vector3.zero; mouseInput = new Vector3(90, 0, 0);
        pModeRotation = Quaternion.identity; bModeRotation = Quaternion.identity;
        setup = false; moveMode = false;
        speed = 0; rotationSpeed = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        SetSpeedCap();

        speed = transform.position.y / 16;
        if (speed > speedCap) speed = speedCap;

        if (!setup && manager.GetComponent<PathGenerator>().GetFinished())
            SetUp();


        if (manager.GetComponent<Instantiate>().GetBuildMode() && !moveMode)
        {
            transform.position = Vector3.SmoothDamp(transform.position, bModePosition, ref vel, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, bModeRotation, 0.2f);
            mouseInput = new Vector3(90, 0, 0);
        }
        if (!manager.GetComponent<Instantiate>().GetBuildMode() && !moveMode)
        {
            transform.position = Vector3.SmoothDamp(transform.position, pModePosition, ref vel, 0.2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, pModeRotation, 0.2f);
            mouseInput = new Vector3(40, 0, 0);
        }


        if (Input.GetMouseButton(0))
        {
            moveMode = true;
            Quaternion rotation = transform.rotation;
            transform.rotation = new Quaternion(0, rotation.y, rotation.z, rotation.w);
            Vector3 forward = transform.forward;
            transform.rotation = rotation;


            Vector3 position = new Vector3(forward.x * -Input.GetAxis("Mouse Y"), 0,
                forward.z * -Input.GetAxis("Mouse Y")) +
                new Vector3(transform.right.x * -Input.GetAxis("Mouse X"), 0,
                transform.right.z * -Input.GetAxis("Mouse X"));

            transform.position += position * speed;
        }

        if(Input.GetMouseButton(1))
        {
            moveMode = true;

            mouseInput += (new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * rotationSpeed);
            transform.rotation = Quaternion.Euler(mouseInput);
        }

        if(Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            moveMode = true;

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                transform.position -= transform.forward;
            else
                transform.position += transform.forward;
        }    
    }

    public void SetUp()
    {
        bModePosition = new Vector3(manager.GetComponent<PathGenerator>().GetMiddleX(), 12f, 
            manager.GetComponent<PathGenerator>().GetMiddleZ());

        bModeRotation = Quaternion.Euler(90, 0, 0);

        pModePosition = new Vector3(9.5f, 10f, manager.GetComponent<PathGenerator>().GetMinZ() - 5f);
        pModeRotation = Quaternion.Euler(40, 0, 0);

        transform.position = bModePosition;
        transform.rotation = bModeRotation;

        setup = true;
    }

    public void SetSpeedCap()
    {
        speedCap = 0.25f;
        int i = 10;

        while(transform.position.y / i >= 1)
        {
            speedCap += 0.25f;
            i += 10;
        }
    }

    public void Clear()
    { moveMode = false; }


}
