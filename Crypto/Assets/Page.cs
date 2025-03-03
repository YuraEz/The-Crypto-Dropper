using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class Page : MonoBehaviour
{
    [SerializeField] private Button adBtn;
    [SerializeField] private Button adBtn2;
    [SerializeField] private GameObject afterClick;
    [SerializeField] private TextMeshProUGUI tmprooo;
    private float allPrice;

    [Space]
    [SerializeField] private List<Sprite> spritesInHeader;
    [SerializeField] private Image img;
    [SerializeField] private Button btn1;
    [SerializeField] private Button btn2;
    [SerializeField] private Button btn3;

    [Space]
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_InputField amount;
    [SerializeField] private List<GameObject> gmobject;

    private int indess;

    [SerializeField] private GameObject nothaveonj;
    [SerializeField] private TextMeshProUGUI tmall;
    private float allhabe;
    

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

    private void Start()
    {

        foreach (GameObject go in gmobject)
        {
            go.SetActive(false);
        }

        afterClick.SetActive(false);
        adBtn.onClick.AddListener(() =>
        {
            afterClick.SetActive(true);
        });

        adBtn2.onClick.AddListener(() =>
        {
            afterClick.SetActive(false);

            //foreach (GameObject go in gmobject)
            //{
            //    go.SetActive(false);
            //}
            gmobject[indess].SetActive(true);
            allhabe += PlayerPrefs.GetFloat(indess.ToString(), 0) * float.Parse(amount.text);
            tmall.text = $"$ {allhabe}";
            nothaveonj.SetActive(false);

            gmobject[indess].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{amount.text} {dropdown.options[dropdown.value].text}";
            gmobject[indess].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"$ {PlayerPrefs.GetFloat(indess.ToString(), 0) * float.Parse(amount.text)}";
        });

        btn1.onClick.AddListener(() =>
        {
            img.sprite = spritesInHeader[0];
        });
        btn2.onClick.AddListener(() =>
        {
            img.sprite = spritesInHeader[1];
        });
        btn3.onClick.AddListener(() =>
        {
            img.sprite = spritesInHeader[2];
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
}
