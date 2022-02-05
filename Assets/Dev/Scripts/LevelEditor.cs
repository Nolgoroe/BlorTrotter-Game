using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public Texture2D map;
    public LevelParser[] colorMappings;
    
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        for (int x = 0; x< map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }
    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);
        
        if (pixelColor.a == 0)
        {
            return;
        }
        foreach (LevelParser colorMapping in colorMappings)
        {
            if (colorMapping.color == pixelColor)
            {
                Vector3 position = new Vector3(x, 0, y);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
            }
        }


    }
    
}
