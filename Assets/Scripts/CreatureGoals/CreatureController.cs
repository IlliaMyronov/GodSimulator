using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject pathFinder;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private List<IGoal> goals;

    // to do list keeps track of potential future goals and their priority
    private Dictionary<IGoal, int> toDoList;

    // current orders tracks the most important goal
    private List<Action> currentOreders;

    // current goal saves type of the goal, in order to execute order after reaching destination
    private IGoal currentGoal;

    private void Awake()
    {
        currentOreders = new List<Action>();
        toDoList = new Dictionary<IGoal, int>();
    }

    private void Update()
    {
        // evaluate goals
        foreach(IGoal toEvaluate in goals)
        {
            int importance = toEvaluate.Evaluate();
            if(importance > 0)
            {
                if(toDoList.ContainsKey(toEvaluate))
                {
                    toDoList[toEvaluate] = importance;
                }
                else
                {
                    toDoList.Add(toEvaluate, importance);
                }
            }
        }

        // select a task from to do list if doing nothing
        if(toDoList.Count > 0)
        {
            if (currentOreders.Count == 0)
            {
                currentGoal = MostImportantGoal();

                // make sure that goal selected does have directions, if goal can not be achieved, directions will be empty
                for(int i = 1; i < toDoList.Count; i++)
                {
                    if(currentGoal.GetOrders().Count > 0)
                    { break; }

                    toDoList.Remove(currentGoal);
                    currentGoal = MostImportantGoal();
                }

                // if we did find a goal that has directions, assign it and remove from toDoList
                if (currentGoal.GetOrders().Count > 0)
                {
                    currentOreders = currentGoal.GetOrders();
                    toDoList.Remove(currentGoal);
                }
            }
        }

        // if we have where to move, move
        if(currentOreders.Count > 0)
        {
            if (currentOreders[0].directions.Count > 0)
            {
                MoveTowards(currentOreders[0].directions.Peek());
            }
        }
    }

    private IGoal MostImportantGoal()
    {
        IGoal toReturn = null;
        int priority = 0;

        foreach(var kvp in toDoList)
        {
            if(priority == 0)
            {
                toReturn = kvp.Key;
                priority = kvp.Value;
            }

            else
            {
                if(priority < kvp.Value)
                {
                    toReturn = kvp.Key;
                    priority = kvp.Value;
                }
            }
        }

        return toReturn;
    }

    private void MoveTowards(Vector2Int dest)
    {
        Vector2 direction = dest - new Vector2(this.transform.position.x, this.transform.position.y);

        // this is for when we reached destination 
        if (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2)) < speed * Time.deltaTime)
        {
            this.transform.position = new Vector3(dest.x, dest.y, this.transform.position.z);
            currentOreders[0].directions.Pop();

            // check if this was the last element, if it was, execute order, and delete this goal
            if (currentOreders[0].directions.Count == 0)
            {
                currentGoal.Execute(currentOreders[0].name);
                currentOreders.RemoveAt(0);
            }

            return;
        }

        // this if is for when I don't reach destination on current move
        direction = direction.normalized;
        this.transform.position = new Vector3(this.transform.position.x + ((direction.x) * speed * Time.deltaTime), this.transform.position.y + ((direction.y) * speed * Time.deltaTime), this.transform.position.z);
    }
}