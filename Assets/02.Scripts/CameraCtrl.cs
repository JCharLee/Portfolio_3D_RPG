using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float height = 1.75f;
    public float camSpeed = 2f;
    public float curPan = 0f;
    public float curTilt = 10f;
    public float dist = 5f;
    public float curDist = 5f;

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
        // 카메라 각도
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

        // 카메라 거리
        dist -= Input.GetAxis("Mouse ScrollWheel") * camSpeed;
        dist = Mathf.Clamp(dist, 0f, maxDist);

        // 카메라 충돌
        Vector3 castDir = (mainCam.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, castDir, out hit, curDist))
        {
            if (hit.collider != mainCam)
            {
                dist = hit.distance;
            }
            else
                dist = curDist;
        }
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + (Vector3.up * height);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, curPan, transform.eulerAngles.z);
        tilt.eulerAngles = new Vector3(curTilt, tilt.eulerAngles.y, tilt.eulerAngles.z);
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, transform.position + tilt.forward * -dist, 0.03f);
    }
}