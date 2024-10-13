using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LookDev;
using UnityEngine.EventSystems;

public class ItemSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float moveTime = 0.1f;

    [Range(0f, 2f), SerializeField] private float scaleAmount = 1.1f;

    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;

    }

    private IEnumerator ScaleItem(bool startingAnimaiton)
    {
        Vector3 endScale;

        float elapsedtime = 0f;
        while (elapsedtime < moveTime)
        {
            elapsedtime += Time.deltaTime;

            if (startingAnimaiton)
            {
                endScale = startScale * scaleAmount;
            }

            else
            {
                endScale = startScale;
            }

            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedtime / moveTime));

            transform.localScale = lerpedScale;

            yield return null;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData)
    {
        StartCoroutine(ScaleItem(true));
    }

    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine (ScaleItem(false));
    }
}
