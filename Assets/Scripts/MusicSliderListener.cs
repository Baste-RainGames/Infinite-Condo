using UnityEngine;
using UnityEngine.UI;

public class MusicSliderListener : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = MusicSystem.MusicVolume;
        slider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float volume)
    {
        MusicSystem.MusicVolume = volume;
    }
}
