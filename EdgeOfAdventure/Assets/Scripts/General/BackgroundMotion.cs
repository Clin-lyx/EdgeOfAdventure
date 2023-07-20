using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMotion : MonoBehaviour
{
    private Camera mainCamera;
    public float motionDelay;
    public float xOffset;
    public float yOffset;

    private void Awake()
    {
        GameObject gameObject = GameObject.FindWithTag("MainCamera");
        mainCamera = gameObject.GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Vector3 cameraPos = mainCamera.transform.position;
        transform.position = new Vector3(cameraPos.x * motionDelay + xOffset, cameraPos.y * motionDelay + yOffset, 0);
    }
}
