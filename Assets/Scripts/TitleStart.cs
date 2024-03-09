using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleStart : MonoBehaviour
{
    Text flashingText;
    
    void Start()
    {
        flashingText=GetComponent<Text>();
        StartCoroutine(BlinkText());
    }

    public IEnumerator BlinkText()
    {
        while(true)
        {
            flashingText.text = "";
            yield return new WaitForSeconds(0.5f);
            flashingText.text = "Press Any Key To Start";
            yield return new WaitForSeconds(0.8f);
        }
    }



}
