using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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

    // all for flashlight
    public GameObject torch;
    RigBuilder m_RigBuilder;
    Rig m_Rig;
    private bool flashActive = false; // saddly didnt find out how to adress the weight slider in Rig through code
    private bool recentflash;
    private float lastActivationTime = 0.0f;       //last press off the 'f' button

    float distToColliderBottom;

    // death counter
    public int deaths = 0;
    public float deathLength = 1.0f;
    private float lastDeath = 0.0f;

    void Start ()
    {
        // animation copmponents
        animator = GetComponent<Animator> ();
        inJumping = false;
        inWalking = false;

        //flashlight components
        m_RigBuilder = GetComponent<RigBuilder> ();
        m_Rig = (m_RigBuilder.layers)[0].rig;
        m_Rig.weight = 0.0f;
        torch.SetActive(flashActive);

        //physik/movement
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_Collider = GetComponent<Collider> ();
        distToColliderBottom = m_Collider.bounds.extents.y - m_Collider.bounds.center.y;
    }

    void FixedUpdate ()
    {
        // timer for not activating flash multiple times on one press; 0.5 sec
        if(Input.GetKey("f") && lastActivationTime < (Time.time - 0.5f)) {
            flashActive = !flashActive;
            m_Rig.weight = (flashActive == true) ? 1.0f : 0.0f;
            torch.SetActive(flashActive);
            lastActivationTime = Time.time;
        }


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
            Death();
        } else if(collision.gameObject.tag == "NPC") {
            Death();
        }
    }

    bool IsGroundedRaycast() {
        return Physics.Raycast(transform.position, -Vector3.up, distToColliderBottom + 0.1f);
    }

    void Death() {
        deaths++;
        m_Rigidbody.position = new Vector3(0, 1, 0);
        lastDeath = Time.realtimeSinceStartup;
    }

    // display Death counter (after first death)
    protected virtual void OnGUI()
    {

        if(deaths == 0) return;

        float size = 20f; 
        
        // interpolate between max size and resting size
        if(lastDeath * Time.fixedDeltaTime < deathLength) {
            lastDeath += 1.0f;
            size += 80f * (1 - ((lastDeath * Time.fixedDeltaTime) / deathLength));
        }

        GUILayout.BeginArea(new Rect(10f, 2.5f, 800f, 150f));
        string content = "Deaths: " + deaths;
        GUILayout.Label($"<color='black'><size={size}>{content}</size></color>");
        GUILayout.EndArea();
    }
}
