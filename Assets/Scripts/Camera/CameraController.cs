using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CameraControl
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Transform TargetFollow; // The object the camera will follow

        [SerializeField] private float distance; // Default distance from the focus point
        //[SerializeField] private float rotationX; // Camera rotation around the X-axis
        //[SerializeField] private float rotationY; // Camera rotation around the Y-axis
        [SerializeField] private float followSpeed; // Speed at which the camera follows the target
        [SerializeField] private float moveSpeed; // Speed of W, A, S, D movement
        [SerializeField] private float zoomSpeed; // Speed of zooming in/out
        [SerializeField] private float minZoom; // Minimum zoom distance
        [SerializeField] private float maxZoom; // Maximum zoom distance

        [Header("Move To Position")]
        [SerializeField] private float moveToPositionSpeed = 5f; // Speed when moving to a specific positio

        // Edge scrolling parameters
        [Header("Edge Scrolling")]
        [SerializeField] private bool enableEdgeScrolling = true;
        [SerializeField] private float edgeScrollThreshold = 20f; // Distance from screen edge that triggers scrolling

        private Camera myCamera;
        private Vector2 moveInput;
        private float zoomInput;
        private bool isFollowing;

        // Move to position variables
        private bool isMovingToPosition = false;
        private Vector3 targetPosition;
        private Vector3 startPosition;
        private float moveToPositionProgress = 0f;

        // Variables to store camera adjustments
        private Vector3 zoomAdjustment = Vector3.zero;
        private Vector2 edgeScrolling = Vector2.zero;

        // Reference to the generated input actions
        private Controller cameraControls;

        private void Awake()
        {
            myCamera = GetComponent<Camera>();
            cameraControls = new Controller();

            // Bind the move action
            cameraControls.CameraControl.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            cameraControls.CameraControl.Move.canceled += ctx => moveInput = Vector2.zero;

            // Bind zoom action
            cameraControls.CameraControl.Zoom.performed += ctx => zoomInput = ctx.ReadValue<float>();

            // Bind zoom speed
            cameraControls.CameraControl.QuickMove.performed += ctx => moveSpeed *= 2;
            cameraControls.CameraControl.QuickMove.canceled += ctx => moveSpeed /= 2;

            // Bind space bar to toggle following behavior
            cameraControls.CameraControl.QuickReturnBase.performed += ctx =>
            {
                MoveToPosition(Vector3.zero);
            };
        }

        private void OnEnable()
        {
            cameraControls.Enable();
        }

        private void OnDisable()
        {
            cameraControls.Disable();
        }

        private void Start()
        {
            // Set the camera's field of view
            myCamera.fieldOfView = (minZoom + maxZoom) / 2;

            // Initialize the camera's rotation
            //transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

            // Initialize isFollowing based on the presence of a target
            isFollowing = TargetFollow != null;
        }

        private void Update()
        {
            // Check for edge scrolling
            if (enableEdgeScrolling)
            {
                CheckEdgeScrolling();
            }

            // Adjust orthographic size based on zoom input
            if (zoomInput != 0)
            {
                float oldSize = myCamera.orthographicSize;
                myCamera.orthographicSize -= zoomInput * zoomSpeed;
                myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize, minZoom, maxZoom);

                // If not following, calculate zoom adjustment towards mouse position
                // if (!isFollowing)
                // {
                //     zoomAdjustment = CalculateZoomAdjustment(oldSize, camera.orthographicSize);
                // }
                // else
                // {
                //     zoomAdjustment = Vector3.zero;
                // }

                zoomInput = 0f; // Reset zoomInput to avoid continuous zooming
            }
            else
            {
                zoomAdjustment = Vector3.zero;
            }
        }


        /// <summary>
        /// Move camera to a specific world position smoothly
        /// </summary>
        /// <param name="worldPosition">Target world position to move to</param>
        [Button]
        private void MoveToPosition(Vector3 worldPosition)
        {
            // Stop following target if currently following
            isFollowing = false;

            // Calculate the camera position that would look at the world position
            //Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
            //Vector3 direction = Vector3.right;
            //Vector3 desiredCameraPosition = worldPosition - direction * distance;

            worldPosition.z = transform.position.z;

            // Set up move to position
            isMovingToPosition = true;
            startPosition = transform.position;
            //targetPosition = desiredCameraPosition;
            targetPosition = worldPosition;
            moveToPositionProgress = 0f;
        }

        /// <summary>
        /// Stop the current move to position operation
        /// </summary>
        private void StopMoveToPosition()
        {
            isMovingToPosition = false;
            moveToPositionProgress = 0f;
        }

        private void CheckEdgeScrolling()
        {
            // Get the current mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Get screen dimensions
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Reset edge scrolling input
            edgeScrolling = Vector2.zero;

            // If mouse is not in game view, don't apply edge scrolling
            if (!IsMouseInGameView())
            {
                return;
            }

            // Calculate horizontal position factor (-1 to 1) where 0 is center,
            // -1 is leftmost edge, and 1 is rightmost edge
            float horizontalFactor = (mousePosition.x / screenWidth) * 2 - 1;

            // Calculate vertical position factor (-1 to 1) where 0 is center,
            // -1 is bottom edge, and 1 is top edge
            float verticalFactor = (mousePosition.y / screenHeight) * 2 - 1;

            // Check if mouse is at screen edges
            bool isAtLeftEdge = mousePosition.x <= edgeScrollThreshold;
            bool isAtRightEdge = mousePosition.x >= screenWidth - edgeScrollThreshold;
            bool isAtBottomEdge = mousePosition.y <= edgeScrollThreshold;
            bool isAtTopEdge = mousePosition.y >= screenHeight - edgeScrollThreshold;

            // Only apply directional movement when at an edge
            if (isAtLeftEdge)
            {
                edgeScrolling.x = -1;

                // If also near top or bottom, add some vertical movement
                if (verticalFactor > 0.5f) // Upper half of the screen
                {
                    edgeScrolling.y = 0.5f; // Some upward movement
                }
                else if (verticalFactor < -0.5f) // Lower half of the screen
                {
                    edgeScrolling.y = -0.5f; // Some downward movement
                }
            }
            else if (isAtRightEdge)
            {
                edgeScrolling.x = 1;

                // If also near top or bottom, add some vertical movement
                if (verticalFactor > 0.5f) // Upper half of the screen
                {
                    edgeScrolling.y = 0.5f; // Some upward movement
                }
                else if (verticalFactor < -0.5f) // Lower half of the screen
                {
                    edgeScrolling.y = -0.5f; // Some downward movement
                }
            }

            if (isAtTopEdge)
            {
                edgeScrolling.y = 1;

                // If also near left or right, add some horizontal movement
                if (horizontalFactor < -0.5f) // Left half of the screen
                {
                    edgeScrolling.x = -0.5f; // Some leftward movement
                }
                else if (horizontalFactor > 0.5f) // Right half of the screen
                {
                    edgeScrolling.x = 0.5f; // Some rightward movement
                }
            }
            else if (isAtBottomEdge)
            {
                edgeScrolling.y = -1;

                // If also near left or right, add some horizontal movement
                if (horizontalFactor < -0.5f) // Left half of the screen
                {
                    edgeScrolling.x = -0.5f; // Some leftward movement
                }
                else if (horizontalFactor > 0.5f) // Right half of the screen
                {
                    edgeScrolling.x = 0.5f; // Some rightward movement
                }
            }

            // Normalize if vector length exceeds 1
            if (edgeScrolling.magnitude > 1)
            {
                edgeScrolling.Normalize();
            }
        }

        // Check if the mouse is within the game view bounds
        private bool IsMouseInGameView()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Simple bounds check
            bool withinScreenBounds = mousePosition.x >= 0 &&
                                     mousePosition.x <= Screen.width &&
                                     mousePosition.y >= 0 &&
                                     mousePosition.y <= Screen.height;

            // When running in editor, we need additional checks
