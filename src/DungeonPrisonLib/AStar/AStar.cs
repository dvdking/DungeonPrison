﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Linq;

namespace DungeonPrisonLib.AStar
{
    class Node:IComparable<Node>
    {
        public Node Parent;
        public int Cost;
        public Point Position;

        public int CompareTo(Node obj)
        {
            if (obj.Cost < Cost)
                return -1;
            else if (obj.Cost > Cost)
                return 1;
            return 0;
        }
    }

    static class AStar
    {
        private const char ExploredArea = '1';
        private const char UnexploredArea = '0';

        static public Queue<Point> FindPath(Point startPoint, Point endPoint, TileMap tileMap, bool diagonal = false)
        {
            if (startPoint == endPoint)
                return new Queue<Point>();

            BinaryHeap<Node> openList = new BinaryHeap<Node>();

            openList.Add(new Node
            {
                Parent = null, Position = startPoint, Cost = 0
            });

            char[,] exploredArea = new char[tileMap.Width, tileMap.Height];
            
            while (openList.Count != 0)
            {
                Node node = openList.Remove();               

                if (node.Position == endPoint)
                {
                    Queue<Point> path = new Queue<Point>();
                    do
                    {
                        path.Enqueue(node.Position);
                        node = node.Parent;
                    } while (node != null);
                    return path;
                }

                AddNode(node, 1, 0, exploredArea, openList, tileMap);
                AddNode(node, -1, 0, exploredArea, openList, tileMap);
                AddNode(node, 0, 1, exploredArea, openList, tileMap);
                AddNode(node, 0, -1, exploredArea, openList, tileMap);

                if (diagonal)
                {
                    throw new NotImplementedException();
                }
            }
            return null;
        }
        static private void AddNode(Node p, int offsetX, int offsetY, char[,] exploredArea, BinaryHeap<Node> openList, TileMap tileMap, bool diagonal = false)
        {
            Point pos = p.Position;

            pos.X += offsetX;
            pos.Y += offsetY;

            

            if (!tileMap.InBounds(pos))
                return;

            if (tileMap.IsSolid(pos))
                return;

            if (exploredArea[pos.X, pos.Y] == ExploredArea)
                return;

            Node node = new Node 
            {
                Position = pos,
                Cost = p.Cost + (diagonal ? 14 : 10),
                Parent = p
            };
            exploredArea[pos.X, pos.Y] = ExploredArea;

            openList.Add(node);

        }


    }
}