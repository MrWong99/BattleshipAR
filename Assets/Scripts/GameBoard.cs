using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour
{

    public HittableTile[] ClickableTiles;

    public GameObject[] ShotresultTiles;

    public GameObject[] Ships;

    public void Update()
    {
        foreach (HittableTile tile in ClickableTiles) {
            if (tile.getIsHit())
            {
                if (isBelowShip(tile.transform.position))
                {
                    tile.HitOnShip = true;
                }
            }
        }
    }

    private bool isBelowShip(Vector3 pTile)
    {
        foreach (GameObject ship in Ships)
        {
            Bounds b = ship.GetComponent<Collider>().bounds;
            if (b.Contains(pTile))
            {
                return true;
            }
        }
        return false;
    }

    private class Position
    {
        public int X
        {
            get { return X; }
            set
            {
                if (value < 0 || value > 9)
                {
                    throw new System.Exception("Value " + value + " not on board!");
                }
                else
                {
                    X = value;
                }
            }
        }

        public int Y
        {
            get { return Y; }
            set
            {
                if (value < 0 || value > 9)
                {
                    throw new System.Exception("Value " + value + " not on board!");
                }
                else
                {
                    Y = value;
                }
            }
        }

        public Position() : this(0, 0)
        { }

        public Position(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    private class Ship
    {
        public Position Start
        {
            get { return Start; }
            set
            {
                if (End == null)
                {
                    Start = value;
                    return;
                }
                if (value.X != End.X && value.Y != End.Y)
                {
                    throw new System.Exception("Ship can't be diagonal.");
                }
                int size = -1;
                if (value.X == End.X)
                {
                    size = value.Y - End.Y;
                }
                else if (value.Y == End.Y)
                {
                    size = value.X - End.X;
                }
                else
                {
                    throw new System.Exception();
                }
                if (size < 2 || size > 5)
                {
                    throw new System.Exception("Size can only be between 2 and 5 (inclusive).");
                }
                Start = value;
                Size = size;
            }
        }

        public Position End
        {
            get { return End; }
            set
            {
                if (Start == null)
                {
                    End = value;
                    return;
                }
                if (value.X != Start.X && value.Y != Start.Y)
                {
                    throw new System.Exception("Ship can't be diagonal.");
                }
                int size = -1;
                if (value.X == Start.X)
                {
                    size = value.Y - Start.Y;
                }
                else if (value.Y == Start.Y)
                {
                    size = value.X - Start.X;
                }
                else
                {
                    throw new System.Exception();
                }
                if (size < 2 || size > 5)
                {
                    throw new System.Exception("Size can only be between 2 and 5 (inclusive).");
                }
                End = value;
                Size = size;
            }
        }

        public int Size { get { return Size; } set { } }

        public Ship(Position start, Position end)
        {
            Start = start;
            End = end;
        }
    }
}
