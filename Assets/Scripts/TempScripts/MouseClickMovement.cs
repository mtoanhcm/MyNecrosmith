using UnityEngine;

public class MouseClickMovement : MonoBehaviour
{
    public Camera mainCamera;              // Reference to the main camera
    public LayerMask groundLayer;       // Instance of the agent
    //public SteeringBehaviorAgent steeringAgent; // Steering behavior component

    void Update()
    {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits a valid ground layer
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                // Move the agent to the clicked position
                //steeringAgent.Initialize(hit.point);
            }
        }
    }
}
