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
        
        worldPos=worldPlayer.transform.position; //�÷��̾��� ��ġ ����. ���̽����� �����̿�
    }

    public override IEnumerator LoadingRoutine() //�̰Ŵ� ��������� ���� ����. 
    {
        
        worldMapPos = Manager.Scene.playerPos;


        yield return null;
    }

    
}
