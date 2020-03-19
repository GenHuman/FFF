using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World : MonoBehaviour
{

    public byte[,,] data;
    public int worldX = 64;
    public int worldY = 64;
    public int worldZ = 64;
    public GameObject chunk;
    public GameObject[,,] chunks;
    public int chunkSize = 16;



    void Start()
    {
        data = new byte[worldX, worldY, worldZ];
        /*for (int x = 0; x < worldX; x++)
        {
            for (int y = 0; y < worldY; y++)
            {
                for (int z = 0; z < worldZ; z++)
                {

                    if (y <= 8)
                    {
                        data[x, y, z] = 1;
                    }

                }
            }
        }
        */
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
                stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
                int dirt = PerlinNoise(x, 100, z, 50, 2, 0) + 1; //Added +1 to make sure minimum grass height is 1

                for (int y = 0; y < worldY; y++)
                {
                    if (y <= stone)
                    {
                        data[x, y, z] = 1;
                    }
                    else if (y <= dirt + stone)
                    { //Changed this line thanks to a comment
                        data[x, y, z] = 2;
                    }

                }
            }
        }

        chunks = new GameObject[Mathf.FloorToInt(worldX / chunkSize),
        Mathf.FloorToInt(worldY / chunkSize),
        Mathf.FloorToInt(worldZ / chunkSize)];

         for (int x = 0; x < chunks.GetLength(0); x++)
         {
             for (int y = 0; y < chunks.GetLength(1); y++)
             {
                 for (int z = 0; z < chunks.GetLength(2); z++)
                 {

                     chunks[x, y, z] = Instantiate(chunk,
                      new Vector3(x * chunkSize, y * chunkSize, z * chunkSize),
                      new Quaternion(0, 0, 0, 0)) as GameObject;

                     Chunk newChunkScript = chunks[x, y, z].GetComponent<Chunk>();



                     newChunkScript.worldGO = gameObject;
                     newChunkScript.chunkSize = chunkSize;
                     newChunkScript.chunkX = x * chunkSize;
                     newChunkScript.chunkY = y * chunkSize;
                     newChunkScript.chunkZ = z * chunkSize;

                 }
             }
         }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public byte Block(int x, int y, int z)
    {

        if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
        {
            return (byte)1;
        }

        return data[x, y, z];
    }


    int PerlinNoise(int x, int y, int z, float scale, float height, float power)
    {
        float rValue;
        rValue = Noise.GetNoise(((float)x) / scale, ((float)y) / scale, ((float)z) / scale);
        rValue *= height;

        if (power != 0)
        {
            rValue = Mathf.Pow(rValue, power);
        }

        return (int)rValue;
    }


    /*public static float Noise(float x, float y, float z)
    {
        var X = Mathf.FloorToInt(x) & 0xff;
        var Y = Mathf.FloorToInt(y) & 0xff;
        var Z = Mathf.FloorToInt(z) & 0xff;
        x -= Mathf.Floor(x);
        y -= Mathf.Floor(y);
        z -= Mathf.Floor(z);
        var u = Fade(x);
        var v = Fade(y);
        var w = Fade(z);
        var A = (perm[X] + Y) & 0xff;
        var B = (perm[X + 1] + Y) & 0xff;
        var AA = (perm[A] + Z) & 0xff;
        var BA = (perm[B] + Z) & 0xff;
        var AB = (perm[A + 1] + Z) & 0xff;
        var BB = (perm[B + 1] + Z) & 0xff;
        return Lerp(w, Lerp(v, Lerp(u, Grad(perm[AA], x, y, z), Grad(perm[BA], x - 1, y, z)),
                               Lerp(u, Grad(perm[AB], x, y - 1, z), Grad(perm[BB], x - 1, y - 1, z))),
                       Lerp(v, Lerp(u, Grad(perm[AA + 1], x, y, z - 1), Grad(perm[BA + 1], x - 1, y, z - 1)),
                               Lerp(u, Grad(perm[AB + 1], x, y - 1, z - 1), Grad(perm[BB + 1], x - 1, y - 1, z - 1))));
    }

    static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    static float Lerp(float t, float a, float b)
    {
        return a + t * (b - a);
    }

    static float Grad(int hash, float x)
    {
        return (hash & 1) == 0 ? x : -x;
    }

    static float Grad(int hash, float x, float y)
    {
        return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
    }

    static float Grad(int hash, float x, float y, float z)
    {
        var h = hash & 15;
        var u = h < 8 ? x : y;
        var v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    static int[] perm = {
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        151
    };*/
}
