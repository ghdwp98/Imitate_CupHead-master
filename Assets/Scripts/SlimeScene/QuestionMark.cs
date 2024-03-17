using System.Collections;
using UnityEngine;

public class QuestionMark : IParry
{

    [SerializeField] GameObject mark1;
    [SerializeField] GameObject mark2;
    [SerializeField] GameObject mark3;

    Animator animator;

    //인스탄티 에이트 된다. 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        if (transform.parent.name == "Mark1")
        {
            animator.Play("QuestionMark1");
        }
        else if (transform.parent.name == "Mark2")
        {
            animator.Play("QuestionMark2");
        }
        else if (transform.parent.name == "Mark3")
        {
            animator.Play("QuestionMark3");
        }
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) //슬라임이 커져서 부딪히면 밀려나야함. 
    {
        
    }


    public override void Parried()
    {
        StartCoroutine(parryCoroutine());
        Destroy(gameObject,0.2f);
       
    }
    IEnumerator parryCoroutine()
    {

        if (isparrying == false)
        {
            Time.timeScale = 0.5f;
            isparrying = true;
            yield return new WaitForSecondsRealtime(0.2f);
            Time.timeScale = 1.0f;
            isparrying = false;

        }
    }

}
