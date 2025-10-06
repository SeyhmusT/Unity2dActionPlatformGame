using UnityEngine;
using UnityEngine.UI;

public class HealthbarControl : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    public Gradient colGradient;
    private Image fillImage;


    private void Awake()
    {
        fillImage = healthSlider.fillRect.GetComponent<Image>();
    }
    public void SetSliderValue(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        fillImage.color = colGradient.Evaluate(healthSlider.normalizedValue);
    }
}
