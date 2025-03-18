using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Calculator : MonoBehaviour
{
    public TMP_InputField transactionAmountInput;
    public TMP_Dropdown commissionTypeDropdown;
    public TMP_InputField platformInput;
    public TMP_InputField commissionPercentageInput;
    public TextMeshProUGUI result2Text;
    public TextMeshProUGUI resultText;

    [Space]
    [SerializeField] private GameObject pref;
    [SerializeField] private Transform spawnParen;

    [Space]
    [SerializeField] private GameObject clear;
    [SerializeField] private GameObject SureAdd;

    private void Start()
    {
        clear.SetActive(true);
    }

    public void CalculateCommission()
    {
        if (resultText == null || result2Text == null)
        {
            Debug.LogError("Text fields are not assigned!");
            return;
        }

        if (!float.TryParse(transactionAmountInput.text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float transactionAmount) || transactionAmount <= 0)
        {
            resultText.text = "Введите корректную сумму!";
            return;
        }

        if (!float.TryParse(commissionPercentageInput.text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float commissionPercentage) || commissionPercentage < 0)
        {
            resultText.text = "Введите корректный процент!";
            return;
        }

        string selectedCommissionType = commissionTypeDropdown.options[commissionTypeDropdown.value].text;
        float commission = 0f;

        switch (selectedCommissionType)
        {
            case "Fixed Fee":
                commission = commissionPercentage;
                break;
            case "Percentage Fee":
                commission = (transactionAmount * commissionPercentage) / 100f;
                break;
            case "Hybrid Fee":
                commission = (transactionAmount * commissionPercentage) / 100f + commissionPercentage;
                break;
            default:
                resultText.text = "Выберите корректный тип комиссии!";
                return;
        }

        float totalAmount = transactionAmount - commission;

        resultText.text = "$ " + commission.ToString("F2");
        result2Text.text = "$ " + totalAmount.ToString("F2");

        lastResultText = "$ " + commission.ToString("F2");
        lastTotalAmount = "$ " + totalAmount.ToString("F2");
    }

    private string lastResultText;
    private string lastTotalAmount;

    public void SpawnLst()
    {
        if (pref == null || spawnParen == null)
        {
            Debug.LogError("Prefab or Spawn Parent is not assigned!");
            return;
        }

        GameObject preff = Instantiate(pref, spawnParen);
        TextMeshProUGUI[] texts = preff.GetComponentsInChildren<TextMeshProUGUI>();

        if (texts.Length >= 2)
        {
            texts[1].text = lastResultText;
            texts[0].text = lastTotalAmount;
        }
        else
        {
            Debug.LogError("Prefab structure is incorrect!");
        }

        clear.SetActive(false);
    }
}
