/*
 * =============<< ********* >>=============
 * Author       : Oriel Fernandes
 * Email        : Fernandesorielilled@gmail.com
 * Created Date : 25 / 06 / 2026
 * Title        : UIManagerScript.
 * Description  : Control de UI.
 * =============<< ********* >>=============
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerScript : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private HealthManager playerHealth;
    [SerializeField] private PlayerMov player;

    [Header("Vida")]
    [SerializeField] private Image healthFill;

    [Header("Pociones")]
    [SerializeField] private TMP_Text potionsText;

    [Header("Hachas")]
    [SerializeField] private TMP_Text axesText;

    private void Update()
    {
        UpdateHealth();
        UpdatePotions();
        UpdateAxes();
    }

    private void UpdateHealth()
    {
        healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, playerHealth.GetHealthPercent(), 5f * Time.deltaTime) ;
    }

    private void UpdatePotions()
    {
        potionsText.text = "X" + player.currentPotions.ToString();
    }

    private void UpdateAxes()
    {
        axesText.text = "X" + player.currentAxes.ToString();
    }
}
