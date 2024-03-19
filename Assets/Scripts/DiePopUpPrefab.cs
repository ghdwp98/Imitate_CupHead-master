using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiePopUpPrefab : MonoBehaviour
{
    [SerializeField] Button retry;
    [SerializeField] Button worldMap;
    [SerializeField] Button quitGame;


    void Start()
    {
        retry.Select();
    }

    void Update()
    {
        
    }
}
