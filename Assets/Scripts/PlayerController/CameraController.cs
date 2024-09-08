using Map;
using Observer;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlayerController {
    public class CameraController : MonoBehaviour
    {
        [Header("----- Move Camera -----")]
        public float panSpeed = 5f;
        public float edgeBoundary = 10f; // Distance from the screen edge to start moving the camera
        public float cameraMovementThreshold = 1f;
        public float panSmoothing = 0.1f; // Used for smoothing camera movement
        public float boundsPadding = 1f; // Padding to make camera bounds tighter

        [Header("----- Zoom Camera -----")]
        public float zoomSpeed = 10f;    // Zoom speed
        public float minZoom = 5f;       // Min zoom
        public float maxZoom = 20f;      // Max zoom

        private FogManager fogOfWar;
        private bool isFogVisible;

        private Vector3Int minNullTile;
        private Vector3Int maxNullTile;
        private Vector3 lastCameraPosition;
        private Vector3 currentVelocity; // Used for smooth camera movement
        private Vector3 worldMin, worldMax;

        private bool boundsNeedRecalculation = true;

        private void Start()
        {
            fogOfWar = MapManager.Instance.fogManager;

            // Initialize the tile bounds to extreme values
            minNullTile = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            maxNullTile = new Vector3Int(int.MinValue, int.MinValue, 0);

            lastCameraPosition = transform.position;

            CalculateNullTileBounds();  // Calculate initial bounds

            EventManager.Instance.StartListening<EventData.OpenFogWarSuccessEvent>(OnFogCleared);
        }

        private void OnFogCleared(EventData.OpenFogWarSuccessEvent data)
        {
            if (data.IsOpenFogSuccess)
            {
                boundsNeedRecalculation = true; // Recalculate bounds when fog is cleared
            }
        }

        //private void Update()
        //{
        //    if ((transform.position - lastCameraPosition).sqrMagnitude > cameraMovementThreshold * cameraMovementThreshold)
        //    {
        //        lastCameraPosition = transform.position;
        //        boundsNeedRecalculation = true; // Mark bounds for recalculation
        //    }
        //}

        private void LateUpdate()
        {
            if (boundsNeedRecalculation)
            {
                CalculateNullTileBounds();
                boundsNeedRecalculation = false;
            }

            PanCameraWithEdgeMovement();
            ZoomCamera();
        }

        void PanCameraWithEdgeMovement()
        {
            Vector3 move = Vector3.zero;
            Vector3 mousePosition = Input.mousePosition;

            // Check if the mouse is near the right edge of the screen
            if (mousePosition.x >= Screen.width - edgeBoundary)
            {
                move.x += panSpeed * Time.deltaTime;
            }
            // Check if the mouse is near the left edge of the screen
            if (mousePosition.x <= edgeBoundary)
            {
                move.x -= panSpeed * Time.deltaTime;
            }
            // Check if the mouse is near the top edge of the screen
            if (mousePosition.y >= Screen.height - edgeBoundary)
            {
                move.y += panSpeed * Time.deltaTime;
            }
            // Check if the mouse is near the bottom edge of the screen
            if (mousePosition.y <= edgeBoundary)
            {
                move.y -= panSpeed * Time.deltaTime;
            }

            if (move != Vector3.zero)
            {
                // Smooth the camera movement
                Vector3 targetPosition = Camera.main.transform.position + move;
                Vector3 smoothedPosition = Vector3.SmoothDamp(Camera.main.transform.position, targetPosition, ref currentVelocity, panSmoothing);

                // Clamp the smoothed camera position within the tightened null tile bounds
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, worldMin.x + boundsPadding, worldMax.x - boundsPadding);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, worldMin.y + boundsPadding, worldMax.y - boundsPadding);

                // Apply the position to the camera
                Camera.main.transform.position = smoothedPosition;
            }
        }

        void ZoomCamera()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float newSize = Camera.main.orthographicSize - scroll * zoomSpeed;

            // Clamp zoom in/out
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

            Camera.main.orthographicSize = newSize;
        }

        // Use a coroutine to spread the work of recalculating tile bounds over several frames
        IEnumerator CalculateNullTileBoundsCoroutine()
        {
            // Get the frustum corners in world space (for 2D, we need just 4 corners)
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.farClipPlane));

            // Convert world space positions to tilemap cells
            Vector3Int minCellPos = fogOfWar.FogMap.WorldToCell(bottomLeft);
            Vector3Int maxCellPos = fogOfWar.FogMap.WorldToCell(topRight);

            // Reset the min and max null tile bounds
            minNullTile = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            maxNullTile = new Vector3Int(int.MinValue, int.MinValue, 0);

            // Spread the loop work over multiple frames
            for (int x = minCellPos.x; x <= maxCellPos.x; x++)
            {
                for (int y = minCellPos.y; y <= maxCellPos.y; y++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0); // Assuming Z = 0 for 2D tilemaps
                    TileBase tile = fogOfWar.FogMap.GetTile(tilePos);

                    if (tile == null)
                    {
                        // Update min and max null tile bounds
                        minNullTile.x = Mathf.Min(minNullTile.x, tilePos.x);
                        minNullTile.y = Mathf.Min(minNullTile.y, tilePos.y);
                        maxNullTile.x = Mathf.Max(maxNullTile.x, tilePos.x);
                        maxNullTile.y = Mathf.Max(maxNullTile.y, tilePos.y);
                    }
                }

                // Yield execution to prevent frame drops
                if (x % 5 == 0)  // Yield every few iterations to spread the load
                {
                    yield return null;
                }
            }

            // Convert the tilemap bounds to world space for clamping the camera movement
            worldMin = fogOfWar.FogMap.CellToWorld(minNullTile);
            worldMax = fogOfWar.FogMap.CellToWorld(maxNullTile);
        }

        // Start the coroutine to calculate tile bounds
        void CalculateNullTileBounds()
        {
            StopAllCoroutines();  // Ensure only one coroutine is running at a time
            StartCoroutine(CalculateNullTileBoundsCoroutine());
        }
    }

}
