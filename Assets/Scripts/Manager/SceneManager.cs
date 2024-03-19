using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] Image fade;
    [SerializeField] Slider loadingBar;
    [SerializeField] float fadeTime;
    [SerializeField] GameObject loadingAnim;
    [SerializeField] public Vector2 playerPos;
    private BaseScene curScene;
    

    public BaseScene GetCurScene()
    {
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }
        return curScene;
    }

    public T GetCurScene<T>() where T : BaseScene
    {
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }
        return curScene as T;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        yield return FadeOut();

        Manager.Pool.ClearPool();
        Manager.Sound.StopSFX();
        Manager.UI.ClearPopUpUI();
        Manager.UI.ClearWindowUI();
        Manager.UI.CloseInGameUI();

        // ��������� �̵��� ���� ��ġ�� ����������ϴϱ�
        // ��� ��쿡 �����ұ�? 

        Time.timeScale = 0f;

        loadingAnim.gameObject.SetActive(true);

        BaseScene PrevScene=GetCurScene(); //�ϴ� �ε� �������� ������ ����

        playerPos = PrevScene.worldPos; 
        Debug.Log("�Ŵ��� pos");
        Debug.Log(playerPos);


        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        while (oper.isDone == false)
        {

            loadingAnim.gameObject.GetComponent<Animator>().Play("LoadingAnim");
            
            yield return new WaitForSecondsRealtime(2f);
        }

        Manager.UI.EnsureEventSystem();

        BaseScene curScene = GetCurScene();
        curScene.worldPos = PrevScene.worldPos;
        playerPos= curScene.worldPos;
        Debug.Log("�Ŵ��� pos cur�� ");
        Debug.Log(playerPos); //���⼭ 0 0 ���� �ʱ�ȭ 
        yield return curScene.LoadingRoutine(); //������� �ε� ��ƾ ���� �ϴ� ��ġ.

        
        

        loadingAnim.gameObject.SetActive(false);
        Time.timeScale = 1f;

        yield return FadeIn();
    }

    IEnumerator FadeOut()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 2)
        {
            rate += Time.deltaTime / fadeTime;
            fade.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 2)
        {
            rate += Time.deltaTime / fadeTime;
            fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }
    }
}
