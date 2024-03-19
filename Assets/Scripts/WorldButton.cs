using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldButton : MonoBehaviour
{
    //��ư Ŭ�� �� ���� �̺�Ʈ�� ����. 
    string slime = "SlimeScene";

    public void GoSlimeScene() //���� �븻 ��� �̰ɷ� �̺�Ʈ �޾��ֱ�. 
    {
        Debug.Log("����ȯ Ŭ����");
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene("SlimeScene"); //�˾��� ������ �� �ε��� ���۵Ǵ� ����. 

    }

    public void RetryScene()
    {
        
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene(slime);
    }

    public void WorldMapScene()
    {
        //���ӸŴ���? �� �÷��̾��� ������ ��ġ �����صΰ� --> �� �Ѿ���� �������ֱ�.
        // ���� ���⼭ �� ��ġ�� �ε� �� ������.
        Manager.UI.ClosePopUpUI();
        Manager.Scene.LoadScene("WorldMapScene");
    }
    public void OnClickExit()
    {
        Application.Quit(); //���� �����ư 
        Debug.Log("�����ư Ŭ��");
    }
}
