using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float movementSpeed = 0.1f;
    public float jumpSpeed = 2f;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    bool isGrounded = false;

    void Start ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        float jumpAxis = Input.GetAxis("Jump");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        bool wantsToJump = !Mathf.Approximately (jumpAxis, 0f);

        if (wantsToJump && isGrounded) {
            m_Rigidbody.velocity += new Vector3(0, jumpSpeed, 0);
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * movementSpeed);
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name == "Ground") {
            isGrounded = true;
            //Debug.Log("grounded");
        }
    }
 
    void OnCollisionExit(Collision collision) {
        if(collision.gameObject.name == "Ground") {
            isGrounded = false;
        }
    }
}
