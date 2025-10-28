using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SearchUtility
{
    public static Vector2Int? FindNearestTile(Vector2Int start, int maxRadius, Func<Vector2Int, bool> condition)
    {
        //position.y = worldManager.GetMap().Count - position.y;
        for (int i = 0; i < maxRadius; i++)
        {
            // need to check square, border of whick is i tiles away from the position.
            // right and left sides will have length of 2*i + 1
            // top and bottom of this square (discluding corners) will have length of 2*i - 1
            // coordinates of top corners of this square would be position.x +- i position.y + i

            int length = 2 * i + 1;
            int previusLength = 2 * (i - 1) + 1;

            // best case
            if (i == 0)
            {
                if (condition(start))
                {
                     return start;
                }
            }

            else
            {
                // need to check length^2 - previous length^2 number of squares
                for (int j = 0; j < (length * length) - (previusLength * previusLength); j++)
                {
                    /* 
                     * possible directions are:
                     * 0 - right
                     * 1 - up
                     * 2 - left
                     * 3 - down
                     */

                    int direction = j % 4;

                    // subdirection points to where a new node is relative to the first node in a square.
                    /*
                     * if sub direction is odd:
                     * and direction is:
                     * 0 - move up
                     * 1 - move left
                     * 2 - move down
                     * 3 - move right
                     * 
                     * if sub direction is even:
                     * and direction is:
                     * 0 - move down
                     * 1 - move right
                     * 2 - move up
                     * 3 - move left
                     */

                    int subDirection = (j - direction) / 4;
                    Vector2Int positionToCheck = new Vector2Int();

                    // shift tracks by how much we shift across sides
                    // shift is negative when odd, positive when even 

                    int shift = ((subDirection + 1) / 2 * (subDirection % 2 == 0 ? -1 : 1));

                    switch (direction)
                    {
                        case 0:
                            positionToCheck = new Vector2Int(start.x + i, start.y + shift);
                            break;
                        case 1:
                            positionToCheck = new Vector2Int(start.x + shift, start.y + i);
                            break;
                        case 2:
                            positionToCheck = new Vector2Int(start.x - i, start.y + shift);
                            break;

                        case 3:
                            positionToCheck = new Vector2Int(start.x + shift, start.y - i);
                            break;
                    }

                    
                    if (condition(positionToCheck))
                    {
                        return positionToCheck;
                    }
                }
            }
        }

        return null;
    }

    public static Vector2Int ConvertPosition(Vector3 toTransform)
    {
        return new Vector2Int(Mathf.RoundToInt(toTransform.x), Mathf.RoundToInt(toTransform.y));
    }
}
