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

    //variables
    public float walkspeed = 5f;
    public float runspeed = 7.5f;
    public float horizontalmove;
    public float verticalmove;



    // Start is called before the first frame update
    void Start()
    {
        controllerr = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        //making the mouse invisble 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //setting idle animation
        Idle();

        //setting walking animation
        //checking if the moving speed doesn't equal zero (the charcter is moving)
        if(verticalmove!=0 || horizontalmove != 0)
        {
            anim.SetFloat("speed" , 0.5f , 0.1f , Time.deltaTime);
        }

        //deciding weather to pass runspeed or walkspeed 
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Move(runspeed);
            anim.SetFloat("speed" , 1f , 0.1f , Time.deltaTime);
        }
        else
        {
            Move(walkspeed);
        }
        
        Rotate();
    }

    void Move(float speed)
    {
        //taking a speed as an input then multipluing it by the movement variables to control speed in walking and running
        
        //getting positive or nigative raw (whole number) value to decide the direction of movement forword or backword
        verticalmove = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        //getting positive or nigative raw (whole number) value to decide the direction of movement right or left
        horizontalmove =Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        
        //writing the values to the console to view errors
        Debug.Log(verticalmove);
        Debug.Log(horizontalmove);

        //assigning the values into one vector3
        Vector3 move = transform.right * horizontalmove + transform.forward * verticalmove;

        //applying gravity to the character 
        controllerr.SimpleMove(move);

        //moving the character 
        controllerr.Move(move);
    }

    void Rotate()
    {
        //assign the camera's rotation to the character
        var CharacterRotation = Camera.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
         
        transform.rotation = CharacterRotation;
    }

    void Idle()
    {
        anim.SetFloat("speed" , 0 , 0.1f , Time.deltaTime);
    }
}
