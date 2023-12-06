using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BiomeCard : MonoBehaviour
{
    private static BiomeCard _instance = null;

    [SerializeField] private TextMeshProUGUI _biomeText;

    [SerializeField] private TextMeshProUGUI _elevationText;
    [SerializeField] private TextMeshProUGUI _temperatureText;
    [SerializeField] private TextMeshProUGUI _rainText;
    [SerializeField] private TextMeshProUGUI _vegetationText;

    private void Start()
    {
        _instance = this;
        transform.GetComponent<Window>().CloseWindow();
    }

    public void OpenBiomeCard(TileData data)
    {

        _biomeText.text = data.BiomeType.name;
        _elevationText.text = "Elevation: " + data.Elevation.ToString("F2") + "m";
        _temperatureText.text = "Temperature: " + MapGen.Instance.TemperatureMapper.Evaluate(data.Temperature).ToString("F2") + "°C";
        _rainText.text = "Rain: " + data.Rain.ToString("F2") + "mm";
        _vegetationText.text = "Vegetation: " + data.Vegetation;

        transform.GetComponent<Window>().OpenWindow();
    }

    public static BiomeCard Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BiomeCard)) as BiomeCard;
            }

            if (_instance == null)
            {
                var obj = new GameObject("BiomeCard");
                _instance = obj.AddComponent<BiomeCard>();
            }

            return _instance;
        }
    }
}
