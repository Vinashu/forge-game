using System;
using System.Collections;
using System.Collections.Generic;

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

}
