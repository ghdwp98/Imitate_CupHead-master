using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopEnterBossPrefab : MonoBehaviour
{
    [SerializeField] Button easyBtn;
    [SerializeField] Button normalBtn;


    void Start() //�������� ���ܳ��� 
    {
        easyBtn.Select();
    }

    void Update()
    {
        
    }
}
