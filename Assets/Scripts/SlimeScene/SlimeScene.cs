using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScene : BaseScene
{
    [SerializeField] Vector2 playerPos;
    [SerializeField] SlimeBoss slime;

    public override IEnumerator LoadingRoutine()
    {

        playerPos = Manager.Scene.playerPos; //의미가 있나? 

        yield return null;
    }

    
}
