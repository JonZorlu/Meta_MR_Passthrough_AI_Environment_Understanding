using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.Events;

public class ImageGalleryUI : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent<int> onImageChanged;
    [SerializeField] private UnityEvent<string> onLimitReached;

    [Header("Gallery Settings")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.3f;

    [Header("Selection Animation Settings")]
    [SerializeField] private float pullUpDistance = 0.2f;
    [SerializeField] private float pullUpScaleMultiplier = 1.25f;
    [SerializeField] private float pullUpDuration = 0.3f;

    [SerializeField] private Image leftImage;
    [SerializeField] private Image centerImage;
    [SerializeField] private Image rightImage;

    private List<Sprite> imageList = new();
    private OVRMicrogestureEventSource ovrMicrogestureEventSource;
    private int currentIndex = 0;
    private bool isCenterEnlarged = false;
    private Vector3 originalCenterScale;
    private Vector3 originalCenterPosition;
    private Coroutine fadeCoroutine;
    private Coroutine pullUpCoroutine;

    // Public properties for event access
    public UnityEvent<int> OnImageChanged => onImageChanged;
    public UnityEvent<string> OnLimitReached => onLimitReached;
    public int CurrentImageIndex => currentIndex;
    public int TotalImageCount => imageList.Count;

    void Start()
    {
        imageList = Resources.LoadAll<Sprite>("GalleryImages")
            .ToList();

        if (imageList.Count < 3)
        {
            Debug.LogError("You need at least 3 images in the list.");
            return;
        }

        ovrMicrogestureEventSource = GetComponent<OVRMicrogestureEventSource>();
        ovrMicrogestureEventSource.GestureRecognizedEvent.AddListener(OnMicrogestureRecognized);

        originalCenterScale = centerImage.rectTransform.localScale;
        originalCenterPosition = centerImage.rectTransform.localPosition;

        UpdateGallery();
    }

    void OnMicrogestureRecognized(OVRHand.MicrogestureType microgestureType)
    {
        if (microgestureType == OVRHand.MicrogestureType.SwipeLeft)
        {
            NavigateToPreviousImage();
        }

        if (microgestureType == OVRHand.MicrogestureType.SwipeRight)
        {
            NavigateToNextImage();
        }

        if (microgestureType == OVRHand.MicrogestureType.SwipeForward)
        {
            SelectCenterImage();
        }

        if (microgestureType == OVRHand.MicrogestureType.SwipeBackward)
        {
            RestoreCenterImage();
        }
    }

    void UpdateGallery()
    {
        int leftIndex = (currentIndex - 1 + imageList.Count) % imageList.Count;
        int rightIndex = (currentIndex + 1) % imageList.Count;

        leftImage.sprite = imageList[leftIndex];
        rightImage.sprite = imageList[rightIndex];

        // Fade out current center image, then fade in new one
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeCenterImage(imageList[currentIndex]));

        // Ensure center image is reset when navigating
        ResetCenterImageTransform();
    }

    private IEnumerator FadeCenterImage(Sprite newSprite)
    {
        // Fade out current image
        Color currentColor = centerImage.color;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            centerImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }

        // Change the sprite
        centerImage.sprite = newSprite;

        // Fade in new image
        elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            centerImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }

        // Ensure full opacity
        centerImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
    }

    private void ResetCenterImageTransform()
    {
        if (pullUpCoroutine != null) StopCoroutine(pullUpCoroutine);
        isCenterEnlarged = false;
        centerImage.rectTransform.localPosition = originalCenterPosition;
        centerImage.rectTransform.localScale = originalCenterScale;
        centerImage.transform.SetSiblingIndex(1); // Restore original place between Left and Right
    }

    private void NavigateToPreviousImage()
    {
        currentIndex = (currentIndex - 1 + imageList.Count) % imageList.Count;
        UpdateGallery();
        onImageChanged?.Invoke(currentIndex);
    }

    private void NavigateToNextImage()
    {
        currentIndex = (currentIndex + 1) % imageList.Count;
        UpdateGallery();
        onImageChanged?.Invoke(currentIndex);
    }

    private void SelectCenterImage()
    {
        if (isCenterEnlarged) return;
        isCenterEnlarged = true;

        centerImage.transform.SetAsLastSibling(); // Ensure it renders on top

        if (pullUpCoroutine != null) StopCoroutine(pullUpCoroutine);
        pullUpCoroutine = StartCoroutine(LerpCenterImageTransform(
            originalCenterPosition + Vector3.up * pullUpDistance,
            originalCenterScale * pullUpScaleMultiplier,
            false
        ));
    }

    private void RestoreCenterImage()
    {
        if (!isCenterEnlarged) return;
        isCenterEnlarged = false;

        if (pullUpCoroutine != null) StopCoroutine(pullUpCoroutine);
        pullUpCoroutine = StartCoroutine(LerpCenterImageTransform(
            originalCenterPosition,
            originalCenterScale,
            true
        ));
    }

    private IEnumerator LerpCenterImageTransform(Vector3 targetPosition, Vector3 targetScale, bool restoreSiblingIndex)
    {
        Vector3 startPosition = centerImage.rectTransform.localPosition;
        Vector3 startScale = centerImage.rectTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < pullUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / pullUpDuration);
            centerImage.rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            centerImage.rectTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        centerImage.rectTransform.localPosition = targetPosition;
        centerImage.rectTransform.localScale = targetScale;

        if (restoreSiblingIndex)
        {
            centerImage.transform.SetSiblingIndex(1);
        }
    }
}