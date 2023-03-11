using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textCubeLeft;
    [SerializeField] private TextMeshProUGUI _textCubeRight;
    [SerializeField] private Image _image;

    [SerializeField] private TextMeshProUGUI _textFloor;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderFill;


    public void Initialize(Material materialFrom)
    {
        _sliderFill.color = materialFrom.color;
    }

    public void UpdateView(int resourceNeed, int resourceCreate, int leftResource)
    {
        if (resourceNeed == 0)
        {
            _textFloor.text = "";
            _textCubeLeft.text = "";
            _image.color = Color.green;
        }
        else
        {
            _image.color = Color.yellow;
            _textFloor.text = resourceNeed.ToString();
            _textCubeLeft.text = leftResource.ToString();
        }

        _textCubeRight.text = resourceCreate.ToString();
    }

    public void ZeroSlider()
    {
        _slider.value = 0;
    }

    public void UpdateViewSlider(float maxTime, float fixedTime)
    {
        _slider.value = fixedTime / maxTime;
    }
}
