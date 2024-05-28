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
            if (moneyNeeded == 0)
            {
                if (sceneToLoad == "ExitGame")
                {
                    Application.Quit();
                }
                SceneManager.LoadScene(sceneToLoad);
            }
            if (objectCounter.totalMoney >= moneyNeeded)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}
