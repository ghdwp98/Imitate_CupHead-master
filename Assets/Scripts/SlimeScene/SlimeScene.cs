using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScene : BaseScene
{
    [SerializeField] GameObject playerPos;
    [SerializeField] SlimeBoss slime;

    public override IEnumerator LoadingRoutine()
    {
        // 시간 멈춰 있는 상태에서 이 씬의 로딩 루틴이 진행된다.
        // 여기서 보스전 인트로 씬 진행해주자. 

        //넉아웃이나 인트로는 ui에서 관리를 해줘야하나??? 씬에서 관리를 해야하나 고민되네 

        yield return null;
    }

    
}
