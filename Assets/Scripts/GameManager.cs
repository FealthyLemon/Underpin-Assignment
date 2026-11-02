
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int startingCoins = 100;
    [SerializeField] private int spinCost = 10;
    [SerializeField] private int winReward = 50;

    [Header("UI References")]
    [SerializeField] private TMP_Text balanceText;

    [Header("Result Panel")]
    [SerializeField] private GameObject resultCanvas;
    [SerializeField] private TMP_Text resultPanelText;
    [SerializeField] private float resultDisplayDuration = 2f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    private int currentCoins;
    private bool isResultPanelActive;

    private void Start()
    {
        currentCoins = startingCoins;

        resultCanvas.SetActive(false);

        UpdateBalanceUI();
    }

    public bool TryPayForSpin()
    {
        // Prevent spinning while the result panel is visible.
        if (isResultPanelActive)
            return false;

        if (currentCoins < spinCost)
        {
            Debug.Log("Not enough coins!");
            return false;
        }

        currentCoins -= spinCost;

        UpdateBalanceUI();

        return true;
    }

    public void EvaluateResult(ReelController[] reels)
    {
        if (reels == null || reels.Length != 3)
        {
            Debug.LogError("Exactly three reels are required!");
            return;
        }

        Sprite firstSymbol = reels[0].GetMiddleSymbol();
        Sprite secondSymbol = reels[1].GetMiddleSymbol();
        Sprite thirdSymbol = reels[2].GetMiddleSymbol();

        if (firstSymbol == null ||
            secondSymbol == null ||
            thirdSymbol == null)
        {
            Debug.LogError("Could not retrieve middle-row symbols!");
            return;
        }

        bool hasWon =
            firstSymbol == secondSymbol &&
            secondSymbol == thirdSymbol;

        if (hasWon)
        {
            currentCoins += winReward;

            UpdateBalanceUI();

            Debug.Log("Player won " + winReward + " coins!");

            audioSource.PlayOneShot(winSound);
            StartCoroutine(ShowResultPanel("YOU WIN!", false));
        }
        else
        {
            Debug.Log("No winning combination.");

            // Only show the losing panel when the balance reaches zero.
            if (currentCoins <= 0)
            {
                audioSource.PlayOneShot(loseSound);
                StartCoroutine(ShowResultPanel("YOU LOSE!", true));
            }
        }
    }

    private IEnumerator ShowResultPanel(string message, bool resetCoins)
    {
        isResultPanelActive = true;

        resultPanelText.text = message;
        resultCanvas.SetActive(true);

        yield return new WaitForSeconds(resultDisplayDuration);

        resultCanvas.SetActive(false);

        if (resetCoins)
        {
            currentCoins = startingCoins;

            UpdateBalanceUI();

            Debug.Log("Coins reset to " + startingCoins);
        }

        isResultPanelActive = false;
    }

    private void UpdateBalanceUI()
    {
        balanceText.text = "COINS: " + currentCoins;
    }
}
