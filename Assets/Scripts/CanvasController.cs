using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        Camera uiCamara = canvas.worldCamera;

        bool isTablet = uiCamara.aspect > (9f / 16f);

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.matchWidthOrHeight = isTablet ? 1f : 0f;
    }
}
