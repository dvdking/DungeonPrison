using DungeonPrisonLib.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public class LOS
    {
        bool[,] visibleArea;

        public LOS()
        {
            visibleArea = new bool[Settings.PlayerView.Width, Settings.PlayerView.Height];
        }

        public bool IsVisible(int x, int y)
        {
            return visibleArea[x, y];
        }

        public void UpdateVisibleArea(Actor player, TileMap tileMap)
        {
            for (int i = 0; i < Settings.PlayerView.Width; i++)
            {
                for (int j = 0; j < Settings.PlayerView.Height; j++)
                {
                    visibleArea[i, j] = InteserectsWall(player, tileMap, player.X + i - Settings.PlayerView.Width / 2, player.Y + j - Settings.PlayerView.Height / 2);
                }
            }
        }

        public bool InteserectsWall(Actor player, TileMap tileMap, int x, int y)
        {
            if (!tileMap.InBounds(x, y))
            {
                return true;
            }

            int t, px, py, ax, ay, sx, sy, dx, dy;
            px = 0;
            py = 0;
 
           /* Delta x is the players x minus the monsters x    *
            * d is my dungeon structure and px is the players  *
            * x position. mx is the monsters x position passed *
            * to the function.                                 */
           dx = x - player.X;
 
           /* dy is the same as dx using the y coordinates */
           dy = y - player.Y;
 
           /* ax & ay: these are the absolute values of dx & dy *
            * multiplied by 2 ( shift left 1 position)          */
           ax = Math.Abs(dx)<<1;
           ay = Math.Abs(dy)<<1;
 
           /* sx & sy: these are the signs of dx & dy */
           sx = Math.Sign(dx);
           sy = Math.Sign(dy);
 
           /* x & y: these are the monster's x & y coords */
           px = player.X;
           py = player.Y;
 
           /* The following if statement checks to see if the line *
            * is x dominate or y dominate and loops accordingly    */
           if(ax > ay)
           {
              /* X dominate loop */
              /* t = the absolute of y minus the absolute of x divided *
                 by 2 (shift right 1 position)                         */
              t = ay - (ax >> 1);
              do
              {
                 if(t >= 0)
                 {
                    /* if t is greater than or equal to zero then *
                     * add the sign of dy to y                    *
                     * subtract the absolute of dx from t         */
                    py += sy;
                    t -= ax;
                 }
 
                 /* add the sign of dx to x      *
                  * add the adsolute of dy to t  */
                 px += sx;
                 t += ay;
 
                 /* check to see if we are at the player's position */
                 if (px == x && py == y)
                 {
                    /* return that the monster can see the player */
                    return true;
                 }
              /* keep looping until the monster's sight is blocked *
               * by an object at the updated x,y coord             */
              }
              while(tileMap.IsSolid(px,py) == false);
 
              /* NOTE: sight_blocked is a function that returns true      *
               * if an object at the x,y coord. would block the monster's *
               * sight                                                    */
 
              /* the loop was exited because the monster's sight was blocked *
               * return FALSE: the monster cannot see the player             */
              return false;
           }
           else
           {
              /* Y dominate loop, this loop is basically the same as the x loop */
              t = ax - (ay >> 1);
              do
              {
                 if(t >= 0)
                 {
                    px += sx;
                    t -= ay;
                 }
                 py += sy;
                 t += ax;
                 if(px == x && py == y)
                 {
                    return true;
                 }
              }
              while (tileMap.IsSolid(px, py) == false);
              return false;
           }
        }
    }
}
