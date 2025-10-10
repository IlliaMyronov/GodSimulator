using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/*
public class TestPathFinder : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject pathFinder;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<IGoal> myGoals;

    private Vector2Int goalCoordinates;
    private Stack<Vector2Int> goalPath;
    private string goalName;

    private List<GoalWithPriority> toDoList;

    private void Awake()
    {
        goalPath = new Stack<Vector2Int>();
        toDoList = new List<GoalWithPriority>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            goalCoordinates = new Vector2Int(Mathf.RoundToInt(mainCamera.ScreenToWorldPoint(Input.mousePosition).x), 
                                             Mathf.RoundToInt(mainCamera.ScreenToWorldPoint(Input.mousePosition).y));
            
            goalPath = pathFinder.GetComponent<PathFinder>().FindPath(new Vector2Int(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y)), 
                                                                      goalCoordinates, gameManager.GetComponent<WorldManager>().GetMap());
        }

        if(goalPath.Count > 0)
        {
            this.MoveTowards(goalPath.Peek());
        }

        if(goalPath.Count == 0)
        {
            foreach (IGoal goal in myGoals)
            {
                int importance = goal.Evaluate();

                if (importance > 0)
                {
                    toDoList.Add(new GoalWithPriority(goal, importance));
                }
            }

            if(toDoList.Count != 0)
            {
                goalPath = toDoList[0].goal.GetOrders()[0].directions;
                goalName = toDoList[0].goal.GetOrders()[0].name;
            }
        }
    }

    private void MoveTowards(Vector2Int goal)
    {
        Vector2 direction = goal - new Vector2(this.transform.position.x, this.transform.position.y);

        // this is for when we reached destination 
        if (Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2)) < speed * Time.deltaTime)
        {
            this.transform.position = new Vector3(goal.x, goal.y, this.transform.position.z);
            goalPath.Pop();

            Debug.Log("path has " + goalPath.Count + " elements left");
            // check if this was the last element, if it was, execute order
            if(goalPath.Count == 0)
            {
                Debug.Log("executing tree");
                toDoList[0].goal.Execute(goalName);
            }

            return;
        }

        // this if is for when I don't reach destination on current move
        direction = direction.normalized;
        this.transform.position = new Vector3(this.transform.position.x + ((direction.x) * speed * Time.deltaTime), this.transform.position.y + ((direction.y) * speed * Time.deltaTime), this.transform.position.z);
    }
}
*/