#if UNITY_EDITOR
            if (Application.isEditor && !Application.isPlaying)
            {
                // In editor, we need to check if game view has focus
                // This is a bit more complex and requires some additional checks
                // that vary based on Unity version

                // A simple approximation would be to check mouse position against
                // the game view rect, but that's not directly accessible

                // For now, we'll just use Screen bounds, but be aware that
                // this might not work perfectly in all editor layouts
            }
#endif

            return withinScreenBounds;
        }

        private Vector3 CalculateZoomAdjustment(float oldDistance, float newDistance)
        {
            // Get the mouse position in screen coordinates
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Convert mouse position to a ray
            Ray mouseRay = myCamera.ScreenPointToRay(mousePosition);

            // Plane at y = 0 (assuming your ground plane is at y = 0)
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            // Calculate the point where the ray intersects the ground plane
            if (groundPlane.Raycast(mouseRay, out float enter))
            {
                Vector3 hitPoint = mouseRay.GetPoint(enter);

                // Calculate the difference in distance (positive when zooming in)
                float distanceDelta = oldDistance - newDistance;

                // Calculate zoom factor
                float zoomFactor = distanceDelta / oldDistance;

                // Calculate direction from camera to hit point
                Vector3 directionToHit = hitPoint - transform.position;

                // Return the adjustment vector
                return directionToHit * zoomFactor;
            }

            return Vector3.zero;
        }

        private void LateUpdate()
        {
            //Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
            Vector3 direction = Vector3.right;

            if (isMovingToPosition)
            {
                // Update progress
                moveToPositionProgress += moveToPositionSpeed * Time.deltaTime;

                // Use smooth step for easing
                float smoothProgress = Mathf.SmoothStep(0f, 1f, moveToPositionProgress);

                // Interpolate position
                Vector3 currentPos = Vector3.Lerp(startPosition, targetPosition, smoothProgress);
                transform.position = currentPos;

                // Check if movement is complete
                if (moveToPositionProgress >= 1f)
                {
                    transform.position = targetPosition;
                    StopMoveToPosition();
                }

                // Set rotation and return early to skip other movement logic
                //transform.rotation = rotation;
                return;
            }

            // Calculate the combined movement input (keyboard + edge scrolling)
            Vector2 combinedMovement = moveInput;

            // If edge scrolling is active and no keyboard input, use edge scrolling
            if (edgeScrolling != Vector2.zero)
            {
                // If the camera is following, stop following when edge scrolling is detected
                if (isFollowing)
                {
                    isFollowing = false;
                }

                // If no keyboard input is happening, use edge scrolling
                if (moveInput == Vector2.zero)
                {
                    combinedMovement = edgeScrolling;
                }
            }

            // Handle movement input
            if (combinedMovement != Vector2.zero)
            {
                // If the camera is following, stop following when a move input is detected
                if (isFollowing)
                {
                    isFollowing = false;
                }

                // Calculate movement - use edge scroll speed for edge scrolling
                float currentSpeed = moveSpeed;
                if (combinedMovement == moveInput)
                {
                    currentSpeed = moveSpeed;
                }

                Vector3 movement = new Vector3(combinedMovement.x, combinedMovement.y, 0) * (currentSpeed * Time.deltaTime);
                //movement = rotation * movement; // Rotate movement to match camera orientation

                // Update the camera's position
                Vector3 newPosition = transform.position + movement + zoomAdjustment;

                // Adjust the camera's height based on distance and rotationX
                //newPosition.y = Mathf.Sin(Mathf.Deg2Rad * rotationX) * distance;

                // Apply the new position
                transform.position = newPosition;
            }
            else if (isFollowing && TargetFollow != null)
            {
                // Following the target
                Vector3 desiredPosition = TargetFollow.position - direction * distance;

                // Apply zoom adjustment
                desiredPosition += zoomAdjustment;

                // Smoothly move the camera to the desired position
                transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            }
            else
            {
                // No movement input and not following a target
                // Apply zoom adjustment
                Vector3 newPosition = transform.position + zoomAdjustment;

                // Adjust the camera's height based on distance and rotationX
                //newPosition.y = Mathf.Sin(Mathf.Deg2Rad * rotationX) * distance;

                // Apply the new position
                transform.position = newPosition;
            }

            // Reset zoom adjustment after applying it
            zoomAdjustment = Vector3.zero;

            // Set the camera's rotation
            //transform.rotation = rotation;
        }
    }
}