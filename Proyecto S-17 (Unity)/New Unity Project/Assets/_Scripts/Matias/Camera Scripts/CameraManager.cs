using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField] Transform targetTransform; //Object to follow
    [SerializeField] Transform cameraPivot; //Up & Down
    [SerializeField] Transform cameraTransform; //Transform of the actual camera object in the scene
    [SerializeField] LayerMask collisionLayers; //The layers the camera will collide with
    [SerializeField] float defaultPosition;
    [SerializeField] Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] Vector3 cameraVectorPosition;

    [SerializeField] float cameraCollisionOffset = 0.2f; //How much the camera will jump off of the objects its colliding with
    [SerializeField] float minimumCollisionOffset = 0.2f;
    [SerializeField] float cameraCollisionRadius = 2;
    [SerializeField] float cameraFollowSpeed = 0.2f;
    [SerializeField] float cameraLookSpeed = 2f;
    [SerializeField] float cameraPivotSpeed = 2f;

    [SerializeField] float lookAngle; //Up & Down
    [SerializeField] float pivotAngle; //Left & Right
    [SerializeField] float minimumPivotAngle = -35;
    [SerializeField] float maximumPivotAngle = 35;

    private void Awake()
    {
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        inputManager = FindObjectOfType<InputManager>();
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);//Updates to the player position
        
        transform.position = targetPosition; //Set the position to the new position
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (inputManager.cameraInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (inputManager.cameraInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);

        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =- (distance - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
