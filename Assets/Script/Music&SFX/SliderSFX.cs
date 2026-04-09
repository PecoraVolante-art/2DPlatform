using UnityEngine;
using UnityEngine.UI;

public class SliderSFX : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f); //porta la posizione dello slider alla posizione delle preference presa dalla GestoineSfx
        slider.onValueChanged.AddListener(CambiaVolume);
    }

    void CambiaVolume(float valore)
    {
        GestioneSFX.Instance.SetVolume(valore);
    }
}
