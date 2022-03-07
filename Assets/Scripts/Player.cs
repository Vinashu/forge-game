using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    string name = "Player #1";
    int level = 0;
    int levels = 0;
    int points = 0;
    int totalPoints = 0;

    public Dictionary<int, Reward> rewards = new Dictionary<int, Reward>();

    public Player()
    {

    }

    public Player (string name, int level, int levels, int points, int totalPoints)
    {
        this.name = name;
        this.level = level;
        this.levels = levels;
        this.points = points;
        this.totalPoints = totalPoints;
    }

    public Postman.Dispatcher CreateMessages()
    {
        ArrayList messageList = new ArrayList();
        messageList.Add(new Postman.Message("level", this.level));
        messageList.Add(new Postman.Message("levels", this.levels));
        messageList.Add(new Postman.Message("points", this.points));
        messageList.Add(new Postman.Message("totalPoints", this.totalPoints));
        Postman.Message[] messages = (Postman.Message[])messageList.ToArray(typeof(Postman.Message));
        Postman.Dispatcher dispatcher = new Postman.Dispatcher(messages);
        //Debug.Log(JsonUtility.ToJson(dispatcher));
        return dispatcher;
    }

    public void UpdateRewards(Rewards result)
    {
        foreach(Reward reward in result.rewards)
        {
            if(!this.rewards.ContainsKey(reward._id))
            {
                this.rewards.Add(reward._id, reward);
            }
        }
    }

    public void UpdateLevel()
    {
        this.level++;
    }

    public void UpdateLevels()
    {
        this.levels++;
    }

    public void UpdatePoints(int points)
    {
        this.points = points;
    }

    public void UpdateTotalPoints(int points)
    {
        this.totalPoints += points;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetLevel()
    {
        return this.level;
    }

    public int GetLevels()
    {
        return this.levels;
    }

    public int GetPoints()
    {
        return this.points;
    }

    public int GetTotalPoints()
    {
        return this.totalPoints;
    }

    internal int GetNumberBadges()
    {
        return this.rewards.Count;
    }
}
