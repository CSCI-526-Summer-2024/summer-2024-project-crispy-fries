using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public struct LevelStars
{
    public bool star1;
    public bool star2;
    public bool star3;

    public LevelStars(bool s1, bool s2, bool s3)
    {
        star1 = s1;
        star2 = s2;
        star3 = s3;
    }

    public LevelStars Union(LevelStars other)
    {
        return new LevelStars(star1 || other.star1, star2 || other.star2, star3 || other.star3);
    }
}
public class StarManager : MonoBehaviour
{
    public static StarManager instance;

    public Dictionary<string, LevelStars> levelStarsDict = new Dictionary<string, LevelStars>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectStar(string levelName, LevelStars newStars)
    {
        if (!levelStarsDict.ContainsKey(levelName))
        {
            levelStarsDict[levelName] = new LevelStars(false, false, false);
        }

        levelStarsDict[levelName] = levelStarsDict[levelName].Union(newStars);
            Debug.Log(levelStarsDict[levelName].star1);

    }
    // Update is called once per frame
    public LevelStars GetLevelStars(string levelName)
    {
        if (levelStarsDict.ContainsKey(levelName))
        {
            return levelStarsDict[levelName];
        }
        else
        {
            return new LevelStars(false, false, false);
        }
    }
}
