using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;

    public Level CreateLevel(int level)
    {
        try
        {
            return Instantiate(_levels[level]);
        }
        catch
        {
            Debug.Log("Levels are over! Enabled first level.");
            return Instantiate(_levels[0]);
        }
       
    }
}
