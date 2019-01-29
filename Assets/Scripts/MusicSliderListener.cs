using UnityEngine;
using UnityEngine.UI;

public class MusicSliderListener : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = SoundSystem.MusicVolume;
        slider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float volume)
    {
        SoundSystem.MusicVolume = volume;
    }
}
