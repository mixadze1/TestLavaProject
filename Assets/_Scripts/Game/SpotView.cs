using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpotView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountSpotGiveText;
    [SerializeField] private TextMeshProUGUI _amountSpotGetText;
    [SerializeField] private Image _image;

    [SerializeField] private TextMeshProUGUI _amountStillGetText;
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
            _amountStillGetText.text = "";
            _image.color = Color.green;
        }
        else
        {
            _image.color = Color.yellow;
            _amountStillGetText.text = resourceNeed.ToString();
        }

        _amountSpotGetText.text = resourceCreate.ToString();
        _amountSpotGiveText.text = leftResource.ToString();
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
