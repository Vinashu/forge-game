using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Player player;
    [SerializeField]
    private AnimationCurve rewardPoints;

    int noPoints = 0;
    int minPoints = 50;
    int avgPoints = 100;
    int maxPoints = 150;


    void Start()
    {
        ConfigPoints();
        PlayerInit();
    }

    void Play()
    {
        int points = Random.Range(0, 100);
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(0)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(10)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(20)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(30)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(40)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(50)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(60)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(70)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(80)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(90)));
        Debug.Log(Mathf.RoundToInt(rewardPoints.Evaluate(100)));
    }
    
    void Update()
    {
        
    }

    void ConfigPoints()
    {
        rewardPoints.AddKey(0, this.noPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 0, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 0, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(5, this.noPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 1, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 1, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(6, this.minPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 2, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 2, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(50, this.minPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 3, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 3, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(51, this.avgPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 4, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 4, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(85, this.avgPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 5, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 5, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(86, this.maxPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 6, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 6, AnimationUtility.TangentMode.Linear);
        rewardPoints.AddKey(100, this.maxPoints);
        AnimationUtility.SetKeyLeftTangentMode(rewardPoints, 7, AnimationUtility.TangentMode.Linear);
        AnimationUtility.SetKeyRightTangentMode(rewardPoints, 7, AnimationUtility.TangentMode.Linear);
        Play();
    }

    void PlayerInit()
    {
        player = new Player();
        Debug.Log(player);
    }
}
