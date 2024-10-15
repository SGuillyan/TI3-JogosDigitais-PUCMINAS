using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Configurações de Zoom")]
    [Tooltip("Zoom mínimo permitido.")]
    public float minZoom = 5f;
    [Tooltip("Zoom máximo permitido.")]
    public float maxZoom = 12f;
    [Tooltip("Velocidade do zoom.")]
    public float zoomSpeed = 5f;
    [Tooltip("Suavidade da transição do zoom.")]
    public float smoothSpeed = 10f;

    private Camera cam;
    private float targetZoom;

    void Start()
    {
        // Obtém a referência da câmera anexada a este GameObject
        cam = GetComponent<Camera>();

        // Verifica se a câmera é ortográfica
        if (!cam.orthographic)
        {
            Debug.LogError("O script CameraZoom requer uma câmera ortográfica.");
            return;
        }

        // Define o zoom inicial como o tamanho atual da câmera
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoom();
    }

    void HandleZoom()
    {
        // Obtém a entrada do scroll do mouse
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Calcula o novo zoom baseado na entrada do scroll e na velocidade do zoom
        targetZoom -= scrollData * zoomSpeed;
        
        // Limita o valor do zoom entre os valores mínimo e máximo
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Suaviza a transição do zoom usando interpolação linear
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * smoothSpeed);
    }
}
