using UnityEngine;
using System.Collections;
using System;

public class GameBoard : MonoBehaviour
{

    private HittableTile[] ClickableTiles;

    public GameObject[] Ships;

    public GameObject TilePrefab;

    public void Update()
    {
        if (Input.touchCount != 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    Physics.Raycast(ray, out hit, 17000f, TilePrefab.layer);
                    hit.transform.SendMessage("isHit");
                    break;
            }
        }
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

    public void OnEnable()
    {
        ClickableTiles = new HittableTile[100];
        int i = 0;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                GameObject o = Instantiate(TilePrefab, Vector3.zero, Quaternion.identity) as GameObject;
                o.transform.parent = gameObject.transform;
                Vector3 pos = new Vector3(-0.45f + 0.1f * x, gameObject.transform.position.y, -0.45f + 0.1f * y);
                o.transform.position = pos;
                ClickableTiles[i] = o.GetComponent<HittableTile>();
                i++;
            }
        }
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

        public Position()
            : this(0, 0)
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
