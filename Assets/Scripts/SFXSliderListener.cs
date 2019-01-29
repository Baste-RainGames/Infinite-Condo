using UnityEngine;
using UnityEngine.UI;

public class SFXSliderListener : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = SoundSystem.SfxVolume;
        slider.onValueChanged.AddListener(UpdateVolume);
    }

    private void UpdateVolume(float volume)
    {
        SoundSystem.SfxVolume = volume;
    }
}
