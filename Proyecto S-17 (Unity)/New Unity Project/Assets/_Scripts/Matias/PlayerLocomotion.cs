using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;

    private Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;
    
    [Header("Movement Flags")]
    [SerializeField] bool isGrounded;

    [Header("Movement Speed")]
    [SerializeField] float movementSpeed = 7f;
    [SerializeField] float rotationSpeed = 15f;
    [SerializeField] float inAirTimer;
    [SerializeField] float leapingVelocity;
    [SerializeField] float fallingVelocity;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput; //Left & Right
        moveDirection.Normalize();
        moveDirection.y = 0; //We dont want the play to walk straight into the sky
        moveDirection = moveDirection * movementSpeed;

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFalling()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;

        if (!isGrounded)
        {
            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        /*if (Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.Up, out hit,))
        {

        }*/
    }
}
