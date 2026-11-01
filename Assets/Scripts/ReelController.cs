
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReelController : MonoBehaviour
{
    [Header("Symbol Settings")]
    [SerializeField] private Sprite[] symbolSprites;

    [Header("Spin Settings")]
    [SerializeField] private float spinSpeed = 1000f;
    [SerializeField] private bool startOnPlay = false;

    private RectTransform reelRect;
    private readonly List<RectTransform> symbolObjects =
        new List<RectTransform>();

    private float reelHeight;
    private float symbolHeight;

    private bool isSpinning;
    private bool isStopping;

    private void Start()
    {
        reelRect = GetComponent<RectTransform>();

        if (symbolSprites == null || symbolSprites.Length != 4)
        {
            Debug.LogError("Each reel must have exactly 4 symbols!", this);
            return;
        }

        CreateSymbols();

        if (startOnPlay)
        {
            StartSpinning();
        }
    }

    private void CreateSymbols()
    {
        reelHeight = reelRect.rect.height;
        symbolHeight = reelHeight / 3f;

        // Create a shuffled copy of the symbol array.
        List<Sprite> shuffledSprites =
            new List<Sprite>(symbolSprites);

        for (int i = shuffledSprites.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = shuffledSprites[i];
            shuffledSprites[i] = shuffledSprites[randomIndex];
            shuffledSprites[randomIndex] = temp;
        }

        // Create four symbol images.
        for (int i = 0; i < shuffledSprites.Count; i++)
        {
            GameObject symbolObject = new GameObject(
                "Symbol_" + (i + 1),
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

            symbolObject.transform.SetParent(transform, false);

            RectTransform symbolRect =
                symbolObject.GetComponent<RectTransform>();

            symbolRect.anchorMin = new Vector2(0.5f, 0.5f);
            symbolRect.anchorMax = new Vector2(0.5f, 0.5f);
            symbolRect.pivot = new Vector2(0.5f, 0.5f);

            symbolRect.sizeDelta = new Vector2(
                reelRect.rect.width,
                symbolHeight
            );

            float yPosition = reelHeight / 2f
                - symbolHeight / 2f
                - (i * symbolHeight);

            symbolRect.anchoredPosition =
                new Vector2(0, yPosition);

            Image symbolImage =
                symbolObject.GetComponent<Image>();

            symbolImage.sprite = shuffledSprites[i];
            symbolImage.preserveAspect = true;
            symbolImage.raycastTarget = false;

            symbolObjects.Add(symbolRect);
        }
    }

    private void Update()
    {
        if (!isSpinning || isStopping)
            return;

        MoveSymbolsBy(spinSpeed * Time.deltaTime);
    }

    // Moves all symbols down and recycles those
    // that move outside the bottom of the reel.
    private void MoveSymbolsBy(float distance)
    {
        float recycleDistance = symbolHeight * 4f;

        float lowerBoundary =
            -reelHeight / 2f - symbolHeight / 2f;

        foreach (RectTransform symbol in symbolObjects)
        {
            Vector2 position = symbol.anchoredPosition;

            position.y -= distance;

            while (position.y <= lowerBoundary)
            {
                position.y += recycleDistance;
            }

            symbol.anchoredPosition = position;
        }
    }

    public void StartSpinning()
    {
        if (symbolObjects.Count == 0 || isSpinning || isStopping)
            return;

        isSpinning = true;
    }

    // Selects a random symbol and smoothly stops
    // the reel with that symbol in the middle row.
    public void StopAtRandomSymbol(float stopDuration)
    {
        if (!isSpinning || isStopping)
            return;

        StartCoroutine(StopReelRoutine(stopDuration));
    }

    private IEnumerator StopReelRoutine(float stopDuration)
    {
        isStopping = true;

        // Randomly select which symbol should appear
        // in the middle row.
        int randomIndex = Random.Range(0, symbolObjects.Count);

        RectTransform targetSymbol = symbolObjects[randomIndex];

        float recycleDistance = symbolHeight * 4f;

        // Calculate the distance needed to align
        // the selected symbol with the middle row.
        float targetDistance =
            Mathf.Repeat(targetSymbol.anchoredPosition.y,
            recycleDistance);

        // Continue spinning for extra cycles before stopping.
        targetDistance += recycleDistance * 2f;

        float elapsedTime = 0f;
        float previousDistance = 0f;

        // Smoothly decelerate using an ease-out curve.
        while (elapsedTime < stopDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(
                elapsedTime / stopDuration
            );

            float easedProgress =
                1f - Mathf.Pow(1f - progress, 3f);

            float currentDistance =
                targetDistance * easedProgress;

            float frameDistance =
                currentDistance - previousDistance;

            MoveSymbolsBy(frameDistance);

            previousDistance = currentDistance;

            yield return null;
        }

        // Apply any remaining movement.
        MoveSymbolsBy(targetDistance - previousDistance);

        // Correct tiny floating-point alignment errors.
        float correction = targetSymbol.anchoredPosition.y;

        foreach (RectTransform symbol in symbolObjects)
        {
            Vector2 position = symbol.anchoredPosition;
            position.y -= correction;
            symbol.anchoredPosition = position;
        }

        isSpinning = false;
        isStopping = false;

        Debug.Log("Reel stopped. Middle symbol: " +
            targetSymbol.GetComponent<Image>().sprite.name);
    }


    public Sprite GetMiddleSymbol()
    {
        if (symbolObjects.Count == 0)
            return null;

        RectTransform middleSymbol = null;
        float closestDistance = float.MaxValue;

        // Find the symbol whose center is closest to Y = 0.
        foreach (RectTransform symbol in symbolObjects)
        {
            float distance = Mathf.Abs(symbol.anchoredPosition.y);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                middleSymbol = symbol;
            }
        }

        if (middleSymbol == null)
            return null;

        return middleSymbol.GetComponent<Image>().sprite;
    }

}
