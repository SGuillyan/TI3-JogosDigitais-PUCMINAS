using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public static bool lockCamera = false;

    private Camera m_camera;

    [Tooltip("Velocidade de movimento da tela")]
    [SerializeField] private float moveSpd;
    [Tooltip("Velocidade com que se d� zoom")]
    [SerializeField] private float zoomRate;
    [Tooltip("'Size' da c�mera m�ximo alcan�ado pelo zoom-out")]
    [SerializeField] private float maxZoom;
    [Tooltip("'Size' da c�mera m�nimo alcan�ado pelo zoom-in")]
    [SerializeField] private float minZoom;

    private Vector3 initialPosition;

    private Vector2 lastPosition;

    private void Start()
    {
        m_camera = GetComponent<Camera>();
        m_camera.orthographicSize = (minZoom + maxZoom) / 2;
        initialPosition = transform.position;
    }

    public void CameraInput()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            AdjustZoom(scrollInput);
        }

        if (Input.touchCount > 0)
        {
            if (!lockCamera)
            {
                if (Input.touchCount == 2)
                {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);

                    // Diferen�a de dist�ncia entre os toques de um frame para o outro
                    float prevTouchDeltaMag = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
                    float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                    // Diferen�a de dist�ncia entre os toques (zoom-in ou zoom-out)
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    // Ajusta o zoom baseado na diferen�a de magnitude do gesto
                    AdjustZoom(deltaMagnitudeDiff * -0.01f);  // Ajuste para controlar a sensibilidade do zoom por pin�a
                }
                else
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        lastPosition = touch.position;
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        Vector2 aux = (touch.position - lastPosition);

                        // angulo 45�
                        float angle = Mathf.PI / 4;
                        // Rotacionar vetor
                        float newX = aux.x * Mathf.Cos(angle) - aux.y * Mathf.Sin(angle);
                        float newY = aux.x * Mathf.Sin(angle) + aux.y * Mathf.Cos(angle);

                        aux = new Vector2(newX, newY);

                        transform.Translate(new Vector3(-aux.x, 0, -aux.y).normalized * (m_camera.orthographicSize / 20), Space.World);
                        lastPosition = touch.position;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        lastPosition = Vector2.zero;
                    }
                }
            }            
        }      
    }

    private void AdjustZoom(float zoomChange)
    {
        if (m_camera.orthographic)
        {
            // Proje��o ortogr�fica: ajusta o tamanho ortogr�fico
            m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize - zoomChange * zoomRate, minZoom, maxZoom);
        }
        else
        {
            // Proje��o em perspectiva: ajusta o campo de vis�o
            m_camera.fieldOfView = Mathf.Clamp(m_camera.fieldOfView - zoomChange * zoomRate, minZoom, maxZoom);
        }
    }

    public void ResetCameraPosition()
    {
        transform.position = initialPosition;
    }
}
