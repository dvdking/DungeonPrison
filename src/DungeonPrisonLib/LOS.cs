﻿using DungeonPrisonLib.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    visibleArea[i, j] = false;
                }
            }

            for (int i = 0; i < Settings.PlayerView.Width; i++)
            {
                Parallel.For(0, Settings.PlayerView.Height, (j) =>
               // {
               // for (int j = 0; j < Settings.PlayerView.Height; j++)
                {
                    if (visibleArea[i, j] == false)
                    {
                        visibleArea[i, j] = InteserectsWall(player, tileMap, player.X + i - Settings.PlayerView.Width / 2, player.Y + j - Settings.PlayerView.Height / 2);
                        if (visibleArea[i, j] == true)
                        {
                            //SetVisibleNearNodes(player, tileMap, i, j);
                        }
                    }
               // }
                });
            }

        }

        static public bool InteserectsWall(Actor actor, TileMap tileMap, float x, float y)
        {
            const int amount = 1;

            if (!tileMap.InBounds((int)x, (int)y))
            {
                return true;
            }

            x *= amount;
            y *= amount;


            float t, px, py, ax, ay, sx, sy, dx, dy;
            px = 0;
            py = 0;
 
           /* Delta x is the players x minus the monsters x    *
            * d is my dungeon structure and px is the players  *
            * x position. mx is the monsters x position passed *
            * to the function.                                 */
           dx = x - actor.X*amount;
 
           /* dy is the same as dx using the y coordinates */
           dy = y - actor.Y*amount;
 
           /* ax & ay: these are the absolute values of dx & dy *
            * multiplied by 2 ( shift left 1 position)          */
           ax = Math.Abs(dx)*2;
           ay = Math.Abs(dy)*2;
 
           /* sx & sy: these are the signs of dx & dy */
           sx = Math.Sign(dx);
           sy = Math.Sign(dy);
 
           /* x & y: these are the monster's x & y coords */
           px = actor.X * amount;
           py = actor.Y * amount;
 
           /* The following if statement checks to see if the line *
            * is x dominate or y dominate and loops accordingly    */
           if(ax > ay)
           {
              t = ay - ax /2f;
              do
              {
                 if(t >= 0)
                 {
                    py += sy;
                    t -= ax;
                 }

                 px += sx;
                 t += ay;
 
                 if (px == x && py == y)
                 {
                    return true;
                 }
              }
              while (tileMap.IsSolid((int)(px / amount),(int)( py / amount)) == false);
              return false;
           }
           else
           {
              /* Y dominate loop, this loop is basically the same as the x loop */
              t = ax - ay /2f;
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
              while (tileMap.IsSolid((int)(px / amount),(int)(py / amount)) == false);
              return false;
           }
        }
    }
}
