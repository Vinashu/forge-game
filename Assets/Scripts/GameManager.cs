using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int players = 0;
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

    public void Play()
    {
        int points = UnityEngine.Random.Range(0, 100);
        points = Mathf.RoundToInt(rewardPoints.Evaluate(points));
        if (points >= 1)
        {
            this.player.UpdateLevel();
        }
        this.player.UpdateLevels();
        this.player.UpdatePoints(points);
        this.player.UpdateTotalPoints(points);
        Postman.Dispatcher dispatcher = this.player.CreateMessages();
        string url = Postman.Instance.getServer() + "/api/engine";
        string json = JsonUtility.ToJson(dispatcher);
        Postman.Instance.Post(
            json,
            url,
            (error) =>
            {
                Debug.LogError($"error: {error}");
                EventBroker.CallOnPostmanError(error);
            },
            (result) =>
            {
                Debug.Log($"result: {result}");
                Rewards rewards = JsonUtility.FromJson<Rewards>(result);
                Debug.Log($"Rewards received: {rewards.rewards.Length}");
                this.player.UpdateRewards(rewards);
                EventBroker.CallOnPostmanSuccess(
                    $"Message successefuly received from the server! {rewards.rewards.Length} rewards for you."
                );
                EventBroker.CallOnPlayerUpdate(this.player);
            }
        );
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Play();
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            PlayerInit();
        }
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
    }

    public void PlayerInit()
    {
        this.players++;
        player = new Player($"Player #{this.players}", 0, 0, 0,0);
        EventBroker.CallOnNewPlayer(this.player);
    }
}
