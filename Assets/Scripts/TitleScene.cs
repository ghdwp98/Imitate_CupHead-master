using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class TitleScene : BaseScene
{
   
    private void Update()
    {
        if(Input.anyKey)
        {
            Manager.Scene.LoadScene("TutorialScene");
        }
    }


    public override IEnumerator LoadingRoutine()
    {
       
        yield return null;
    }

}
