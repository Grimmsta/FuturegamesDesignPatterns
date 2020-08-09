using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MapReader
{
    public List<string> ReadFile(TextAsset file)
    {
        string pathFile = AssetDatabase.GetAssetPath(file);
        StreamReader reader = new StreamReader(pathFile);

        List<string> lines = new List<string>();

        while (reader != null)
        {
            string line = reader.ReadLine();

            if (reader.EndOfStream || line == "#")
            {
                break;
            }

            lines.Add(line);
        }
        return lines;
    }

    public List<string> ReadEnemyWaves(TextAsset file)
    {
        string pathFile = AssetDatabase.GetAssetPath(file);
        StreamReader reader = new StreamReader(pathFile);

        string previousLine = null;

        List<string> enemies = new List<string>();

        while (reader != null)
        {
            string enemy = reader.ReadLine();

            if (previousLine == "#")
            {

                enemies.Add(enemy);
            }

            if (reader.EndOfStream)
            {
                break;
            }

            if (enemy == "#")
            {
                previousLine = "#";
            }
        }
        return enemies;
    }
}
