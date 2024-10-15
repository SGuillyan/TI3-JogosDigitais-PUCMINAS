using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LookDev;
using UnityEngine.EventSystems;

public class ItemSelectionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // Duration for the scaling animation (in seconds).
    [SerializeField] private float moveTime = 0.1f;

    // Scale factor for the item when selected, adjustable in the inspector (between 0 and 2).
    [Range(0f, 2f), SerializeField] private float scaleAmount = 1.1f;

    // Variable to store the original scale of the item.
    private Vector3 startScale;

    // Unity method called when the script instance is being loaded.
    private void Start()
    {
        // Capture the initial scale of the object when the game starts.
        startScale = transform.localScale;
    }

    // Coroutine to handle the scaling effect of the item.
    private IEnumerator ScaleItem(bool startingAnimation)
    {
        Vector3 endScale; // Variable to hold the target scale.
        float elapsedTime = 0f; // Track the time elapsed during the scaling.

        // Loop to smoothly interpolate the scaling over the specified duration.
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime; // Increment the elapsed time.

            // Determine the target scale based on whether the animation is starting or ending.
            if (startingAnimation)
            {
                // If starting the animation, scale up the item.
                endScale = startScale * scaleAmount;
            }
            else
            {
                // If ending the animation, return to the original scale.
                endScale = startScale;
            }

            // Interpolate the current scale towards the target scale.
            Vector3 lerpedScale = Vector3.Lerp(transform.localScale, endScale, (elapsedTime / moveTime));

            // Apply the interpolated scale to the object.
            transform.localScale = lerpedScale;

            // Wait for the next frame before continuing the loop.
            yield return null;
        }

    }

    // Method called when the pointer enters the object's collider area.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Set the selected object in the event data to this game object.
        eventData.selectedObject = gameObject;
    }

    // Method called when the pointer exits the object's collider area.
    public void OnPointerExit(PointerEventData eventData)
    {
        // Clear the selected object in the event data.
        eventData.selectedObject = null;
    }

    // Method called when the object is selected (e.g., clicked or highlighted).
    public void OnSelect(BaseEventData eventData)
    {
        // Start the scaling up animation.
        StartCoroutine(ScaleItem(true));
    }

    // Method called when the object is deselected (e.g., clicked away from).
    public void OnDeselect(BaseEventData eventData)
    {
        // Start the scaling down animation.
        StartCoroutine(ScaleItem(false));
    }
}
