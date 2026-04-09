using UnityEngine;
using UnityEngine.UI;

public class SliderMusic : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            slider.value = PlayerPrefs.GetFloat("Volume");
        }

        slider.onValueChanged.AddListener(delegate { CambiaVolume(); });
    }

    public void CambiaVolume()
    {
        float nuovoVolume = slider.value;
        FindFirstObjectByType<GestoreMusica>().ImpostaVolume(nuovoVolume);
    }
}

