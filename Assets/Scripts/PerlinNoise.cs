using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 2f;

    #region Singleton
    private static PerlinNoise _instance;
    public static PerlinNoise Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();

    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        return texture;
    }

    Color CalculateColor(int x, int y)
    {

        float xCoord = (float)x / width * scale;
        float yCord = (float)y / height * scale;

        float sample = Mathf.PerlinNoise(xCoord, yCord);
        return new Color(sample, sample, sample);

    }

    public float CalculateValue(int x, int z)
    {
        float xCoord = (float)x / width * scale;
        float zCord = (float)z / height * scale;

        return Mathf.PerlinNoise(xCoord, zCord);

    }

    public float CalculateValue(int x, int z,float scale,int width,int height)
    {
        float xCoord = (float)x / width * scale;
        float zCord = (float)z / height * scale;

        return Mathf.PerlinNoise(xCoord, zCord);

    }
}
