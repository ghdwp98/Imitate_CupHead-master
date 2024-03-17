using System.Collections;
using UnityEngine;

public class BoxTransform : MonoBehaviour
{
    [SerializeField]GameObject slime;
    Vector2 dir;
    [SerializeField] Vector2 lastVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Monster")
        {
            if(this.gameObject.name=="Mark1")
            {
                gameObject.transform.Translate(new Vector2(-0.5f, 0.5f));
            }
            else if(this.gameObject.name=="Mark2")
            {
                gameObject.transform.Translate(new Vector2(0, 0.6f));

            }
            else if(this.gameObject.name=="Mark3")
            {
                Debug.Log("¸¶Å©3");
                gameObject.transform.Translate(new Vector2(0.5f, 0.5f));


            }

        }

    }

    public void Update()
    {
        lastVelocity=slime.GetComponent<Rigidbody2D>().velocity;
    }


    IEnumerator TargetCoroutine()
    {

        dir = (slime.transform.position - transform.position).normalized;

        yield return new WaitForSecondsRealtime(0.2f);


    }
}
