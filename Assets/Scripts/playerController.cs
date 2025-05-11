using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpForce;
    [SerializeField] int gravity;

    Vector3 moveDir;
    Vector3 playerVel;

    bool isSprinting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Sprint();
    }

    void Movement()
    {
        if (controller.isGrounded)
            playerVel = Vector3.zero;

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);

       // Jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y = gravity * Time.deltaTime;

    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }

        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }
}
