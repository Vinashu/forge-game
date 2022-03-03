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

    ArrayList rewards = new ArrayList();

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
        //Postman.Message[] messages = new Postman.Message[4];
        //messages[0] = (new Postman.Message("level", this.level));
        //messages[1] = (new Postman.Message("levels", this.levels));
        //messages[2] = (new Postman.Message("points", this.points));
        //messages[3] = (new Postman.Message("totalPoints", this.totalPoints));
        Postman.Dispatcher dispatcher = new Postman.Dispatcher(messages);
        Debug.Log(JsonUtility.ToJson(dispatcher));
        return dispatcher;
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
}
