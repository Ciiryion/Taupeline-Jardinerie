using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public float x, y;
    public List<bool> isClose;
    public List<Direction> sorties;
    public int ennemisNbr = 0;
    public Type type = Type.Normal;

    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum Type
    {
        Normal,
        KeyRoom,
        Shop
    }

    public Room(float x, float y, List<Direction> sorties, List<bool> isClose, int ennemisNbr)
    {
        this.x = x;
        this.y = y;
        this.sorties = sorties;
        this.isClose = isClose;
        this.ennemisNbr = ennemisNbr;
    }
}
