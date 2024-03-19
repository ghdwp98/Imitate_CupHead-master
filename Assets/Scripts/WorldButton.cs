using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldButton : MonoBehaviour
{
    //버튼 클릭 및 선택 이벤트들 실행. 
    string slime = "SlimeScene";

    public void GoSlimeScene() //이지 노말 모두 이걸로 이벤트 달아주기. 
    {
        Debug.Log("씬전환 클릭됨");
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene("SlimeScene"); //팝업이 닫혀야 씬 로딩이 시작되는 느낌. 

    }

    public void RetryScene()
    {
        
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene(slime);
    }

    public void WorldMapScene()
    {
        //게임매니저? 에 플레이어의 마지막 위치 저장해두고 --> 씬 넘어가기전 저장해주기.
        // 이후 여기서 그 위치로 로드 씬 해주자.
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene("WorldMapScene");
    }
    public void OnClickExit()
    {
        Application.Quit(); //게임 종료버튼 
        Debug.Log("종료버튼 클릭");
    }
}
