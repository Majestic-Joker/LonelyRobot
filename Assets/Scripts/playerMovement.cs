using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //Initialize variables, unseen ones first, then serialized private, then public
    private GameObject gameManager;
    private CharacterController controller;
    private GameObject Vacuum;
    private ParticleSystem vParticles;
    private Collider vCollider;
    private Collider playerCollider;
    private AudioSource moveAudio;
    private bool moving;
    private bool jumping;
    private bool sprinting;
    private bool sucking;
    private float sprintReset;
    private float turnSmoothVelocity;
    private Vector3 moveDirection;

    //Serialized variables - Seen in inspector, not editable by outside scripts.
    [SerializeField]
    private float speed = 10.0f;
    [SerializeField] [Tooltip("Should probably be double the speed value, or at least bigger than the speed value.")]
    private float sprintSpeed = 20.0f;
    [SerializeField]
    private float jumpHeight = 1.5f;
    [SerializeField]
    private float jumpSpeed = 0.5f;
    [SerializeField]
    private float gravity = 2.0f;
    [SerializeField]
    private float turnSpeed = 2.0f;
    

    
    //Runs every time the object is spawned
    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController");
        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();
        sprintReset = speed;
        Vacuum = transform.GetChild(0).gameObject;
        moveAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(moving == false)
        {
            moving = (Input.GetButton("Horizontal") || Input.GetButton("Vertical"));
        }

        if (jumping == false)
        {
            jumping = Input.GetButton("Jump");
        }

        if (sprinting == false)
            sprinting = Input.GetButton("Sprint");

        if (sucking == false)
            sucking = Input.GetButton("Fire1");


    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (!moveAudio.isPlaying)
                moveAudio.Play();
            Movement();
            moving = false;
        }
        else
        {
            if (moveAudio.isPlaying)
                moveAudio.Stop();
        }

        if (jumping)
        {
            Movement();
            jumping = false;
        }
        else if (controller.isGrounded)
            moveDirection.y = 0f;
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection);
        }

        if (sprinting)
        {
            speed = sprintSpeed;
            sprinting = false;
        }
        else
            speed = sprintReset;
        if (sucking)
        {
            Suck();
            sucking = false;
        }
        else
            StopSuck();

        if(transform.position.y < -5)
        {
            gameManager.GetComponent<GameManager>().GameOver(false);
        }
    }

    private void Movement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical);
        Vector3 transformDirection = transform.TransformDirection(inputDirection);

        Vector3 flatMovement = speed * Time.deltaTime * inputDirection;

        moveDirection = new Vector3(flatMovement.x, moveDirection.y, flatMovement.z);

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, jumpHeight);
        float proportionalHeight = (jumpHeight - hit.distance) / jumpHeight;

        if (jumping && transform.position.y < proportionalHeight)
                moveDirection.y = jumpSpeed;
        else if (controller.isGrounded)
            moveDirection.y = 0f;
        else
            moveDirection.y -= gravity * Time.deltaTime;

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
        controller.Move(moveDirection);
        jumping = false;
    }

    private void Suck()
    {
        Vacuum.GetComponentInChildren<suckCollider>().EnableSuckCollider();
        
        /*if (!vCollider.gameObject.activeSelf)
        {
            vParticles.Play();
            vCollider.gameObject.SetActive(true);
        }*/
    }
    private void StopSuck()
    {
        Vacuum.GetComponentInChildren<suckCollider>().DisableSuckCollider();

        /*if (vCollider.gameObject.activeSelf)
        {
            vParticles.Stop();
            vCollider.gameObject.SetActive(false);
        }*/
    }


}
