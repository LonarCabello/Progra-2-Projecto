using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Image fill;

    private HealthManager health;

    [SerializeField] private float lerpSpeed = 10f;


    private void Start()
    {
        health = GetComponentInParent<HealthManager>();
    }

    private float currentFill = 1;

    private void Update()
    {
        currentFill = Mathf.Lerp(
            currentFill,
            health.GetHealthPercent(),
            Time.deltaTime * lerpSpeed);

        fill.fillAmount = currentFill;


        if (currentFill <= 0.01f)
        {
            gameObject.SetActive(false);
        }
    }
}
