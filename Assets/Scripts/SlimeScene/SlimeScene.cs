using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScene : BaseScene
{
    [SerializeField] GameObject playerPos;
    [SerializeField] SlimeBoss slime;

    public override IEnumerator LoadingRoutine()
    {
        // �ð� ���� �ִ� ���¿��� �� ���� �ε� ��ƾ�� ����ȴ�.
        // ���⼭ ������ ��Ʈ�� �� ����������. 

        //�˾ƿ��̳� ��Ʈ�δ� ui���� ������ ������ϳ�??? ������ ������ �ؾ��ϳ� ��εǳ� 

        yield return null;
    }

    
}
