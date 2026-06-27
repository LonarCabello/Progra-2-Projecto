using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = cam.transform.forward;
    }
}
