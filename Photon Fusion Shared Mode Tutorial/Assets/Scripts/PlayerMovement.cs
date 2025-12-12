using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController controller;
    public Camera Camera;

    [SerializeField] private float playerSpeed = 2f;

    private Vector3 velocity;
    private bool jumpPressed;
    public float JumpForce = 5f;
    public float GravityValue = -9.81f;

    private void Awake(){
        Application.runInBackground = true;
        controller = GetComponent<CharacterController>();
    } 

    private void Update(){
        if(Input.GetButtonDown("Jump")){
            jumpPressed = true;
        }
    }

    public override void Spawned(){

        if(HasStateAuthority){
            Camera = Camera.main;
            Camera.GetComponent<FirstPersonCamera>().Target = transform;
        }
    }

    public override void FixedUpdateNetwork(){

        if(controller.isGrounded){
            velocity = new Vector3(0,-1,0);
        }

        Quaternion cameraRotationY = Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y, 0f);
        
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        velocity.y += GravityValue * Runner.DeltaTime;

        if(jumpPressed && controller.isGrounded){
            velocity.y += JumpForce;
        }

        controller.Move(move + velocity * Runner.DeltaTime);

        if(move != Vector3.zero){
            gameObject.transform.forward = move;
        }

        jumpPressed = false;
    }
}
