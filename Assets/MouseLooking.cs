using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLooking : MonoBehaviour
{
    [Header("Camera Holder")]
    [SerializeField] GameObject cameraHolder;

    [Header("Sensetivity Variables")]
    [SerializeField] float mouseSensitivty;

    [Header("Debug Settings")]
    [SerializeField] public CursorLockMode cursorState;
    [SerializeField] bool mouseInvisible;

    float verticalLookRotation = 0;

    void Start()
    {
        Cursor.lockState = cursorState;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }
    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivty);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivty;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }
}
