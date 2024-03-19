using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class WorldMapScene : BaseScene
{
    [SerializeField] GameObject worldPlayer;
    [SerializeField] public Vector2 worldMapPos;

    public void Update()
    {
        
        worldPos=worldPlayer.transform.position; //플레이어의 위치 저장. 베이스씬의 변수이용
    }

    public override IEnumerator LoadingRoutine() //이거는 월드맵으로 들어가는 씬임. 
    {
        
        worldMapPos = Manager.Scene.playerPos;


        yield return null;
    }

    
}
