using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro.EditorUtilities;
using TMPro;


public class MainMenu : MonoBehaviour
{

    [SerializeField]
    public GameObject inputObject;

    [SerializeField]
    public GameObject errObj;

    private int noDisks;



    public void PlayHanoi()
    {
        noDisks = int.Parse(inputObject.GetComponent<TMP_InputField>().text);
        if(noDisks > 0)
        {
            PlayerPrefs.SetInt("no_disks", noDisks);
            SceneManager.LoadScene("Main Scene");
        }
        else
        {
            TMP_Text errText = errObj.GetComponent<TMP_Text>();
            errText.text = "Please enter a non-negative integer.";
            // To do: display text only for 5 seconds..?
        }

    }
}
