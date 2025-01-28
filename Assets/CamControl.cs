using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    [SerializeField] float sensX;
    [SerializeField] float sensY;

    [SerializeField] float clampAngleX = 75f; // Horizontal clamp angle
    [SerializeField] float clampAngleY = 75f; // Vertical clamp angle

    [SerializeField] Transform orientation;

    float rotationX;
    float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY * Time.deltaTime;

        rotationY += mouseX;
        rotationX -= mouseY;

        // Clamp both axes
        rotationX = Mathf.Clamp(rotationX, -clampAngleY, clampAngleY);
        rotationY = Mathf.Clamp(rotationY, -clampAngleX, clampAngleX);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
