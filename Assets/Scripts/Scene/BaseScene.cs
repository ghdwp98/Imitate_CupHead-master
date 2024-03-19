using System.Collections;
using UnityEngine;

public abstract class BaseScene : MonoBehaviour
{

    public Vector2 worldPos;

    public abstract IEnumerator LoadingRoutine();
}
