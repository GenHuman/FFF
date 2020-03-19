using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Map : MonoBehaviour
{

    public int width = 64;
    public int height = 64;
    public int length = 64;

    public float chanceOfElevation = 0.5f;
    public float neighbourInfluence = 0.25f;

    public GameObject prefab;

    bool[,,] byteMap;
    int blockCount = 0;
    List<Vector3> vertexTNE = new List<Vector3>();
    List<Vector3> vertexTNW = new List<Vector3>();
    List<Vector3> vertexTSE = new List<Vector3>();
    List<Vector3> vertexTSW = new List<Vector3>();
    List<Vector3> vertexBNE = new List<Vector3>();
    List<Vector3> vertexBNW = new List<Vector3>();
    List<Vector3> vertexBSE = new List<Vector3>();
    List<Vector3> vertexBSW = new List<Vector3>();

    public Mesh mesh;
    private MeshCollider col;

    private Dictionary<int, List<int>> edges = new Dictionary<int, List<int>>();

    public GameObject[] mapLimits;


    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();
    private int faceCount;
    private Vector2 tStone = new Vector2(1, 0);
    private float tUnit = 0.25f;

    void Start()
    {

        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();


        byteMap = new bool[width, height, length];
        ///System.Random rand = new System.Random(0);

        /*for(int y = 0; y<height; y++)
        {
            if (y == 0)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        byteMap[x, y, z] = rand.NextDouble() < chanceOfElevation;
                        if (z == 0 || z == length || x == 0 || x == width || y == 0 || y == height)
                        {

                        }
                        else
                        {

                        }

                        if (byteMap[x, y, z]) Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
            else{
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        byteMap[x, y, z] = rand.NextDouble() < chanceOfElevation/y && byteMap[x, y-1, z];

                        if (byteMap[x, y, z]) Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
                    }
                }
            }
        }*/
        generateMap(0);

        /*Array.Clear(byteMap,0,byteMap.Length);
        byteMap[4, 2, 4] = true;
        byteMap[4, 1, 4] = true;*/
        //List< Vector3 > vectoruniq= new List<Vector3>();
        //vectoruniq = vertexBSE.Concat(vertexBNW).Concat(vertexBSW).Concat(vertexBNE).Concat(vertexTSE).Concat(vertexTNW).Concat(vertexTSW).Concat(vertexTNE).ToList();
        /*vertexBSE = vertexBSE.Distinct().ToList();
        vertexBNW = vertexBSE.Distinct().ToList();
        vertexBSW = vertexBSE.Distinct().ToList();
        vertexBNE = vertexBSE.Distinct().ToList();
        vertexTSE = vertexBSE.Distinct().ToList();
        vertexTNW = vertexBSE.Distinct().ToList();
        vertexTSW = vertexBSE.Distinct().ToList();
        vertexTNE = vertexBSE.Distinct().ToList();*/

        // mesh.
        //List<int> triangles = new List<int>();

        //Debug.Log(vectoruniq.Count);
        //vectoruniq = vectoruniq.Distinct().ToList();
        //Debug.Log(vectoruniq.Count);
        //mesh.SetVertices(vectoruniq);
        //mesh.Optimize();
        /*bool cleanupDone = false;
        while (!cleanupDone)
        {
            bool verticalCleanupDone = false;
            while (!verticalCleanupDone)
            {

            }
        }*/


        /*for (int i = 0; i < vectoruniq.Count; i++)
        {

            Instantiate(prefab, vertexBSE[i], Quaternion.identity);
            Instantiate(prefab, vertexBNW[i], Quaternion.identity);
            Instantiate(prefab, vertexBSW[i], Quaternion.identity);
            Instantiate(prefab, vertexBNE[i], Quaternion.identity);
            Instantiate(prefab, vertexTSE[i], Quaternion.identity);
            Instantiate(prefab, vertexTNW[i], Quaternion.identity);
            Instantiate(prefab, vertexTSW[i], Quaternion.identity);
            Instantiate(prefab, vertexTNE[i], Quaternion.identity);
            Instantiate(prefab, vectoruniq[i], Quaternion.identity);
        }*/




        //GenerateMesh();

        initiateMapLimits(width, height, length);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void initiateMapLimits(int width, int height, int length)
    {
        /*float xWallPosition = (float)width / 2 + 0.5f;
        float yWallPosition = (float)height / 2;
        float zWallPosition = (float)length / 2 + 0.5f;*/

        float xWallPosition = (float)width / 2;
        float yWallPosition = (float)height / 2;
        float zWallPosition = (float)length / 2;

        int xWallSize = width + 2;
        int yWallSize = height + 2;
        int zWallSize = length + 2;


        mapLimits[0].transform.position = new Vector3(xWallPosition, -0.5f, zWallPosition);
        mapLimits[0].transform.localScale = new Vector3(xWallSize, 1, zWallSize);

        mapLimits[1].transform.position = new Vector3(xWallPosition, yWallPosition, length+0.5f);
        mapLimits[1].transform.localScale = new Vector3(xWallSize, yWallSize, 1);

        mapLimits[2].transform.position = new Vector3(width+0.5f, yWallPosition, zWallPosition);
        mapLimits[2].transform.localScale = new Vector3(1, yWallSize, zWallSize);

        mapLimits[3].transform.position = new Vector3(xWallPosition, yWallPosition, -0.5f);
        mapLimits[3].transform.localScale = new Vector3(xWallSize, yWallSize, 1);

        mapLimits[4].transform.position = new Vector3(-0.5f, yWallPosition, zWallPosition);
        mapLimits[4].transform.localScale = new Vector3(1, yWallSize, zWallSize);

        mapLimits[5].transform.position = new Vector3(xWallPosition, height + 0.5f, zWallPosition);
        mapLimits[5].transform.localScale = new Vector3(xWallSize, 1, zWallSize);

        /*
        mapLimits[0].transform.position = new Vector3(0, -0.5f, 0);
        mapLimits[0].transform.localScale = new Vector3(xWallSize, 1, zWallSize);

        mapLimits[1].transform.position = new Vector3(0, yWallPosition, zWallPosition);
        mapLimits[1].transform.localScale = new Vector3(xWallSize, yWallSize, 1);

        mapLimits[2].transform.position = new Vector3(xWallPosition, yWallPosition, 0);
        mapLimits[2].transform.localScale = new Vector3(1, yWallSize, zWallSize);

        mapLimits[3].transform.position = new Vector3(0, yWallPosition, -zWallPosition);
        mapLimits[3].transform.localScale = new Vector3(xWallSize, yWallSize, 1);

        mapLimits[4].transform.position = new Vector3(-xWallPosition, yWallPosition, 0);
        mapLimits[4].transform.localScale = new Vector3(1, yWallSize, zWallSize);

        mapLimits[5].transform.position = new Vector3(0, height + 0.5f, 0);
        mapLimits[5].transform.localScale = new Vector3(xWallSize, 1, zWallSize);*/

    }

    /*void addVertex(float x, float y, float z)
    {

        float xDisplace = (float)width / 2;

        float zDisplace = (float)length / 2;

        vertexBSE.Add(new Vector3(x - xDisplace - 0.5f, y - 0.5f, z - zDisplace - 0.5f));
        vertexBNW.Add(new Vector3(x - xDisplace + 0.5f, y - 0.5f, z - zDisplace + 0.5f));
        vertexBSW.Add(new Vector3(x - xDisplace + 0.5f, y - 0.5f, z - zDisplace - 0.5f));
        vertexBNE.Add(new Vector3(x - xDisplace - 0.5f, y - 0.5f, z - zDisplace + 0.5f));


        vertexTSE.Add(new Vector3(x - xDisplace - 0.5f, y + 0.5f, z - zDisplace - 0.5f));
        vertexTNW.Add(new Vector3(x - xDisplace + 0.5f, y + 0.5f, z - zDisplace + 0.5f));
        vertexTSW.Add(new Vector3(x - xDisplace + 0.5f, y + 0.5f, z - zDisplace - 0.5f));
        vertexTNE.Add(new Vector3(x - xDisplace - 0.5f, y + 0.5f, z - zDisplace + 0.5f));

    }*/




    void GenerateMesh()
    {

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < length; z++)
                {
                    //This code will run for every block in the chunk

                    if (Block(x, y, z))
                    {
                        //If the block is solid

                        if (!Block(x, y + 1, z))
                        {
                            //Block above is air
                            CubeTop(x, y, z, Block(x, y, z));
                        }

                        if (!Block(x, y - 1, z))
                        {
                            //Block below is air
                            CubeBot(x, y, z, Block(x, y, z));

                        }

                        if (!Block(x + 1, y, z))
                        {
                            //Block east is air
                            CubeEast(x, y, z, Block(x, y, z));

                        }

                        if (!Block(x - 1, y, z))
                        {
                            //Block west is air
                            CubeWest(x, y, z, Block(x, y, z));

                        }

                        if (!Block(x, y, z + 1))
                        {
                            //Block north is air
                            CubeNorth(x, y, z, Block(x, y, z));

                        }

                        if (!Block(x, y, z - 1))
                        {
                            //Block south is air
                            CubeSouth(x, y, z, Block(x, y, z));

                        }

                    }

                }
            }
        }

        UpdateMesh();
    }


    void CubeTop(int x, int y, int z, bool block)
    {

        newVertices.Add(new Vector3(x, y+1, z + 1));
        newVertices.Add(new Vector3(x + 1, y+1, z + 1));
        newVertices.Add(new Vector3(x + 1, y+1, z));
        newVertices.Add(new Vector3(x, y+1, z));

        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);

    }

    void CubeNorth(int x, int y, int z, bool block)
    {

        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y+1, z + 1));
        newVertices.Add(new Vector3(x, y+1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));

        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);

    }
    void CubeEast(int x, int y, int z, bool block)
    {

        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y+1, z));
        newVertices.Add(new Vector3(x + 1, y+1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));

        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);

    }
    void CubeWest(int x, int y, int z, bool block)
    {

        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y+1, z + 1));
        newVertices.Add(new Vector3(x, y+1, z));
        newVertices.Add(new Vector3(x, y, z));


        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);

    }
    void CubeSouth(int x, int y, int z, bool block)
    {

        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y+1, z));
        newVertices.Add(new Vector3(x + 1, y+1, z));
        newVertices.Add(new Vector3(x + 1, y, z));

        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);

    }
    void CubeBot(int x, int y, int z, bool block)
    {

        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));

        Vector2 texturePos;

        texturePos = tStone;

        Cube(texturePos);

    }

    void UpdateMesh()
    {
        mesh.Clear();
        //newVertices = newVertices.Distinct().ToList();




        //vertexTriangleCleanUp(ref newVertices,ref newTriangles, ref newUV);


        for (int i = 0; i < newVertices.Count; i++) {
            Instantiate(prefab, newVertices[i], Quaternion.identity);
        }

        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.Optimize();
        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();

        faceCount = 0; //Fixed: Added this thanks to a bug pointed out by ratnushock!
    }

    void Cube(Vector2 texturePos)
    {

        newTriangles.Add(faceCount * 4); //1
        List<int> l = new List<int>();
        l.Add(faceCount * 4 + 1);
        l.Add(faceCount * 4 + 2);
        l.Add(faceCount * 4 + 3);
        edges.Add(faceCount * 4, l);
        newTriangles.Add(faceCount * 4 + 1); //2
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4 + 3); //4

        newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y));
        newUV.Add(new Vector2(tUnit * texturePos.x + tUnit, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y + tUnit));
        newUV.Add(new Vector2(tUnit * texturePos.x, tUnit * texturePos.y));

        faceCount++; // Add this line
    }

    bool Block(int x, int y, int z)
    {
        //Debug.Log(x + "," + y + "," + z);
        if(x>=width || z >= length || x < 0 || y < 0 || z < 0)
        {
            return true;
        }
        else if (y >= height )
        {
            return false;
        }
        
        return byteMap[x, y, z];
    }


    void vertexTriangleCleanUp(ref List <Vector3> vertices, ref List<int> triangles, ref List<Vector2> uvs)
    {
        bool cleanUpDone = false;
        int count = 0;
        int count2 = 1;
        //bool removed = false;
        
        while (!cleanUpDone && count < vertices.Count-1)
        {
            //Debug.Log(count + " " + count2 + " "+ vertices.Count);
            if (Approximately(vertices[count], vertices[count2], 0f))
            {

                while (triangles.Contains(count2))
                {
                    int index = triangles.IndexOf(count2);
                    if (index != -1)
                    {
                       
                        triangles[index] = count;
                        /*if (!removed)
                        {
                            uvs.RemoveAt(count2);
                            vertices.RemoveAt(count2);
                        }*/
                        //removed = true;
                    }
                }
            }
            //if (!removed)
                count2++;
            
            if (count2 >= vertices.Count)
            {
                count++;
                count2 = count + 1;
            }
            ///removed = false;
        }

        /*int maxvert = triangles.Max();
        for (int i = triangles.Count-1; i>maxvert; i--)
        {

        }*/
        
    }

    void triangleCleanUp(ref List<Vector3> vertices, ref List<int> triangles)
    {

    }



    public bool Approximately(Vector3 me, Vector3 other, float allowedDifference)
    {
        var dx = me.x - other.x;
        if (Mathf.Abs(dx) > allowedDifference)
            return false;

        var dy = me.y - other.y;
        if (Mathf.Abs(dy) > allowedDifference)
            return false;

        var dz = me.z - other.z;

        return Mathf.Abs(dz) <= allowedDifference;
    }

    private void generateMap(int seed)
    {
        #region generation
        System.Random rand = new System.Random(seed);
        for (int y = 0; y < height; y++)
        {
            int x = 0, xe = width, z = 0, ze = length, i;
            while (x < xe && z < ze)
            {
                // Print the first row  
                // from the remaining rows 

                for (i = z; i < ze; ++i)
                {

                    if (y == 0)
                    {
                        
                        int neighbors = 0;
                        if (x > 0)
                        {
                            if (byteMap[x - 1, y, i])
                            {
                                neighbors++;
                            }
                        }
                        if (x < width-1)
                        {
                            if (byteMap[x + 1, y, i])
                            {
                                neighbors++;
                            }
                        }
                        if (i > 0)
                        {
                            if (byteMap[x, y, i-1])
                            {
                                neighbors++;
                            }
                        }

                        if (i < length-1)
                        {
                            if (byteMap[x, y, i + 1])
                            {
                                neighbors++;
                            }
                        }
                        if (neighbors > 1)
                        {
                            byteMap[x, y, i] = true;
                        }
                        else if (neighbors == 1)
                        {
                            byteMap[x, y, i] = rand.NextDouble() < (chanceOfElevation*2);
                        }
                        else
                        {
                            byteMap[x, y, i] = rand.NextDouble() < chanceOfElevation;
                        }
                        

                    }
                    else
                    {
                        //Debug.Log("Rand: "+rand.NextDouble()+" chanceofElev / y:" +chanceOfElevation +" / "+ y +" prev: "  +byteMap[x, y - 1, i]);
                        //Debug.Log(rand.NextDouble() < chanceOfElevation / y && byteMap[x, y - 1, i]);

                        int neighbors = 0;
                        if (x > 0)
                        {
                            if (byteMap[x - 1, y, i])
                            {
                                neighbors++;
                            }
                        }
                        if (x < width - 1)
                        {
                            if (byteMap[x + 1, y, i])
                            {
                                neighbors++;
                            }
                        }
                        if (i > 0)
                        {
                            if (byteMap[x, y, i - 1])
                            {
                                neighbors++;
                            }
                        }

                        if (i < length - 1)
                        {
                            if (byteMap[x, y, i + 1])
                            {
                                neighbors++;
                            }
                        }
                        if (neighbors > 1)
                        {
                            byteMap[x, y, i] =  byteMap[x, y - 1, i];
                        }
                        else if (neighbors == 1)
                        {
                            byteMap[x, y, i] = rand.NextDouble() < (chanceOfElevation * 2) && byteMap[x, y - 1, i];
                        }
                        else
                        {
                            byteMap[x, y, i] = rand.NextDouble() < chanceOfElevation / y && byteMap[x, y - 1, i];
                        }
                       
                    }


                    if (byteMap[x, y, i])
                    {
                        //addVertex(x, y, i);
                        Instantiate(prefab, new Vector3(x, y, i), Quaternion.identity);//Console.Write(a[k, i] + " ");
                        blockCount++;
                    }
                }
                x++;

                // Print the last column from the 
                // remaining columns 
                for (i = x; i < xe; ++i)
                {
                    if (y == 0)
                    {
                        int neighbors = 0;
                        if (i > 0)
                        {
                            if (byteMap[i - 1, y, ze - 1])
                            {
                                neighbors++;
                            }
                        }
                        if (i < width - 1)
                        {
                            if (byteMap[i + 1, y, ze - 1])
                            {
                                neighbors++;
                            }
                        }
                        if (ze > 1)
                        {
                            if (byteMap[x, y, ze - 2])
                            {
                                neighbors++;
                            }
                        }

                        if (ze < length - 1)
                        {
                            if (byteMap[i, y, ze])
                            {
                                neighbors++;
                            }
                        }
                        if (neighbors > 1)
                        {
                            byteMap[i, y, ze - 1] = true;
                        }
                        else if (neighbors == 1)
                        {
                            byteMap[i, y, ze - 1] = rand.NextDouble() < (chanceOfElevation * 2);
                        }
                        else
                        {
                            byteMap[i, y, ze - 1] = rand.NextDouble() < chanceOfElevation / y;
                        }


                    }
                    else
                    {
                        //Debug.Log("Rand: "+rand.NextDouble()+" chanceofElev / y:" +chanceOfElevation +" / "+ y +" prev: "  +byteMap[x, y - 1, i]);
                        //Debug.Log(rand.NextDouble() < chanceOfElevation / y && byteMap[x, y - 1, i]);

                        int neighbors = 0;
                        if (i > 0)
                        {
                            if (byteMap[i-1, y, ze - 1])
                            {
                                neighbors++;
                            }
                        }
                        if (i < width - 1)
                        {
                            if (byteMap[i + 1, y, ze - 1])
                            {
                                neighbors++;
                            }
                        }
                        if (ze > 1)
                        {
                            if (byteMap[x, y, ze - 2])
                            {
                                neighbors++;
                            }
                        }

                        if (ze < length - 1)
                        {
                            if (byteMap[i, y, ze])
                            {
                                neighbors++;
                            }
                        }
                        if (neighbors > 1)
                        {
                            byteMap[i, y, ze - 1] = byteMap[i, y - 1, ze - 1];
                        }
                        else if (neighbors == 1)
                        {
                            byteMap[i, y, ze - 1] = rand.NextDouble() < (chanceOfElevation * 2) && byteMap[i, y-1, ze - 1];
                        }
                        else
                        {
                            byteMap[i, y, ze - 1] = rand.NextDouble() < chanceOfElevation / y && byteMap[i, y-1, ze - 1];
                        }

                    }
                    /*if (y == 0)
                    {
                        byteMap[i, y, ze - 1] = rand.NextDouble() < chanceOfElevation;
                    }
                    else
                    {
                        byteMap[i, y, ze - 1] = rand.NextDouble() < chanceOfElevation / y && byteMap[i, y - 1, ze - 1];
                    }*/

                    if (byteMap[i, y, ze - 1])
                    {
                        //addVertex(i, y, ze - 1);
                        Instantiate(prefab, new Vector3(i, y, ze - 1), Quaternion.identity); //Console.Write(a[i, n - 1] + " ");
                        blockCount++;
                    }
                }
                ze--;

                // Print the last row from  
                // the remaining rows  
                if (x < xe)
                {
                    for (i = ze - 1; i >= z; --i)
                    {
                        int n = 0;
                        if (y == 0)
                        {

                            int neighbors = 0;
                            if (xe > 1)
                            {
                                if (byteMap[xe - 2, y, i])
                                {
                                    neighbors++;
                                }
                            }
                            if (xe < width - 1)
                            {
                                if (byteMap[xe, y, i])
                                {
                                    neighbors++;
                                }
                            }
                            if (i > 0)
                            {
                                if (byteMap[xe - 1, y, i - 1])
                                {
                                    neighbors++;
                                }
                            }

                            if (i < length - 1)
                            {
                                if (byteMap[xe - 1, y, i + 1])
                                {
                                    neighbors++;
                                }
                            }
                            if (neighbors > 1)
                            {
                                byteMap[xe - 1, y, i] = true;
                            }
                            else if (neighbors == 1)
                            {
                                byteMap[xe - 1, y, i] = rand.NextDouble() < (chanceOfElevation * 2);
                            }
                            else
                            {
                                byteMap[xe - 1, y, i] = rand.NextDouble() < chanceOfElevation / y;
                            }
                            n = neighbors;

                        }
                        else
                        {
                            //Debug.Log("Rand: "+rand.NextDouble()+" chanceofElev / y:" +chanceOfElevation +" / "+ y +" prev: "  +byteMap[x, y - 1, i]);
                            //Debug.Log(rand.NextDouble() < chanceOfElevation / y && byteMap[x, y - 1, i]);

                            int neighbors = 0;
                            if (xe > 1)
                            {
                                if (byteMap[xe - 2, y, i])
                                {
                                    neighbors++;
                                }
                            }
                            if (xe < width - 1)
                            {
                                if (byteMap[xe, y, i])
                                {
                                    neighbors++;
                                }
                            }
                            if (i > 0)
                            {
                                if (byteMap[xe - 1, y, i - 1])
                                {
                                    neighbors++;
                                }
                            }

                            if (i < length - 1)
                            {
                                if (byteMap[xe - 1, y, i + 1])
                                {
                                    neighbors++;
                                }
                            }
                            if (neighbors > 1)
                            {
                                byteMap[xe - 1, y, i] = byteMap[xe - 1, y - 1, i];
                            }
                            else if (neighbors == 1)
                            {
                                byteMap[xe - 1, y, i] = rand.NextDouble() < (chanceOfElevation * 2) && byteMap[xe - 1, y - 1, i];
                            }
                            else
                            {
                                byteMap[xe - 1, y, i] = rand.NextDouble() < chanceOfElevation / y && byteMap[xe - 1, y - 1, i];
                            }
                            n = neighbors;
                        }


                        /*if (y == 0)
                        {
                            byteMap[xe - 1, y, i] = rand.NextDouble() < chanceOfElevation;
                        }
                        else
                        {
                            byteMap[xe - 1, y, i] = rand.NextDouble() < chanceOfElevation / y && byteMap[xe - 1, y - 1, i];
                        }*/


                        if (byteMap[xe - 1, y, i])
                        {
                            //addVertex(xe - 1, y, i);
                            GameObject g = Instantiate(prefab, new Vector3(xe - 1, y, i), Quaternion.identity);//Console.Write(a[m - 1, i] + " ");
                            if (n > 0)
                                g.GetComponent<MeshRenderer>().material.color = Color.red;
                            blockCount++;
                        }
                    }
                    xe--;
                }

                // Print the first column from  
                // the remaining columns 
                if (z < x)
                {
                    for (i = xe - 1; i >= x; --i)
                    {
                       int n = 0;
                        if (y == 0)
                        {

                            int neighbors = 0;
                            if (i > 1)
                            {
                                if (byteMap[i - 1, y, z])
                                {
                                    neighbors++;
                                }
                            }
                            if (i < width - 1)
                            {
                                if (byteMap[i + 1, y, z])
                                {
                                    neighbors++;
                                }
                            }
                            if (z > 0)
                            {
                                if (byteMap[i, y, z - 1])
                                {
                                    neighbors++;
                                }
                            }

                            if (z < length - 1)
                            {
                                if (byteMap[i, y, z + 1])
                                {
                                    neighbors++;
                                }
                            }
                            if (neighbors > 1)
                            {
                                byteMap[i, y, z] = true;
                            }
                            else if (neighbors == 1)
                            {
                                byteMap[i, y, z] = rand.NextDouble() < (chanceOfElevation * 2);
                            }
                            else
                            {
                                byteMap[i, y, z] = rand.NextDouble() < chanceOfElevation / y;
                            }
                            n = neighbors;

                        }
                        else
                        {
                            //Debug.Log("Rand: "+rand.NextDouble()+" chanceofElev / y:" +chanceOfElevation +" / "+ y +" prev: "  +byteMap[x, y - 1, i]);
                            //Debug.Log(rand.NextDouble() < chanceOfElevation / y && byteMap[x, y - 1, i]);

                            int neighbors = 0;
                            if (i > 1)
                            {
                                if (byteMap[i - 1, y, z])
                                {
                                    neighbors++;
                                }
                            }
                            if (i < width - 1)
                            {
                                if (byteMap[i + 1, y, z])
                                {
                                    neighbors++;
                                }
                            }
                            if (z > 0)
                            {
                                if (byteMap[i, y, z - 1])
                                {
                                    neighbors++;
                                }
                            }

                            if (z < length - 1)
                            {
                                if (byteMap[i, y, z + 1])
                                {
                                    neighbors++;
                                }
                            }
                            if (neighbors > 1)
                            {
                                byteMap[i, y, z] = byteMap[i, y - 1, z];
                            }
                            else if (neighbors == 1)
                            {
                                byteMap[i, y, z] = rand.NextDouble() < (chanceOfElevation * 2) && byteMap[i, y - 1, z];
                            }
                            else
                            {
                                byteMap[i, y, z] = rand.NextDouble() < chanceOfElevation / y && byteMap[i, y - 1, z];
                            }
                            n = neighbors;
                        }

                        /*if (y == 0)
                        {
                            byteMap[i, y, z] = rand.NextDouble() < chanceOfElevation;
                        }
                        else
                        {
                            byteMap[i, y, z] = rand.NextDouble() < chanceOfElevation / y && byteMap[i, y - 1, z];
                        }*/

                        if (byteMap[i, y, z])
                        {
                            //addVertex(i, y, z);
                            GameObject g = Instantiate(prefab, new Vector3(i, y, z), Quaternion.identity);//Console.Write(a[i, l] + " ");
                            if (n > 0)
                                g.GetComponent<MeshRenderer>().material.color = Color.red;
                            blockCount++;
                        }
                    }
                    z++;
                }
            }
        }
        #endregion
    }
}
