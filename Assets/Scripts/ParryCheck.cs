using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCheck : MonoBehaviour
{
    public bool isParryed=false;
    Animator animator;
    [SerializeField]GameObject parryEffectPrefab;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking" ||
            collision.tag == "Parry"||collision.tag=="Monster")
        {
            return; //do nothing
        }

        isParryed = true;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Checking"
            || collision.tag == "Parry" || collision.tag == "Monster")
        {
            return; //do nothing
        }

        isParryed = false;

    }

    public void ParryAniPlay()
    {
        Instantiate(parryEffectPrefab,transform.position,transform.rotation);
        
    }


    private void Update()
    {
        
    }


}
