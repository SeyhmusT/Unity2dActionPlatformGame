using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image reloadImageFill;

    public void ActivateReloadBar()
    {
        reloadImageFill.fillAmount = 0;
        canvasGroup.alpha = 1;
    }

    public void DeactivateReloadBar()
    {
        canvasGroup.alpha = 0;
    }

    public void UpdateReloadBar(float elapsedTime, float reloadTime)
    {
        reloadImageFill.fillAmount = Mathf.Clamp01(elapsedTime / reloadTime);
    }
}
