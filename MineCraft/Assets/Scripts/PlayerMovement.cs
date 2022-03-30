using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //refrences
    CharacterController controllerr;
    //camera's transform
    public GameObject Camera;

    Animator anim ;
    Rigidbody rb;

    //variables
    public float walkspeed = 5f;
    public float runspeed = 7.5f;
    public float horizontalmove;
    public float verticalmove;
    public float turnsmoothvelocity;
    public float turnsmoothtime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 1f;
    public float gravityMultiplier = 1f;
    Vector3 move;
    Vector3 movedir;
    Vector3 playerVelocity;
    bool isGrounded;



    // Start is called before the first frame update
    void Start()
    {
        controllerr = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //making the mouse invisble 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controllerr.isGrounded;
        //setting idle animation
        Idle();

        //setting walking animation
        //checking if the moving speed doesn't equal zero (the charcter is moving)
        if(verticalmove!=0 || horizontalmove != 0)
        {
            anim.SetFloat("speed" , 0.5f , 0f , Time.deltaTime);
        }

        //deciding weather to pass runspeed or walkspeed 
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Move(runspeed);
            if(verticalmove!=0 || horizontalmove != 0)
            {
                anim.SetFloat("speed" , 1f , 0f , Time.deltaTime);
            }
        }
        else
        {
            Move(walkspeed);
        }
        
        Jump();
    }

    void Move(float speed)
    {
        //taking a speed as an input then multipluing it by the movement variables to control speed in walking and running
        
        //getting positive or nigative raw (whole number) value to decide the direction of movement forword or backword
        verticalmove = Input.GetAxisRaw("Vertical");

        //getting positive or nigative raw (whole number) value to decide the direction of movement right or left
        horizontalmove =Input.GetAxisRaw("Horizontal");
        
        //writing the values to the console to view errors
        Debug.Log(verticalmove);
        Debug.Log(horizontalmove);

        //assigning the values into one vector3
        move = new Vector3(horizontalmove , 0f , verticalmove).normalized;
            //applying gravity to the character 
            // controllerr.SimpleMove(movedir.normalized * speed * Time.deltaTime);

        if(move.magnitude >= 0.1f)
        {
            float smoothedtarget = Mathf.Atan2(move.x , move.z) * Mathf.Rad2Deg + Camera.transform.eulerAngles.y; 
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y , smoothedtarget , ref turnsmoothvelocity , turnsmoothtime );
            transform.rotation = Quaternion.Euler(0f , angle , 0f);

            movedir = Quaternion.Euler(0f , angle , 0f) * Vector3.forward;
            // if(!isGrounded){movedir.y = gravity;}
            // else{movedir.y = 0f;}

            //moving the character 
            controllerr.Move(movedir.normalized * speed * Time.deltaTime);
        }
    }

    void Idle()
    {
        anim.SetFloat("speed" , 0 , 0f , Time.deltaTime);
    }
    void Jump()
    {
        // Changes the height position of the player..
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityMultiplier);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controllerr.Move(playerVelocity * Time.deltaTime);
    }
}
