using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraControl
{
    [RequireComponent(typeof(Camera))]
    public class IsometricCamera : MonoBehaviour
    {
        public Transform TargetFollow { get; private set; } // The object the camera will follow
        
        [SerializeField]private float distance; // Default distance from the focus point
        [SerializeField]private float rotationX; // Camera rotation around the X-axis
        [SerializeField]private float rotationY; // Camera rotation around the Y-axis
        [SerializeField]private float followSpeed; // Speed at which the camera follows the target
        [SerializeField]private float moveSpeed; // Speed of W, A, S, D movement
        [SerializeField]private float zoomSpeed; // Speed of zooming in/out
        [SerializeField]private float minDistance; // Minimum zoom distance
        [SerializeField]private float maxDistance; // Maximum zoom distance
        [SerializeField]private float fov; // Field of view for the camera

        private Camera camera;
        private Vector2 moveInput;
        private float zoomInput;
        private bool isFollowing;

        // Variables to store camera adjustments
        private Vector3 zoomAdjustment = Vector3.zero;

        // Reference to the generated input actions
        private Controller cameraControls;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            cameraControls = new Controller();

            // Bind the move action
            cameraControls.CameraControl.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            cameraControls.CameraControl.Move.canceled += ctx => moveInput = Vector2.zero;

            // Bind zoom action
            cameraControls.CameraControl.Zoom.performed += ctx => zoomInput = ctx.ReadValue<float>();

            // Bind space bar to toggle following behavior
            cameraControls.CameraControl.ToggleFollow.performed += ctx =>
            {
                if (TargetFollow != null)
                {
                    isFollowing = !isFollowing;
                }
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
            camera.fieldOfView = fov;

            // Initialize the camera's rotation
            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);

            // Initialize isFollowing based on the presence of a target
            isFollowing = TargetFollow != null;
        }

        private void Update()
        {
            // Adjust distance based on zoom input
            if (zoomInput != 0)
            {
                float oldDistance = distance;
                distance -= zoomInput * zoomSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);

                // If not following, calculate zoom adjustment towards mouse position
                if (!isFollowing)
                {
                    zoomAdjustment = CalculateZoomAdjustment(oldDistance, distance);
                }
                else
                {
                    zoomAdjustment = Vector3.zero;
                }

                zoomInput = 0f; // Reset zoomInput to avoid continuous zooming
            }
            else
            {
                zoomAdjustment = Vector3.zero;
            }
        }

        private Vector3 CalculateZoomAdjustment(float oldDistance, float newDistance)
        {
            // Get the mouse position in screen coordinates
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Convert mouse position to a ray
            Ray mouseRay = camera.ScreenPointToRay(mousePosition);

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
            Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
            Vector3 direction = rotation * Vector3.forward;

            // Handle movement input
            if (moveInput != Vector2.zero)
            {
                // If the camera is following, stop following when a move input is detected
                if (isFollowing)
                {
                    isFollowing = false;
                }

                // Calculate movement
                Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * (moveSpeed * Time.deltaTime);
                movement = rotation * movement; // Rotate movement to match camera orientation

                // Update the camera's position
                Vector3 newPosition = transform.position + movement + zoomAdjustment;

                // Adjust the camera's height based on distance and rotationX
                newPosition.y = Mathf.Sin(Mathf.Deg2Rad * rotationX) * distance;

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
                newPosition.y = Mathf.Sin(Mathf.Deg2Rad * rotationX) * distance;

                // Apply the new position
                transform.position = newPosition;
            }

            // Reset zoom adjustment after applying it
            zoomAdjustment = Vector3.zero;

            // Set the camera's rotation
            transform.rotation = rotation;
        }
    }
}
