using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SearchForFood : IGoal
{
    [SerializeField] private CreatureStats stats;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private PathFinder pathFinder;
    [SerializeField] private GameObject character;

    // value should be in (0,1) range
    [SerializeField] private float hungerMattersThreshold;

    [SerializeField] private float foodSearchRadius;

    public override int Evaluate()
    {
        int importance = EvaluationFormula();

        // check if creature has inventory
        Inventory inv = character.GetComponent<Inventory>();
        if (inv != null && importance > 0)
        {
            float foodEaten = Mathf.Min(stats.maxHunger - stats.myHunger, inv.nutritionStored);

            inv.nutritionStored -= foodEaten;
            stats.Feed(foodEaten);
        }

        // re-evaluate hunger
        importance = EvaluationFormula();

        if(importance > 0)
        {
            return importance;
        }

        else if(inv != null)
        {
            if(inv.desiredNutritionStored > inv.nutritionStored)
            {
                // return how important it is to fill inventory with food
                // 10 is a completely random constant
                return 10;
            }
        }

        return 0;
    }

    private int EvaluationFormula()
    {
        return (int)Mathf.Max((stats.maxHunger * hungerMattersThreshold - stats.myHunger) * ((stats.maxHunger - stats.myHunger) / stats.maxHunger) /*maybe extra division in the future*/, 0);
    }

    // should decide between "gather" and "eat"
    public override List<Action> GetOrders()
    {
        GameObject target = FindFood(new Vector2Int((int)character.transform.position.x, (int)character.transform.position.y), -1, foodSearchRadius);
        List<Action> orders = new List<Action>();

        if(target == null)
        {
            return orders;
        }

        Stack<Vector2Int> directions = pathFinder.FindPath(new Vector2Int((int)character.transform.position.x, (int)character.transform.position.y), 
                                                           new Vector2Int((int)target.transform.position.x, (int)target.transform.position.y));

        // need to decide between gather and eat
        string order;
        if(EvaluationFormula() > 0)
        {
            // if hungry, eat straight off the food source
            order = "eat";
        }
        else if(character.GetComponent<Inventory>() != null)
        {
            // if goal is to fill inventory
            order = "gather";
        }
        else
        {
            // if got here by mistake
            return orders;
        }
        orders.Add(new Action(order, directions));

        return orders;
    }

    private GameObject FindFood(Vector2Int position, int direction, float distanceToCheck)
    {
        GameObject closestFood = null;
        GameObject newFood = null;

        // check for out of bounds
        if (position.x >= gameManager.GetComponent<WorldManager>().GetMap()[0].Count ||
            position.y >= gameManager.GetComponent<WorldManager>().GetMap().Count)

        {
            return null;
        }

        // best case
        if (gameManager.GetComponent<WorldManager>().GetTile(position).hasFlora)
        {
            return gameManager.GetComponent<WorldManager>().GetPlant(position);
        }

        float straightDistance = 1f;
        float diagonalDistance = 1.4f;

        if (distanceToCheck - straightDistance >= 0)
        {
            // check straight tiles
            // if initial node, check all 4 directions
            // if node came from straight direction, check its straight directionsss
            // if node came from diagonal direction, don't check its straight neighbors
            if (direction == -1)
            {
                newFood = FindFood(CalculateNewPos(position, 0), 0, distanceToCheck - straightDistance);
                closestFood = ChooseFood(position, closestFood, newFood);

                newFood = FindFood(CalculateNewPos(position, 2), 2, distanceToCheck - straightDistance);
                closestFood = ChooseFood(position, closestFood, newFood);

                newFood = FindFood(CalculateNewPos(position, 4), 4, distanceToCheck - straightDistance);
                closestFood = ChooseFood(position, closestFood, newFood);

                newFood = FindFood(CalculateNewPos(position, 6), 6, distanceToCheck - straightDistance);
                closestFood = ChooseFood(position, closestFood, newFood);

            }
            else if (direction % 2 == 0)
            {
                newFood = FindFood(CalculateNewPos(position, direction), direction, distanceToCheck - straightDistance);
                closestFood = ChooseFood(position, closestFood, newFood);
            }

            if (distanceToCheck - diagonalDistance >= 0)
            {
                if (direction == -1)
                {
                    newFood = FindFood(CalculateNewPos(position, 1), 1, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);

                    newFood = FindFood(CalculateNewPos(position, 3), 3, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);

                    newFood = FindFood(CalculateNewPos(position, 5), 5, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);

                    newFood = FindFood(CalculateNewPos(position, 7), 7, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);
                }
                else if (direction % 2 == 0)
                {
                    newFood = FindFood(CalculateNewPos(position, direction + 1), direction + 1, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);

                    newFood = FindFood(CalculateNewPos(position, (direction - 1 + 8) % 8), (direction - 1 + 8) % 8, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);
                }
                else
                {
                    newFood = FindFood(CalculateNewPos(position, direction), direction, distanceToCheck - diagonalDistance);
                    closestFood = ChooseFood(position, closestFood, newFood);
                }
            }
        }

         return closestFood;
    }

    // the only current action here is "gather"
    public override void Execute(string action)
    {
        if(action == "gather")
        {
            Debug.Log("called gather");

            gameManager.GetComponent<WorldManager>().Gather(new Vector2Int(Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt(character.transform.position.y)));
            character.GetComponent<Inventory>().nutritionStored += 10;

            Debug.Log("current nutrition in the inventory is " + character.GetComponent<Inventory>().nutritionStored);

        }

        else if(action == "eat")
        {
            Debug.Log("eating");
            gameManager.GetComponent<WorldManager>().Gather(new Vector2Int(Mathf.RoundToInt(character.transform.position.x), Mathf.RoundToInt(character.transform.position.y)));
            stats.Feed(10);

            Debug.Log("my hunger now is " + stats.myHunger);
        }
    }

    private Vector2Int CalculateNewPos(Vector2Int oldPos, int dir)
    {
        return new Vector2Int(oldPos.x + (Mathf.RoundToInt(Mathf.Cos(dir * (Mathf.PI / 4)))),
                              oldPos.y + (Mathf.RoundToInt(Mathf.Sin(dir * (Mathf.PI / 4)))));
    }

    private GameObject LeastDistance(Vector2Int myPos, GameObject obj1, GameObject obj2)
    {
        Vector2 obj1Pos = obj1.transform.position;
        Vector2 obj2Pos = obj2.transform.position;

        if ((obj1Pos - myPos).sqrMagnitude < (obj2Pos - myPos).sqrMagnitude)
        {
            return obj1;
        }

        return obj2;
    }

    private GameObject ChooseFood(Vector2Int pos, GameObject obj1, GameObject obj2)
    {
        if(obj1 == null)
        {
            if(obj2 == null)
            {
                return null;
            }

            else if(obj2.GetComponent<PlantInfo>().CanGather("food"))
            {
                return obj2;
            }

            return null;
        }

        else
        {
            if (obj2 == null)
            {
                if (obj1.GetComponent<PlantInfo>().CanGather("food"))
                {
                    return obj1;
                }
                return null;
            }
        }

        // at this point both are not null
        if(obj1.GetComponent<PlantInfo>().CanGather("food"))
        {
            if(obj2.GetComponent<PlantInfo>().CanGather("food"))
            {
                return LeastDistance(pos, obj1, obj2);
            }
            else
            {
                return obj1;
            }
        }

        else
        {
            if(obj2.GetComponent<PlantInfo>().CanGather("food"))
            {
                return obj2;
            }

            return null;
        }
    }
}

