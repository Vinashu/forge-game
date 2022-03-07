using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward  
{
    public int _id;
    string category;
    string name;
    string description;
    public string imagePath;
    Texture2D texture2D;
    Sprite sprite;
}

[Serializable]
public class Rewards
{
    public Reward[] rewards;

    Rewards() { }

    Rewards(Reward[] rewards)
    {
        this.rewards = rewards;
    }
}