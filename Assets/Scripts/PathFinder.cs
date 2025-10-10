using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    // variable to store maximum path difficulty
    [SerializeField] int maxPathDifficulty;
    [SerializeField] private GameObject gameManager;

    // straight distance between two points
    int straightDistance;

    // diagonal distance between two points
    int diagonalDistance;

    // variable to store the node with the smallest f value while looking for next node to explore
    private Node closest;
    private Vector2Int closestCoordinates;

    private List<List<TileInfo>> map;

    private void Awake()
    {
        straightDistance = 10;
        diagonalDistance = 14;
    }

    public Stack<Vector2Int> FindPath(Vector2Int startNodeCoordinates, Vector2Int endNodeCoordinates)
    {
        if(map == null)
        {
            map = gameManager.GetComponent<WorldManager>().GetMap();
        }
        // variable declration
        int pathDifficultyCounter = 0;

        // this dictionary stores a list of the nodes those have been evaluated, thus shortest path to these nodes was found
        Dictionary<Vector2Int, Node> evaluatedNodes = new Dictionary<Vector2Int, Node>();

        // this dictionary stores a list of nodes adjusted to already evaluated nodes, thus information about shortest path to them may change
        Dictionary<Vector2Int, Node> toEvaluateNodes = new Dictionary<Vector2Int, Node>();

        evaluatedNodes.Add(startNodeCoordinates, 
                        new Node(0, this.FindHCostOfNode(startNodeCoordinates, endNodeCoordinates), -1));

        toEvaluateNodes = EvaluateNodesAround(startNodeCoordinates, endNodeCoordinates, map[startNodeCoordinates.y][startNodeCoordinates.x].GetMoveDifficulty(), new Vector2Int(map[0].Count, map.Count), evaluatedNodes, toEvaluateNodes);

        while (!evaluatedNodes.ContainsKey(endNodeCoordinates) && pathDifficultyCounter < maxPathDifficulty)
        {
            pathDifficultyCounter++;

            // this is the base node, that is assumed to be closest, we check if it is true in for loop
            closest = toEvaluateNodes.ElementAt(0).Value;
            closestCoordinates = toEvaluateNodes.ElementAt(0).Key;

            for (int i = 0; i < toEvaluateNodes.Count; i++)
            {

                if(toEvaluateNodes.ElementAt(i).Value.fCost < closest.fCost)
                {
                    closest = toEvaluateNodes.ElementAt(i).Value;
                    closestCoordinates = toEvaluateNodes.ElementAt(i).Key;
                }

                else if (toEvaluateNodes.ElementAt(i).Value.fCost == closest.fCost && toEvaluateNodes.ElementAt(i).Value.hCost < closest.hCost)
                {
                    closest = toEvaluateNodes.ElementAt(i).Value;
                    closestCoordinates = toEvaluateNodes.ElementAt(i).Key;
                }

            }

            toEvaluateNodes.Remove(closestCoordinates);
            evaluatedNodes.Add(closestCoordinates, closest);

            toEvaluateNodes = this.EvaluateNodesAround(closestCoordinates, endNodeCoordinates, map[closestCoordinates.y][closestCoordinates.x].GetMoveDifficulty(), new Vector2Int(map[0].Count, map.Count), evaluatedNodes, toEvaluateNodes);
        }

        if (evaluatedNodes.ContainsKey(endNodeCoordinates))
        {
            return this.RecreatePath(startNodeCoordinates, endNodeCoordinates, evaluatedNodes, new Stack<Vector2Int>());
        }

        return new Stack<Vector2Int>();
    }

    // creates a brand new node
    private Node CreateNewNode(Vector2Int nodeCoordinates, Vector2Int originalNodeCoordinates, int reachDistance, Vector2Int endNode, int nodeMoveDifficulty, Dictionary<Vector2Int, Node> evaluatedNodes)
    {
        return new Node(evaluatedNodes[originalNodeCoordinates].gCost + (reachDistance * nodeMoveDifficulty), 
                        this.FindHCostOfNode(nodeCoordinates, endNode), 
                        FindOriginDirection(originalNodeCoordinates, nodeCoordinates));
    }

    // finds distance between two points
    private int FindHCostOfNode(Vector2Int nodeCoordinates, Vector2Int goalCoordinates)
    {
        int hCost = 0;

        Vector2Int difference = goalCoordinates - nodeCoordinates;
        difference = new Vector2Int(Mathf.Abs(difference.x), Mathf.Abs(difference.y));

        if(difference.x > difference.y)
        {
            hCost += difference.y * diagonalDistance;
            hCost += (difference.x - difference.y) * straightDistance;
        }

        else
        {
            hCost += difference.x * diagonalDistance;
            hCost += (difference.y - difference.x) * straightDistance;
        }

        return hCost;
    }

    // evaluates (and re-evaluates if needed) all adjustent nodes
    private Dictionary<Vector2Int, Node> EvaluateNodesAround(Vector2Int nodeCoordinates, Vector2Int endNode, int nodeMoveDifficulty, Vector2Int worldSize, Dictionary<Vector2Int, Node> evaluatedNodes, Dictionary<Vector2Int, Node> toEvaluateNodes)
    {
        int gCost = evaluatedNodes[nodeCoordinates].gCost;

        for(int i = 0; i < 8; i++)
        {

            Vector2Int nodeLookedAt = FindCoordinatesFromDirection(nodeCoordinates, i);

            // check if node we are looking at is not yet finally evaluated, if it is, no need to re-evaluate
            if (!evaluatedNodes.ContainsKey(nodeLookedAt) && nodeLookedAt.x < worldSize.x && nodeLookedAt.y < worldSize.y && nodeLookedAt.x > 0 && nodeLookedAt.y > 0)
            {

                // create a new node and check if it should be added
                Node newNode = CreateNewNode(nodeLookedAt, nodeCoordinates, i % 2 == 0 ? straightDistance : diagonalDistance, endNode, nodeMoveDifficulty, evaluatedNodes);

                if (!toEvaluateNodes.ContainsKey(nodeLookedAt))
                {

                    toEvaluateNodes.Add(nodeLookedAt, newNode);

                }

                else if (toEvaluateNodes[nodeLookedAt].fCost > newNode.fCost)
                {

                    toEvaluateNodes.Remove(nodeLookedAt);
                    toEvaluateNodes.Add(nodeLookedAt, newNode);

                }

            }
        }

        return toEvaluateNodes;

    }

    // returns an int which multiplied by pi/4 radians gives a direction of original node
    private int FindOriginDirection(Vector2Int originNode, Vector2Int newNode)
    {
        Vector2Int difference = originNode - newNode;

        float angle = Mathf.Atan((float)difference.y / (float)difference.x);

        if(difference.x < 0)
        {
            angle += Mathf.PI;
        }

        if(angle < 0)
        {
            angle += Mathf.PI * 2;
        }

        return Mathf.RoundToInt(angle / (Mathf.PI / 4));
    }

    // returns coordinates of a point from given origin point and direction (direction varies from 0 to 7 and to get an angle should be multiplied by pi/4 radians)
    private Vector2Int FindCoordinatesFromDirection(Vector2Int origin, int direction)
    {

        return new Vector2Int(origin.x + Mathf.RoundToInt(Mathf.Cos(direction * (Mathf.PI / 4))),
                              origin.y + Mathf.RoundToInt(Mathf.Sin(direction * (Mathf.PI / 4))));
    }

    private Stack<Vector2Int> RecreatePath(Vector2Int startCoordinates, Vector2Int endCoordinates, Dictionary<Vector2Int, Node> evaluatedNodes, Stack<Vector2Int> solution)
    {

        if (startCoordinates == endCoordinates)
        {
            return solution;
        }

        solution.Push(endCoordinates);

        solution = this.RecreatePath(startCoordinates, FindCoordinatesFromDirection(endCoordinates, evaluatedNodes[endCoordinates].myOrigin), evaluatedNodes, solution);
        return solution;
    }
}




public class Node
{
    // g cost is distance from starting node
    public int gCost;

    // h cost is distance to the final node
    public int hCost;

    // f cost is a sum of g and h
    public int fCost;

    // variable to save where we got to this node from in myOrigin * pi/4 radians (variable may vary from 0 to 7)
    public int myOrigin;

    public Node(int g, int h, int origin)
    {
        this.gCost = g;
        this.hCost = h;
        this.fCost = g + h;
        this.myOrigin = origin;
    }
}
