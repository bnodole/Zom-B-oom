using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    private float xRotation = 0f;
    public Transform weaponHolder;
    private Transform playerBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock the cursor to the center of the screen and hide it for first-person view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerBody = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        Scope();
    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // UP/DOWN (camera only)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);

        // Apply vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Apply horizontal rotation to the player body
        playerBody.Rotate(Vector3.up * mouseX); 

        weaponHolder.rotation = transform.rotation; 
    }

    void Scope()
    {
        if (Input.GetMouseButton(1))
        {
            weaponHolder.localPosition = new Vector3(0.00700000022f, 0.335999995f, 0.531300008f);
            this.GetComponent<Camera>().fieldOfView = 30;
        }
        else
        {
            weaponHolder.localPosition = new Vector3(0.449499995f, 0.305999994f, 0.531300008f);
            this.GetComponent<Camera>().fieldOfView = 60;
        }
    }
}
