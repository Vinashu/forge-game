using System;
using UnityEngine;

[Serializable]
public class Reward  
{
    public string _id;
    public Category[] category;
    public string name;
    string description;
    public string imagePath;
    Texture2D texture2D;
    Sprite sprite;
}

[Serializable]
public class Category
{
    public string _id;
    public string name;
    string description;
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