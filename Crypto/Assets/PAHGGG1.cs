using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PAHGGG1 : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnObj;
    [SerializeField] private Button btm;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField amount;

    [SerializeField] private TextMeshProUGUI textMeshPro;
    private float allhabe;
    public GameObject showd;

    private int indess;
    private void Start()
    {
        showd.SetActive(false);
        foreach (GameObject go in spawnObj)
        {
            go.SetActive(false);
        }
        btm.onClick.AddListener(() =>
        {
            showd.SetActive(true);

            //foreach (GameObject go in gmobject)
            //{
            //    go.SetActive(false);
            //}
            spawnObj[indess].SetActive(true);
            allhabe += PlayerPrefs.GetFloat(indess.ToString(), 0) * float.Parse(amount.text);
            textMeshPro.text = $"$ {allhabe}";
            //  nothaveonj.SetActive(false);
            print(amount);
            spawnObj[indess].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = amount.text;
            spawnObj[indess].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"$ {PlayerPrefs.GetFloat(indess.ToString() + indess.ToString(), 0) * float.Parse(amount.text)}";
        });

        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        Debug.Log("Выбранный индекс: " + index);
        indess = index;
        Debug.Log("Выбранный текст: " + dropdown.options[index].text);
    }

    public int GetSelectedIndex()
    {
        return dropdown.value; // Возвращает текущий индекс
    }

    private void Awake()
    {
        amount.onValueChanged.AddListener(ValidateInput);
    }

    private void ValidateInput(string input)
    {
        string filtered = Regex.Replace(input, @"\D", ""); // Убираем все, кроме цифр
        amount.text = filtered;
        amount.caretPosition = amount.text.Length; // Перемещаем курсор в конец
    }
}
