using UnityEngine;

public class ExitToTutorial : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //나가기 팝업창 띄워주고

            if (Input.GetKeyDown(KeyCode.A))
            {
                Manager.Scene.LoadScene("TutorialScene");
            }
        }
    }
}
