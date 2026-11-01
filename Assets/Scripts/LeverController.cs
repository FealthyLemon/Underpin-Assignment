
using System.Collections;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [Header("Lever References")]
    [SerializeField] private GameObject leverUp;
    [SerializeField] private GameObject leverDown;

    [Header("Reel References")]
    [SerializeField] private ReelController[] reels;

    [Header("Lever Settings")]
    [SerializeField] private float leverDuration = 2f;

    [Header("Reel Stop Settings")]
    [SerializeField] private float initialSpinDuration = 2f;
    [SerializeField] private float stopAnimationDuration = 1f;
    [SerializeField] private float delayBetweenStops = 0.5f;

    [Header("Game Manager")]
    [SerializeField] private GameManager gameManager;

    private bool isLeverAnimating;
    private bool isSpinSequenceRunning;

    private void Start()
    {
        leverUp.SetActive(true);
        leverDown.SetActive(false);
    }

    public void OnUpLeverPressed()
    {
        if (isLeverAnimating || isSpinSequenceRunning)
            return;

        // Check and deduct the spin cost.
        if (!gameManager.TryPayForSpin())
            return;

        StartCoroutine(ActivateLever());
    }

    private IEnumerator ActivateLever()
    {
        isLeverAnimating = true;
        isSpinSequenceRunning = true;

        // Change lever visuals.
        leverUp.SetActive(false);
        leverDown.SetActive(true);

        // Start all reels together.
        foreach (ReelController reel in reels)
        {
            reel.StartSpinning();
        }

        // Start the independent reel stopping sequence.
        StartCoroutine(StopReelsSequentially());

        // Keep the lower lever visible for the specified duration.
        yield return new WaitForSeconds(leverDuration);

        leverDown.SetActive(false);
        leverUp.SetActive(true);

        isLeverAnimating = false;
    }

    private IEnumerator StopReelsSequentially()
    {
        // Let all reels spin before the first reel stops.
        yield return new WaitForSeconds(initialSpinDuration);

        for (int i = 0; i < reels.Length; i++)
        {
            // Stop the current reel.
            reels[i].StopAtRandomSymbol(stopAnimationDuration);

            // Wait until this reel has stopped before
            // moving to the next reel.
            yield return new WaitForSeconds(
                stopAnimationDuration + delayBetweenStops
            );
        }

        isSpinSequenceRunning = false;

        Debug.Log("All reels have stopped!");
        gameManager.EvaluateResult(reels);
    }
}
