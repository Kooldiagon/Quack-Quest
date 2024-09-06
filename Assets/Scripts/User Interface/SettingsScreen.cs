using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField]
    private SliderPlus musicSlider, soundSlider;

    private void OnEnable()
    {
        musicSlider.SetValue(GameManager.Instance.SaveData.MusicVolume);
        soundSlider.SetValue(GameManager.Instance.SaveData.SoundVolume);
    }

    public void MusicAction()
    {
        AudioHandler.Instance.AdjustMusicVolume((int)musicSlider.Slider.value);
    }

    public void SoundAction()
    {
        AudioHandler.Instance.AdjustSoundVolume((int)soundSlider.Slider.value);
    }

    public void AcceptChanges(GameObject screen)
    {
        GameManager.Instance.SaveData.MusicVolume = (int)musicSlider.Slider.value;
        GameManager.Instance.SaveData.SoundVolume = (int)soundSlider.Slider.value;
        UnityServicesHandler.Instance.CloudSave.Save();
        UIHandler.Instance.ChangeScreen(screen);
    }

    public void DeclineChanges(GameObject screen)
    {
        AudioHandler.Instance.AdjustMusicVolume(GameManager.Instance.SaveData.MusicVolume);
        AudioHandler.Instance.AdjustSoundVolume(GameManager.Instance.SaveData.SoundVolume);
        UIHandler.Instance.ChangeScreen(screen);
    }
}
