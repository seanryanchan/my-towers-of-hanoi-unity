using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject leftPegBase;

    public GameObject middlePegBase;

    public GameObject rightPegBase;

    public GameObject diskHandlerObject;

    public bool randomPlay = false; // checkbox for random play
    public AudioClip[] clips;



    private AudioSource audioSource;
    int clipOrder = 0; // for ordered playlist
    private DiskHandler diskHandler;

    // todo - make diskHandler that handles disks-prefabs and use the gameHandler to run them.

    void Start()
    {
        diskHandler = diskHandlerObject.GetComponent<DiskHandler>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            // if random play is selected
            if (randomPlay == true)
            {
                audioSource.clip = GetRandomClip();
                audioSource.Play();
                // if random play is not selected
            }
            else
            {
                audioSource.clip = GetNextClip();
                audioSource.Play();
            }
        }
    }

    // function to get a random clip
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    // function to get the next clip in order, then repeat from the beginning of the list.
    private AudioClip GetNextClip()
    {
        if (clipOrder >= clips.Length - 1)
        {
            clipOrder = 0;
        }
        else
        {
            clipOrder += 1;
        }
        return clips[clipOrder];
    }





    public void GoToMenu()
    {
        audioSource.Stop();
        audioSource.clip = null;
        SceneManager.LoadScene("Menu");

    }

}
