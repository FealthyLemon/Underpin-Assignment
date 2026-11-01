
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LeverController : MonoBehaviour
{
    [Header("Lever References")]
    [SerializeField] private GameObject leverUp;
    [SerializeField] private GameObject leverDown;

    [Header("Reel References")]
    [SerializeField] private ReelController[] reels;

    [Header("Settings")]
    [SerializeField] private float leverDuration = 2f;

    private bool isLeverAnimating;

    private void Start()
    {
        // Set the initial lever state.
        leverUp.SetActive(true);
        leverDown.SetActive(false);
    }


    public void OnUpLeverPressed()
    {
        Debug.Log("LeverController received the click!");

        if (isLeverAnimating)
        {
            Debug.Log("Lever is already animating!");
            return;
        }

        StartCoroutine(ActivateLever());
    }


    private IEnumerator ActivateLever()
    {
        isLeverAnimating = true;

        // Switch lever visuals.
        leverUp.SetActive(false);
        leverDown.SetActive(true);

        // Start all three reels.
        foreach (ReelController reel in reels)
        {
            reel.StartSpinning();
        }

        // Keep the lower lever visible for 2 seconds.
        yield return new WaitForSeconds(leverDuration);

        // Return to the initial lever position.
        leverDown.SetActive(false);
        leverUp.SetActive(true);

        isLeverAnimating = false;
    }
}
