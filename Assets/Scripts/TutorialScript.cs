using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTrigger : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI tutorialText;

    [Header("Tutorial Settings")]
    [TextArea(3, 5)]
    public string message = "Press WASD to move";

    [Tooltip("How long to show the message (0 = until player leaves)")]
    public float displayDuration = 0f;

    [Header("Fade Settings")]
    public bool useFadeEffect = true;
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    [Header("Options")]
    public bool showOnce = true;
    public bool destroyTriggerAfterUse = false;

    private bool hasTriggered = false;
    private bool isPlayerInside = false;
    private static Coroutine currentFadeCoroutine;
    private static TutorialTrigger activeTrigger;

    void Start()
    {
        // Make sure text is hidden at start
        if (tutorialText != null)
        {
            tutorialText.gameObject.SetActive(false);
            // Set alpha to 0 initially
            Color color = tutorialText.color;
            tutorialText.color = new Color(color.r, color.g, color.b, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if it's the player
        if (other.CompareTag("Player"))
        {
            // If showOnce is true and already triggered, do nothing
            if (showOnce && hasTriggered)
                return;

            isPlayerInside = true;

            // Cancel any hide invoke from previous trigger
            CancelInvoke("HideTutorialText");

            // If another trigger is active, immediately take over
            if (activeTrigger != null && activeTrigger != this)
            {
                activeTrigger.isPlayerInside = false;
                activeTrigger.CancelInvoke("HideTutorialText");
            }

            // Set this as the active trigger
            activeTrigger = this;

            // Show the tutorial text
            ShowTutorialText();
            hasTriggered = true;

            // If duration is set, hide after that time
            if (displayDuration > 0)
            {
                Invoke("HideTutorialText", displayDuration);
            }

            // Optionally destroy this trigger after use
            if (destroyTriggerAfterUse)
            {
                Destroy(gameObject, displayDuration + fadeOutDuration);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Hide text when player leaves (only if no duration set)
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            // Only hide if this is still the active trigger and no duration is set
            if (activeTrigger == this && displayDuration == 0)
            {
                // Small delay before hiding to prevent flicker when entering next trigger
                Invoke("CheckAndHide", 0.1f);
            }
        }
    }

    void CheckAndHide()
    {
        // Only hide if still not inside and still the active trigger
        if (!isPlayerInside && activeTrigger == this)
        {
            HideTutorialText();
        }
    }

    void ShowTutorialText()
    {
        if (tutorialText != null)
        {
            tutorialText.text = message;
            tutorialText.gameObject.SetActive(true);

            // Stop any existing fade from ANY trigger
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            if (useFadeEffect)
            {
                currentFadeCoroutine = StartCoroutine(FadeText(tutorialText.color.a, 1f, fadeInDuration));
            }
            else
            {
                // Instant show
                Color color = tutorialText.color;
                tutorialText.color = new Color(color.r, color.g, color.b, 1f);
            }
        }
        else
        {
            Debug.LogWarning("Tutorial Text not assigned in " + gameObject.name);
        }
    }

    void HideTutorialText()
    {
        // Only hide if this is still the active trigger
        if (activeTrigger != this)
            return;

        if (tutorialText != null)
        {
            // Stop any existing fade
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            if (useFadeEffect)
            {
                currentFadeCoroutine = StartCoroutine(FadeText(tutorialText.color.a, 0f, fadeOutDuration));
            }
            else
            {
                // Instant hide
                tutorialText.gameObject.SetActive(false);
            }

            // Clear active trigger
            activeTrigger = null;
        }
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color color = tutorialText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            tutorialText.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Set final alpha
        tutorialText.color = new Color(color.r, color.g, color.b, endAlpha);

        // Disable GameObject after fade out
        if (endAlpha == 0f)
        {
            tutorialText.gameObject.SetActive(false);
        }

        currentFadeCoroutine = null;
    }

    void OnDestroy()
    {
        // Clean up if this trigger is destroyed
        if (activeTrigger == this)
        {
            activeTrigger = null;
        }
    }
}