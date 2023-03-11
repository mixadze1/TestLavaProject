using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelView;

    public void Initialize(int level)
    {
        _levelView.text = "Level " + (level+1);
    }
}
