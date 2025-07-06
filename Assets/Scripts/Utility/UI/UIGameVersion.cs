using TMPro;
using UnityEngine;

namespace GameUtility.UI {
    public class UIGameVersion : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI versionTxt;

        private void Awake()
        {
            if (versionTxt == null)
            {
                Debug.LogError("Version TextMeshProUGUI is not assigned in the inspector.");
                return;
            }

            // Set the version text to the current game version
            versionTxt.text = $"Version: {Application.version}";
        }
    }
}
