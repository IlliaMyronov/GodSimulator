using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGoal : MonoBehaviour
{
    public abstract int Evaluate();
    public abstract List<Action> GetOrders();
    public abstract void Execute(string action);
}
