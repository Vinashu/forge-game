using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player
{
    string name = "Player #1";
    int matches = 0;
    int points = 0;
    int winsInARow = 0;

    public Dictionary<string, Reward> rewards = new Dictionary<string, Reward>();

    public Player()
    {

    }

    public Player (string name, int matches, int points, int winsInARow)
    {
        this.name = name;
        this.matches = matches;
        this.points = points;
        this.winsInARow = winsInARow;
    }

    public Postman.Dispatcher CreateMessages()
    {
        ArrayList messageList = new ArrayList();
        messageList.Add(new Postman.Message("matches", this.matches));
        messageList.Add(new Postman.Message("points", this.points));
        messageList.Add(new Postman.Message("winsInARow", this.winsInARow));
        Postman.Message[] messages = (Postman.Message[])messageList.ToArray(typeof(Postman.Message));
        Postman.Dispatcher dispatcher = new Postman.Dispatcher(messages);
        return dispatcher;
    }

    public void UpdateRewards(Rewards result)
    {
        foreach (Reward reward in result.rewards)
        {
            if (reward.category[0].name.Equals("Points")) {
                this.UpdatePoints(Int32.Parse(reward.name));
            } else if (reward.category[0].name.Equals("Badge"))
            {
                if (!this.rewards.ContainsKey(reward._id))
                {
                    this.rewards.Add(reward._id, reward);
                }
            }
        }
    }

    public void UpdateMatches()
    {
        this.matches++;
    }

    public void UpdatePoints(int points)
    {
        this.points += points;
    }

    public void UpdateWinsInARow()
    {
        this.winsInARow++;
    }

    public void ResetWinsInARow()
    {
        this.winsInARow = 0;
    }

    public string GetName()
    {
        return this.name;
    }

    public int GetMatches()
    {
        return this.matches;
    }

    public int GetPoints()
    {
        return this.points;
    }

    public int GetWinsInARow()
    {
        return this.winsInARow;
    }

    internal int GetNumberBadges()
    {
        return this.rewards.Count;
    }
}
