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

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }



    private DiskHandler diskHandler;

    // todo - make diskHandler that handles disks-prefabs and use the gameHandler to run them.

    void Start()
    {
        diskHandler = diskHandlerObject.GetComponent<DiskHandler>();
    }


}
