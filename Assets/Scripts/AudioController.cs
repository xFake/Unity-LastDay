using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    List<AudioClip> musicSamples;

    [SerializeField]
    TextMeshProUGUI musicSliderValue;

    [SerializeField]
    Slider musicSlider;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        musicSlider.value = PlayerPrefs.GetFloat("Music");
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = musicSamples[Random.Range(0, musicSamples.Count)];
            audioSource.Play();
        }

        audioSource.volume = musicSlider.value / 100;
        musicSliderValue.text = "Music " + musicSlider.value + "%";
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }


}
