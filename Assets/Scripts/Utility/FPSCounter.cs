using UnityEngine;

namespace GameUtility
{
    public class FPSCounter : MonoBehaviour
    {
        [Tooltip("How often to update the FPS counter")]
        [SerializeField] private float updateInterval = 0.5f;

        [Tooltip("Font size for the FPS display")]
        [SerializeField] private int fontSize = 20;

        [Tooltip("Color of the FPS text")]
        [SerializeField] private Color textColor = Color.white;

        private float accum = 0.0f;
        private int frames = 0;
        private float timeLeft;
        private float fps;
        private GUIStyle style;
        private Rect rect;

        private void Start()
        {
            timeLeft = updateInterval;

            // Initialize GUIStyle
            style = new GUIStyle();
            style.alignment = TextAnchor.UpperRight;
            style.fontSize = fontSize;
            style.normal.textColor = textColor;

            // Set the position for the FPS counter
            // Width and height will be calculated based on text
            rect = new Rect(Screen.width - 100, 10, 90, 30);
        }

        private void Update()
        {
            // Increment frame counter
            timeLeft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            frames++;

            // Update FPS display when the interval has elapsed
            if (timeLeft <= 0.0f)
            {
                // Calculate average FPS for the interval
                fps = accum / frames;

                // Reset for next interval
                timeLeft = updateInterval;
                accum = 0.0f;
                frames = 0;
            }
        }

        private void OnGUI()
        {
            // Display FPS in the top-right corner
            GUI.Label(rect, string.Format("{0:F1} FPS", fps), style);
        }
    }
}
