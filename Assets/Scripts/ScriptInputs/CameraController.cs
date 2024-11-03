using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera m_camera;

    [SerializeField] private float moveSpd;
    [SerializeField] private float zoomRate;

    private Vector2 lastPosition;

    private void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    public void CameraInput()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                // Diferença de distância entre os toques de um frame para o outro
                float prevTouchDeltaMag = (touch1.position - touch1.deltaPosition - (touch2.position - touch2.deltaPosition)).magnitude;
                float touchDeltaMag = (touch1.position - touch2.position).magnitude;

                // Diferença de distância entre os toques (zoom-in ou zoom-out)
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // Ajusta o zoom baseado na diferença de magnitude do gesto
                AdjustZoom(deltaMagnitudeDiff * -0.01f);  // Ajuste para controlar a sensibilidade do zoom por pinça
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

                    // angulo 45°
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

    private void AdjustZoom(float zoomChange)
    {
        if (m_camera.orthographic)
        {
            // Projeção ortográfica: ajusta o tamanho ortográfico
            m_camera.orthographicSize = Mathf.Clamp(m_camera.orthographicSize - zoomChange * zoomRate, 10, 30);
        }
        else
        {
            // Projeção em perspectiva: ajusta o campo de visão
            m_camera.fieldOfView = Mathf.Clamp(m_camera.fieldOfView - zoomChange * zoomRate, 10, 30);
        }
    }
}
