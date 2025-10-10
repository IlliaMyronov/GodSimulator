using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public string name;
    public Stack<Vector2Int> directions;

    public Action(string myName, Stack<Vector2Int> myDirections)
    {
        name = myName;
        directions = myDirections;
    }
}
