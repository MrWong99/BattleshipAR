using UnityEngine;
using System.Collections;

public class GameBoard : MonoBehaviour
{

    private HittableTile[] ClickableTiles;

    public GameObject[] Ships;

    public GameObject TilePrefab;

    public void Update()
    {
        foreach (HittableTile tile in ClickableTiles)
        {
            if (tile.getIsHit())
            {
                if (isBelowShip(tile.GetComponent<Collider>().bounds))
                {
                    tile.HitOnShip = true;
                }
            }
        }
    }

    public void Start()
    {
        ClickableTiles = new HittableTile[100];
        int i = 0;
        for(int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Object o = Instantiate(TilePrefab, new Vector3(-0.45f + 0.1f * x, 0, -0.45f + 0.1f * y), Quaternion.identity);
                ClickableTiles[i] = ((GameObject)o).GetComponent<HittableTile>();
                i++;
            }
        }
    }

    private bool isBelowShip(Bounds colTile)
    {
        foreach (GameObject ship in Ships)
        {
            Bounds colShip = ship.GetComponent<Collider>().bounds;
            if (colShip.Intersects(colTile))
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
