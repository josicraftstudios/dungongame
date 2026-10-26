using System;
using System.Net.NetworkInformation;
using UnityEngine;

public class mapGen : MonoBehaviour
{
    public int width;
    public int height;
    [Range(0, 100)]
    public int RandomWallProcent;

    public string seed;
    public bool RandomSeed;
    int[,] map;

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
        }    
    }

    void GenerateMap()
    {
        map = new int[width, height];

        if (RandomSeed) { seed = Time.time.ToString(); }
        System.Random prng = new System.Random(seed.GetHashCode());


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (i == 0 || i == width - 1 || j == 0 || j == height - 1) { map[i, j] = 1; } else
                {
                    map[i, j] = (prng.Next(0, 100) < RandomWallProcent) ? 1 : 0;
                }
            
            }
        }

        for(int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
    }

    void SmoothMap()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int wallsnext = WallsAroundspot(i, j);
                if (wallsnext > 4)
                {
                    map[i, j] = 1;
                }
                else if (wallsnext < 4)
                {
                    map[i,j]= 0;
                }
            }
        }
    }


    int WallsAroundspot(int x,int y)
    {
        int nummber = 0;
        for (int i = x-1; i <= x+1; i++)
        {
            for (int j = y-1; j <= y+1; j++)
            {
                if(i!=x || j != y)
                {
                    if (i >= 0 && i < width && j >= 0 && j < height)
                    {
                        nummber += map[i, j];
                    }
                    else { nummber++; }
                }
            }
        }

        return nummber;
    }

    private void OnDrawGizmos()
    {
        if(map != null)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Gizmos.color = (map[i, j] == 1) ? Color.black : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + i + .5f,0,-height/2 + j +.5f);
                    Gizmos.DrawCube(pos, Vector3.one);

                }
            }
        }
    }

}
