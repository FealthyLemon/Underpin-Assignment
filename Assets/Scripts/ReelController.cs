
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
    private readonly List<RectTransform> symbolObjects = new List<RectTransform>();

    private float reelHeight;
    private float symbolHeight;
    private bool isSpinning;

    private void Start()
    {
        reelRect = GetComponent<RectTransform>();

        if (symbolSprites.Length != 4)
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
        List<Sprite> shuffledSprites = new List<Sprite>(symbolSprites);

        for (int i = shuffledSprites.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = shuffledSprites[i];
            shuffledSprites[i] = shuffledSprites[randomIndex];
            shuffledSprites[randomIndex] = temp;
        }

        // Create four symbol images and arrange them vertically.
        for (int i = 0; i < shuffledSprites.Count; i++)
        {
            GameObject symbolObject = new GameObject(
                "Symbol_" + (i + 1),
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

            symbolObject.transform.SetParent(transform, false);

            RectTransform symbolRect = symbolObject.GetComponent<RectTransform>();

            symbolRect.anchorMin = new Vector2(0.5f, 0.5f);
            symbolRect.anchorMax = new Vector2(0.5f, 0.5f);
            symbolRect.pivot = new Vector2(0.5f, 0.5f);

            symbolRect.sizeDelta = new Vector2(
                reelRect.rect.width,
                symbolHeight
            );

            // Position symbols from top to bottom.
            float yPosition = reelHeight / 2f
                - symbolHeight / 2f
                - (i * symbolHeight);

            symbolRect.anchoredPosition = new Vector2(0, yPosition);

            Image symbolImage = symbolObject.GetComponent<Image>();
            symbolImage.sprite = shuffledSprites[i];
            symbolImage.preserveAspect = true;
            symbolImage.raycastTarget = false;

            symbolObjects.Add(symbolRect);
        }
    }

    private void Update()
    {
        if (!isSpinning)
            return;

        MoveSymbols();
    }

    private void MoveSymbols()
    {
        foreach (RectTransform symbol in symbolObjects)
        {
            symbol.anchoredPosition += Vector2.down
                * spinSpeed * Time.deltaTime;

            // Recycle a symbol after it moves below the reel.
            if (symbol.anchoredPosition.y <= -reelHeight / 2f - symbolHeight / 2f)
            {
                symbol.anchoredPosition += Vector2.up * (symbolHeight * 4f);
            }
        }
    }

    public void StartSpinning()
    {
        if (symbolObjects.Count == 0)
            return;

        isSpinning = true;
    }

    public void StopSpinning()
    {
        isSpinning = false;
    }
}
