using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField]
    private ObjectCounter objectCounter;
    [SerializeField]
    private int moneyNeeded = 150;

    [SerializeField]
    private string sceneToLoad;

    [SerializeField] private TextMeshProUGUI textQuantity;
    
    private void Start()
    {
        textQuantity.text = moneyNeeded.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (moneyNeeded < 1)
            {
                if (sceneToLoad == "ExitGame")
                {
                    #if UNITY_STANDALONE
                        Application.Quit();
                    #endif
                    #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                    #endif
                }
                SceneManager.LoadScene(sceneToLoad);
            }
            else if (objectCounter.totalMoney >= moneyNeeded)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        Debug.Log($"Total Money: {objectCounter.totalMoney}, Needed Money: {moneyNeeded}");
    }
}
