using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MapUtil
{
    [RequireComponent(typeof(Rigidbody))]
    public class CameraController : MonoBehaviour
    {
        public float movingScale = 1.0f;
        Rigidbody rb;
        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        void LateUpdate()
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
                dir += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                dir += Vector3.right;
            if (Input.GetKey(KeyCode.W))
                dir += Vector3.forward;
            if (Input.GetKey(KeyCode.S))
                dir += Vector3.back;
            dir *= movingScale;
            if (Input.GetKey(KeyCode.LeftShift))
                dir *= 2.0f;
            // rb.velocity = dir * Time.deltaTime;
            rb.velocity = dir;
            //transform.localPosition += dir * Time.deltaTime;
        }
    }
}