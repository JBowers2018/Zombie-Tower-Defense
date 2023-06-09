using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    private GameObject pathPoint, towerPoint, canvas;
    private int x, z, xSize, zSize, adjustedZ, adjustedJ;
    private int[,] grid;
    private List<GameObject> path, towers;
    private bool finished;
    private float minZ, maxZ, minX, maxX;

    // Start is called before the first frame update
    void Start()
    {
        pathPoint = Resources.Load<GameObject>("Prefabs/Map Point");
        towerPoint = Resources.Load<GameObject>("Prefabs/Tower Button");
        canvas = GameObject.Find("Canvas");

        path = new List<GameObject>();
        towers = new List<GameObject>();

        x = 0; z = 0; xSize = 20; zSize = 20; minZ = 999; maxZ = -999;
        grid = new int[xSize, zSize];
        finished = false;

        CreateGrid();
    }

    public void CreateGrid()
    {
        int ranDirection = 0, prevDirection = -1;
        z = Random.Range(0, zSize);
        adjustedZ = z + 13;

        path.Add(Instantiate(pathPoint, new Vector3(x, GetTerrainHeight(x, adjustedZ), adjustedZ), 
            Quaternion.identity, transform));

        path[0].name = "Point 0";
        grid[x, z] = 1;

        while(x < xSize - 1)
        {
            ranDirection = Random.Range(0, 3);

            while ((ranDirection == 0 && prevDirection == 1) || (ranDirection == 1 && prevDirection == 0))
                ranDirection = Random.Range(0, 3);

            switch(ranDirection)
            {
                case 0: if (z < zSize - 1) { z++; prevDirection = 0; } else { x++; prevDirection = -1; } break;
                case 1: if (z > 0) { z--; prevDirection = 1; } else { x++; prevDirection = -1; } break;
                case 2: x++; prevDirection = -1; break;
            }

            adjustedZ = z + 13;

            path.Add(Instantiate(pathPoint, new Vector3(x, GetTerrainHeight(x, adjustedZ), adjustedZ), 
                Quaternion.identity, transform));
            path[path.Count - 1].name = "Point " + (path.Count - 1).ToString();
            grid[x, z] = 1;

            CreateTowerLocations(x, z);
        }

        for(int i = 0; i < xSize; i++)
        {
            for(int j = 0; j < zSize; j++)
            {
                if (grid[i, j] == 2)
                {
                    adjustedJ = j + 13;

                    GameObject tClone = Instantiate(towerPoint, new Vector3(i, GetTerrainHeight(i, adjustedJ), adjustedJ),
                        towerPoint.transform.rotation, canvas.transform);
                    towers.Add(tClone);

                    if (adjustedJ < minZ) minZ = adjustedJ;
                    if (adjustedJ > maxZ) maxZ = adjustedJ;

                    if (i < minX) minX = i;
                    if (i > maxX) maxX = i;
                }
            }
        }

        finished = true;
    }

    private void CreateTowerLocations(int x, int z)
    {
        int a, b;

        // Outer loop determines vertical position (starts at bottom).
        for(int i = -1; i <= 1; i++)
        {
            // Inner loop determines horizontal position (starts at left).
            for(int j = -1; j <= 1; j++)
            {
                a = x + j; b = z + i;

                if (a >= 0 && b >= 0 && a < xSize && b < zSize && grid[a, b] == 0)
                    grid[a, b] = 2;
            }
        }
    }

    private float GetTerrainHeight(float i, float j)
    {
        RaycastHit hit;

        if (Physics.Raycast(new Vector3(i, 10, j), Vector3.down, out hit) || 
            Physics.Raycast(new Vector3(i, 10, j), Vector3.up, out hit))
            return hit.point.y + 0.5f;

        return 0;
    }

    public List<GameObject> GetPath()
    { return path; }

    public List<GameObject> GetTowers()
    { return towers; }

    public bool GetFinished()
    { return finished; }

    public float GetMinZ()
    { return minZ; }

    public float GetMaxZ()
    { return maxZ; }

    public float GetMiddleZ()
    { return (minZ + maxZ) / 2; }

    public float GetMiddleX()
    { return (minX + maxX) / 2; }
}
