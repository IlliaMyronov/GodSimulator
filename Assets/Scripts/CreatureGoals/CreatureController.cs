using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : MonoBehaviour
{
    [SerializeField] private float speed;
    private WorldManager worldManager;

    // kid goals store goals that kid has, they might be shared with an adult or unique
    [SerializeField] private List<IGoal> kidGoals;
    [SerializeField] private List<IGoal> goals;

    // to do list keeps track of potential future goals and their priority
    private Dictionary<IGoal, int> toDoList;

    // current orders tracks the most important goal
    private List<Action> currentOrders;

    // current goal saves type of the goal, in order to execute order after reaching destination
    private IGoal currentGoal;

    private List<IGoal> myAgeGoals;

    private void Awake()
    {
        currentOrders = new List<Action>();
        toDoList = new Dictionary<IGoal, int>();

        worldManager = FindObjectOfType<WorldManager>();

        if(this.GetComponent<CreatureStats>().IsAdult())
        {
            myAgeGoals = goals;
        }

        else
        {
            myAgeGoals = kidGoals;
        }
    }

    private void FixedUpdate()
    {
        if(worldManager.GetMap() == null)
        {
            return;
        }
        // evaluate goals

        if(currentOrders.Count == 0)
        {
            if(this.GetComponent<CreatureStats>().IsAdult())
            {

            }
            foreach (IGoal toEvaluate in myAgeGoals)
            {

                int importance = toEvaluate.Evaluate();

                if (importance > 0)
                {
                    if (toDoList.ContainsKey(toEvaluate))
                    {
                        toDoList[toEvaluate] = importance;
                    }
                    else
                    {
                        toDoList.Add(toEvaluate, importance);
                    }
                }
            }
        }
        

        // select a task from to do list if doing nothing
        if(toDoList.Count > 0)
        {
            if (currentOrders.Count == 0)
            {
                currentGoal = MostImportantGoal();
                currentOrders = currentGoal.GetOrders();

                // make sure that goal selected does have directions, if goal can not be achieved, directions will be empty
                for (int i = 1; i < toDoList.Count; i++)
                {
                    
                    if (currentOrders.Count > 0)
                    { break; }

                    toDoList.Remove(currentGoal);
                    currentGoal = MostImportantGoal();
                    currentOrders = currentGoal.GetOrders();
                }

                // if we did find a goal that has directions, assign it and remove from toDoList
                if (currentOrders.Count > 0)
                {
                    toDoList.Remove(currentGoal);
                }
            }
        }

        // if we have where to move, move
        if(currentOrders.Count > 0)
        {
            if (currentOrders[0].directions.Count > 0)
            {
                MoveTowards(currentOrders[0].directions.Peek());
            }

            else
            {
                currentGoal.Execute(currentOrders[0].name);
                currentOrders.RemoveAt(0);
            }
        }
    }

    private IGoal MostImportantGoal()
    {
        IGoal toReturn = null;
        int priority = 0;

        foreach(var kvp in toDoList)
        {
            if(priority < kvp.Value)
            {
                toReturn = kvp.Key;
                priority = kvp.Value;
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
            currentOrders[0].directions.Pop();

            // check if this was the last element, if it was, execute order, and delete this goal
            if (currentOrders[0].directions.Count == 0)
            {
                currentGoal.Execute(currentOrders[0].name);
                currentOrders.RemoveAt(0);
            }

            return;
        }

        // this if is for when I don't reach destination on current move
        direction = direction.normalized;
        this.transform.position = new Vector3(this.transform.position.x + ((direction.x) * speed * Time.deltaTime), this.transform.position.y + ((direction.y) * speed * Time.deltaTime), this.transform.position.z);
    }

    public float GetSpeed()
    {
        return this.speed;
    }
    public void SetSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }

    public void NowAdult()
    {
        myAgeGoals = goals;
        toDoList.Clear();
    }
}