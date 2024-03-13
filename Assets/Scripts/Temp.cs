using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    Text text;
    void Start()
    {
        string str = Manager.Game.getScore().ToString();
        text = GetComponent<Text>();
        text.text = $"x {str}";
    }

    void Update()
    {
        
    }
}
