using UnityEngine;
using UnityEngine.UI;

public class SliderShipsController : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _sliderText;

    public float SliderShipValue => _slider.value;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _slider.value++;
        }
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _slider.value--;
        }
    }
    
    public void SetPercent()
    {
        var percent = _slider.value * 10;//todo В константу!
        _sliderText.text = percent + "%";
    }
}
