using UnityEngine;

public class GoToTutorialDoor : MonoBehaviour
{
    [SerializeField] GameObject DoorKeyPrefab;
    GameObject popUp;
    [SerializeField] GameObject pos;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            popUp = Instantiate(DoorKeyPrefab, pos.transform.position, Quaternion.identity);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(popUp);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Manager.Scene.LoadScene("TutorialScene");
        }
    }

    

}
