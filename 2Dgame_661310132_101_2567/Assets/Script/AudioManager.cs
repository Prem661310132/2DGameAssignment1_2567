using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    public AudioClip gamebg;
    public AudioClip creditbg;
    public AudioClip winbg;
    public AudioClip gameoverbg;
    public AudioClip mainmenubg;

    public AudioClip playerhurt;
    public AudioClip playerdie;
    public AudioClip playerattack;
    public AudioClip playerjump;
    public AudioClip playerlanding;

    private void Start()
    {
        
        SceneManager.sceneLoaded += OnSceneLoaded;

      
        PlayMusicForCurrentScene();
    }

    private void OnDestroy()
    {
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
 
        string sceneName = SceneManager.GetActiveScene().name;

        musicSource.Stop();

      
        switch (sceneName)
        {
            case "SampleScene":
                musicSource.clip = gamebg;
                break;
            case "Credit":
                musicSource.clip = creditbg;
                break;
            case "Ending":
                musicSource.clip = winbg;
                break;
            case "GameOver":
                musicSource.clip = gameoverbg;
                break;
            case "MainMenu":
                musicSource.clip = mainmenubg;
                break;
            default:
                musicSource.clip = null;
                break;
        }

        if (musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
