using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIManager : MonoBehaviour
{
    public static DeathUIManager Instance;

    [SerializeField] private GameObject deathPanel;

    [SerializeField] private Image blackImage;
    [SerializeField] private Image deathImage;
    [SerializeField] private Image retryButtonImage;
    [SerializeField] private Button retryButton;

    [SerializeField] private float backgroundFadeTime = 1f;
    [SerializeField] private float textFadeTime = 0.6f;
    [SerializeField] private float buttonFadeTime = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        deathPanel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        deathPanel.SetActive(true);

        retryButton.interactable = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(DeathSequence());
    }
    public void HideDeathScreen()
    {
        
        SetAlpha(blackImage, 0);
        SetAlpha(deathImage, 0);
        SetAlpha(retryButtonImage, 0);

        retryButton.interactable = false;

        deathPanel.SetActive(false);
    }

    IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(
            FadeImage(blackImage, backgroundFadeTime)
        );

        yield return new WaitForSeconds(0.3f);

        yield return StartCoroutine(
            FadeImage(deathImage, textFadeTime)
        );

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(
            FadeImage(retryButtonImage, buttonFadeTime)
        );


        retryButton.interactable = true;
    }

    IEnumerator FadeImage(Image image, float duration)
    {
        Color c = image.color;

        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;

            c.a = Mathf.Lerp(0, 1, t / duration);

            image.color = c;

            yield return null;
        }

        c.a = 1;

        image.color = c;
    }
    private void SetAlpha(Graphic graphic, float alpha)
    {
        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
    }

}
