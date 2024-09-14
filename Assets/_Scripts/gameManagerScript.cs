using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class gameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public Health player;
    public float transitionDuration = 2.0f;
    public CanvasGroup gameOverCanvasGroup;

    void Start()
    {
        // Ensure the CanvasGroup is initialized properly
        if (gameOverCanvasGroup == null)
        {
            gameOverCanvasGroup = gameOverUI.GetComponent<CanvasGroup>();

            // If CanvasGroup doesn't exist, add one
            if (gameOverCanvasGroup == null)
                gameOverCanvasGroup = gameOverUI.AddComponent<CanvasGroup>();
        }

        // Ensure the UI is initially hidden
        gameOverUI.SetActive(false);
        gameOverCanvasGroup.alpha = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && gameOverUI.activeSelf)
        {
            StartCoroutine(FadeOutAndRespawn());
        }
    }

    IEnumerator FadeOutAndRespawn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / transitionDuration);
            gameOverCanvasGroup.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the UI is fully hidden
        gameOverCanvasGroup.alpha = 0f;

        // Deactivate the UI
        gameOverUI.SetActive(false);

        // Trigger the respawn
        player.Respawn();
    }

    public void gameOver()
    {
        // Activate the UI initially
        gameOverUI.SetActive(true);

        // Start the fade-in coroutine
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / transitionDuration);
            gameOverCanvasGroup.alpha = alpha;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the UI is fully visible
        gameOverCanvasGroup.alpha = 1f;
    }
}
