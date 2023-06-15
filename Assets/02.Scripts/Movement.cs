using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public enum Mode { None, Gaze }
    public Mode mode = Mode.None;

    public float moveSpeed;
    public float rotSpeed;
    public float jumpForce;
    public bool onAir = false;

    private float h, v;

    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera mainCam;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;
    }

    void Start()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onAir = false;
            anim.SetBool("OnGround", true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onAir = true;
            anim.SetBool("OnGround", false);
        }
    }

    void Update()
    {
        KeyCtrl();
        MouseCtrl();
        Sprint();
        Jump();
    }

    void KeyCtrl()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 movedir = Rotating(h, v);

        if ((h != 0) || (v != 0))
        {
            transform.position += movedir.normalized * moveSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movedir), rotSpeed * Time.deltaTime);
            anim.SetBool("IsMove", true);
        }
        else
            anim.SetBool("IsMove", false);
    }

    Vector3 Rotating(float horizontal, float vertical)
    {
        Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0f, -forward.x);
        Vector3 newDir = (forward * vertical) + (right * horizontal);

        return newDir;
    }

    void MouseCtrl()
    {
        // 주시 모드
        if (Input.GetMouseButton(1))
        {
            mode = Mode.Gaze;

            Vector3 forward = mainCam.transform.TransformDirection(Vector3.forward);
            forward.y = 0f;
            forward = forward.normalized;
            transform.rotation = Quaternion.LookRotation(forward);

            anim.SetBool("IsGaze", true);
            anim.SetFloat("MoveX", h, 1f, 0.1f);
            anim.SetFloat("MoveY", v, 1f, 0.1f);
        }
        else
        {
            mode = Mode.None;
            anim.SetBool("IsGaze", false);
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && mode != Mode.Gaze)
        {
            moveSpeed = 8f;
            anim.SetFloat("Speed", 2f, 0.1f, 0.1f);
        }
        else
        {
            moveSpeed = 5f;
            anim.SetFloat("Speed", 1f, 0.1f, 0.1f);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!onAir)
            {
                onAir = true;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                anim.SetTrigger("Jump");
                anim.SetBool("OnGround", false);
            }
        }
    }
}