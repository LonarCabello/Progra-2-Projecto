/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 28 / 05 / 2026
 * Title        : CameraControler
 * Description  : Control de camara 3ra persona.
 * =============<< ********* >>=============
 */

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float mouseSensitivity = 200f;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -35f, 60f);

        // Rotación horizontal
        transform.position = target.position;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
