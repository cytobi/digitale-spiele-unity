using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Animation components
    Animator animator;
    // prevent unnecesarry commands to animator
    bool inJumping;
    bool inWalking;


    public float turnSpeed = 20f;
    public float movementSpeed = 0.1f;
    public float jumpSpeed = 2f;
    public float horizontalSpeedFactor = 1f;

    Rigidbody m_Rigidbody;
    Collider m_Collider;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    float distToColliderBottom;

    void Start ()
    {
        // animation copmponents
        animator = GetComponent<Animator> ();
        inJumping = false;
        inWalking = false;

        //physik/movement
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_Collider = GetComponent<Collider> ();
        distToColliderBottom = m_Collider.bounds.extents.y;
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        float jumpAxis = Input.GetAxis("Jump");
        
        m_Movement.Set(horizontal * horizontalSpeedFactor, 0f, vertical);
        m_Movement.Normalize ();

        float camera_angle = Camera.main.transform.rotation.eulerAngles.y;
        m_Movement = Quaternion.Euler(0, camera_angle, 0) * m_Movement;
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        bool wantsToJump = !Mathf.Approximately (jumpAxis, 0f);
        bool isGrounded = IsGroundedRaycast();

        // animation transitions
            // walking
        if(!inWalking && isWalking) {
            animator.SetBool("isWalking", true);
            inWalking = true;
        }
            // stop walking
        else if (inWalking && !isWalking) {
            animator.SetBool("isWalking", false);
            inWalking = false;
        }
            // jumping
        if(!isGrounded && !inJumping) {
            animator.SetBool("isJumping", true);
            inJumping = true;
        }
            // landing
        if(inJumping && isGrounded) {
            animator.SetBool("isJumping", false);
            inJumping = false;
        }
        Debug.Log(inJumping);
        Debug.Log(inWalking);
        
        if (wantsToJump && isGrounded) {
            m_Rigidbody.velocity += new Vector3(0, jumpSpeed, 0);
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * movementSpeed);
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name == "Deathplane1") {
            m_Rigidbody.position = new Vector3(0, 1, 0);
        } else if(collision.gameObject.tag == "NPC") {
            m_Rigidbody.position = new Vector3(0, 1, 0);
        }
    }

    bool IsGroundedRaycast() {
        return Physics.Raycast(transform.position, -Vector3.up, distToColliderBottom + 0.1f);
    }
}
