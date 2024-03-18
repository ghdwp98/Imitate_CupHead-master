using UnityEngine;

public class SlimePunch : MonoBehaviour
{
    [SerializeField] GameObject slime;

    void Start()
    {
        SpriteRenderer slimeSprite = slime.gameObject.GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        SpriteRenderer slimeSprite = slime.gameObject.GetComponent<SpriteRenderer>();

        if (slimeSprite.flipX == true)
        {
            if (gameObject.name == "PunchArm")
            {
                transform.localPosition = new Vector2(4.35f, 2.97f);

            }
            else if(gameObject.name=="PunchFist")
            {
                transform.localPosition = new Vector2(8.47f, 3.13f);

            }

        }
        else
        {
            if (gameObject.name == "PunchArm")
            {
                transform.localPosition = new Vector2(-4.07f, 2.97f);
            }
            else if (gameObject.name == "PunchFist")
            {
                transform.localPosition = new Vector2(-7.18f,3.13f);
            }


        }
    }
}
