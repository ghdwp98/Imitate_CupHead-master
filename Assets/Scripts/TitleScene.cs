using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TitleScene : BaseScene
{
   
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            Manager.Scene.LoadScene("TutorialScene");
        }
    }


    public override IEnumerator LoadingRoutine()
    {
       
        yield return null;
    }

}
