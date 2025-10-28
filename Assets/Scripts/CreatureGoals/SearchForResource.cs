using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchForResource
{
    //[SerializeField] private WorldManager worldManager;

    // this is depth first search, doesn't make much sense
    /*
    public GameObject FindResource(Resource toFind, Vector2Int position, int direction, float radius)
    {
        GameObject closestFood = null;
        GameObject newFood = null;

        // check for out of bounds
        if (position.x >= worldManager.GetMap()[0].Count ||
            position.y >= worldManager.GetMap().Count)

        {
            return null;
        }

        // best case
        if (worldManager.GetTile(position).hasFlora)
        {
            return worldManager.GetPlant(position);
        }

        float straightDistance = 1f;
        float diagonalDistance = 1.4f;

        if (radius - straightDistance >= 0)
        {
            // check straight tiles
            // if initial node, check all 4 directions
            // if node came from straight direction, check its straight directionsss
            // if node came from diagonal direction, don't check its straight neighbors
            if (direction == -1)
            {
                newFood = FindResource(toFind, CalculateNewPos(position, 0), 0, radius - straightDistance);
                closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                newFood = FindResource(toFind, CalculateNewPos(position, 2), 2, radius - straightDistance);
                closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                newFood = FindResource(toFind, CalculateNewPos(position, 4), 4, radius - straightDistance);
                closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                newFood = FindResource(toFind, CalculateNewPos(position, 6), 6, radius - straightDistance);
                closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

            }
            else if (direction % 2 == 0)
            {
                newFood = FindResource(toFind, CalculateNewPos(position, direction), direction, radius - straightDistance);
                closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);
            }

            if (radius - diagonalDistance >= 0)
            {
                if (direction == -1)
                {
                    newFood = FindResource(toFind, CalculateNewPos(position, 1), 1, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                    newFood = FindResource(toFind, CalculateNewPos(position, 3), 3, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                    newFood = FindResource(toFind, CalculateNewPos(position, 5), 5, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                    newFood = FindResource(toFind, CalculateNewPos(position, 7), 7, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);
                }
                else if (direction % 2 == 0)
                {
                    newFood = FindResource(toFind, CalculateNewPos(position, direction + 1), direction + 1, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);

                    newFood = FindResource(toFind, CalculateNewPos(position, (direction - 1 + 8) % 8), (direction - 1 + 8) % 8, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);
                }
                else
                {
                    newFood = FindResource(toFind, CalculateNewPos(position, direction), direction, radius - diagonalDistance);
                    closestFood = ChooseClosestResource(toFind, position, closestFood, newFood);
                }
            }
        }

        return closestFood;
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

    private GameObject ChooseClosestResource(Resource toGather, Vector2Int pos, GameObject obj1, GameObject obj2)
    {
        if (obj1 == null)
        {
            if (obj2 == null)
            {
                return null;
            }

            else if (obj2.GetComponent<PlantInfo>().CanGather(toGather))
            {
                return obj2;
            }

            return null;
        }

        else
        {
            if (obj2 == null)
            {
                if (obj1.GetComponent<PlantInfo>().CanGather(toGather))
                {
                    return obj1;
                }
                return null;
            }
        }

        // at this point both are not null
        if (obj1.GetComponent<PlantInfo>().CanGather(toGather))
        {
            if (obj2.GetComponent<PlantInfo>().CanGather(toGather))
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
            if (obj2.GetComponent<PlantInfo>().CanGather(toGather))
            {
                return obj2;
            }

            return null;
        }
    }
    */
    public static GameObject FindResource2(Resource resource, WorldManager worldManager, Vector2Int position, int radius)
    {
        Vector2Int? targetPosition = SearchUtility.FindNearestTile(position, radius, pos =>
        {
            if (!(pos.x < 0 || pos.y < 0 ||
                    pos.x > worldManager.GetMap()[0].Count || pos.y > worldManager.GetMap().Count))
            {
                if (worldManager.GetTile(pos).hasFlora)
                {

                    if (worldManager.GetPlant(pos).GetComponent<PlantInfo>().CanGather(resource))
                    {
                        return true;
                    }
                }
            }
            return false;
        });

        if(targetPosition == null)
        {
            return null;
        }

        else
        {
            return worldManager.GetPlant(targetPosition.Value);
        }
    }
}
