using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GetCrypto : MonoBehaviour
{
    public TextMeshProUGUI priceText; // Основной UI-элемент для вывода всех цен

    [SerializeField] private List<TextMeshProUGUI> btcTexts;
    [SerializeField] private List<TextMeshProUGUI> ethTexts;
    [SerializeField] private List<TextMeshProUGUI> adaTexts;
    [SerializeField] private List<TextMeshProUGUI> solTexts;
    [SerializeField] private List<TextMeshProUGUI> ltcTexts;
    [SerializeField] private List<TextMeshProUGUI> trxTexts;
    [SerializeField] private List<TextMeshProUGUI> bnbTexts;

    private string apiUrl = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,cardano,solana,litecoin,tron,binancecoin&vs_currencies=usd";

    void Start()
    {
        StartCoroutine(GetCryptoPrices());
    }

    IEnumerator GetCryptoPrices()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = request.downloadHandler.text;
                JObject json = JObject.Parse(jsonResult);

                float btcPrice = json["bitcoin"]["usd"].Value<float>();
                float ethPrice = json["ethereum"]["usd"].Value<float>();
                float adaPrice = json["cardano"]["usd"].Value<float>();
                float solPrice = json["solana"]["usd"].Value<float>();
                float ltcPrice = json["litecoin"]["usd"].Value<float>();
                float trxPrice = json["tron"]["usd"].Value<float>();
                float bnbPrice = json["binancecoin"]["usd"].Value<float>();

                // Заполняем текстовые элементы
                UpdateTextElements(btcTexts, btcPrice);
                UpdateTextElements(ethTexts, ethPrice);
                UpdateTextElements(adaTexts, adaPrice);
                UpdateTextElements(solTexts, solPrice);
                PlayerPrefs.SetFloat("0", solPrice);
                PlayerPrefs.SetFloat("1", ethPrice);
                PlayerPrefs.SetFloat("2", adaPrice);

                PlayerPrefs.SetFloat("00", btcPrice);
                PlayerPrefs.SetFloat("11", ethPrice);

                UpdateTextElements(ltcTexts, ltcPrice);
                UpdateTextElements(trxTexts, trxPrice);
                UpdateTextElements(bnbTexts, bnbPrice);

                // Обновляем основной UI-текст
                priceText.text = $"BTC: ${btcPrice}\nETH: ${ethPrice}\nADA: ${adaPrice}\nSOL: ${solPrice}\nLTC: ${ltcPrice}\nTRX: ${trxPrice}\nBNB: ${bnbPrice}";
            }
            else
            {
                priceText.text = "Ошибка загрузки";
                Debug.LogError("Ошибка запроса: " + request.error);
            }
        }
    }

    // Метод для обновления списка текстов
    private void UpdateTextElements(List<TextMeshProUGUI> textElements, float price)
    {
        foreach (TextMeshProUGUI textElement in textElements)
        {
            textElement.text = $"$ {price}";
        }
    }
}
