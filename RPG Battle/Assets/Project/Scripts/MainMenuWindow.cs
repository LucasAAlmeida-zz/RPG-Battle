using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuWindow : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip buttonClickedAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPlayButtonClicked()
    {
        audioSource.PlayOneShot(buttonClickedAudioClip);
    }
}
