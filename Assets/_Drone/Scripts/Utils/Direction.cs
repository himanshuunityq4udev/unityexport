using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RDC
{
    public class Direction
    {
        private static Vector2[] directions = new Vector2[]
         {
                            new Vector2(1, 0),    // left
                            new Vector2(-1, 0),   // right
                            new Vector2(0, 1),    // forward
                            new Vector2(0, -1),    // backward

        };

        public static Vector2[] Directions => directions;
    }
}