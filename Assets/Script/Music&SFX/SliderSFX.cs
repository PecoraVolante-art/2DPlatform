using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderSFX : MonoBehaviour, IPointerUpHandler
{
    public Slider slider;
    public AudioClip previewClip;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        slider.onValueChanged.AddListener(CambiaVolume);
    }

    void CambiaVolume(float valore)
    {
        if (GestioneSFX.Instance != null)
            GestioneSFX.Instance.SetVolume(valore);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GestioneSFX.Instance != null && previewClip != null)
        {
            GestioneSFX.Instance.PlaySFX(previewClip);
        }
    }
}