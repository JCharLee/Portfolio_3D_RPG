using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float height;
    public float camSpeed;
    public float curPan;
    public float curTilt;
    public float dist;

    float maxDist = 10f;
    float yMax = 90f;

    public Movement player;
    public Transform tilt;
    public Camera mainCam;

    void Start()
    {
        transform.position = player.transform.position + (Vector3.up * height);
        transform.rotation = player.transform.rotation;

        tilt.eulerAngles = new Vector3(curTilt, transform.eulerAngles.y, transform.eulerAngles.z);
        mainCam.transform.position += tilt.forward * -dist;
    }

    void Update()
    {
        // ī�޶� ����
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            //Cursor.lockState = CursorLockMode.Locked;
            curPan += Input.GetAxis("Mouse X") * camSpeed;
            curTilt -= Input.GetAxis("Mouse Y") * camSpeed;
            curTilt = Mathf.Clamp(curTilt, -yMax, yMax);
            Vector3 playerDir = player.transform.eulerAngles;
            playerDir.y = transform.eulerAngles.y;
        }
        else
            //Cursor.lockState = CursorLockMode.None;

        // ī�޶� �Ÿ�
        dist -= Input.GetAxis("Mouse ScrollWheel") * camSpeed;
        dist = Mathf.Clamp(dist, 0f, maxDist);

        // ī�޶� �浹
        
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + (Vector3.up * height);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, curPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(curTilt, tilt.eulerAngles.y, tilt.eulerAngles.z);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, transform.position + tilt.forward * -dist, 0.03f);
    }
}