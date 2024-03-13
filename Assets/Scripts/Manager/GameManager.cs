using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : Singleton<GameManager>
{
    int coinScore = 0;

    public void SetScore(int value)
    {
        coinScore += value;
    }

    public int getScore()
    {
        return coinScore;
    }

    public void Test()
    {
        Debug.Log(GetInstanceID());
        Debug.Log(coinScore);
    }
    
}
