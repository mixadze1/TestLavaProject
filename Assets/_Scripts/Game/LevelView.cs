using TMPro;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelView;

    public void Initialize(int level)
    {
        _levelView.text = "Level " + (level+1); // in collection first element is 0.
    }
}
