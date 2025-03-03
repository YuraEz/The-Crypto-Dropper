using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Nore : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown wellb;
    [SerializeField] private TMP_InputField dish;
    [SerializeField] private TMP_InputField location;

    [Space]
    [SerializeField] private TMP_InputField startTime;
    [SerializeField] private TMP_InputField endTime;

    [Space]
    [SerializeField] private TMP_Dropdown food;
    [SerializeField] private TMP_Dropdown plants;
    [SerializeField] private TMP_Dropdown animals;
   // [SerializeField] private TMP_Dropdown way;

   // [SerializeField] private TMP_InputField noteName;
   // [SerializeField] private TMP_Dropdown idk;
   // [SerializeField] private TMP_InputField adress;
   // [SerializeField] private TMP_InputField price;
   // [SerializeField] private TMP_InputField desc;

  //  [SerializeField] private List<Star> stars;

    [SerializeField] private Button add;
    [SerializeField] private Calendar calendar;

    public Button btn1;
    public Button btn2;
        public Button btn3;

    private int starsAmount;
    private int index;

    private void OnTimeInputChanged(string input)
    {
        TMP_InputField field = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject?.GetComponent<TMP_InputField>();
        if (field == null) return;

        string filtered = System.Text.RegularExpressions.Regex.Replace(input, @"[^0-9]", ""); // Оставляем только цифры

        if (filtered.Length > 4)
            filtered = filtered.Substring(0, 4); // Ограничиваем до 4 цифр (часы + минуты)

        if (filtered.Length >= 3)
            filtered = filtered.Insert(2, ":"); // Вставляем двоеточие после двух цифр

        field.text = filtered;
        field.caretPosition = field.text.Length; // Ставим каретку в конец
    }

    private void Start()
    {
        startTime.onValueChanged.AddListener(OnTimeInputChanged);
        endTime.onValueChanged.AddListener(OnTimeInputChanged);
        //   foreach (Star star in stars)
        //   {
        //       star.onChange += OnChangeStars;
        //   }

        btn1.onClick.AddListener(() =>
        {
            index = 0;
            print("index" + index);
        });
        btn2.onClick.AddListener(() =>
        {
            index = 1;
            print("index" + index);
        });
            btn3.onClick.AddListener(() =>
            {
                index = 2;
                print("index" + index);
            });
     //
        add.onClick.AddListener(()=>{

            int noteIndex = PlayerPrefs.GetInt("noteId", 0);
            PlayerPrefs.SetString($"wellb{noteIndex}", wellb.options[wellb.value].text);

            PlayerPrefs.SetString($"dish{noteIndex}", dish.text);
            PlayerPrefs.SetString($"location{noteIndex}", location.text);

            PlayerPrefs.SetString($"start{noteIndex}", startTime.text); 
            PlayerPrefs.SetString($"end{noteIndex}", endTime.text);

            PlayerPrefs.SetString($"food{noteIndex}", food.options[food.value].text);
            PlayerPrefs.SetString($"plants{noteIndex}", plants.options[plants.value].text);
            PlayerPrefs.SetString($"animals{noteIndex}", animals.options[animals.value].text);

            PlayerPrefs.SetInt($"importance{noteIndex}", index);
            //   PlayerPrefs.SetString($"way{noteIndex}", way.options[way.value].text);

            // int noteIndex = PlayerPrefs.GetInt("noteId", 0);
            // PlayerPrefs.SetString($"name{noteIndex}", noteName.text);
            // PlayerPrefs.SetString($"adress{noteIndex}", adress.text);
            // PlayerPrefs.SetString($"price{noteIndex}", price.text);
            // PlayerPrefs.SetString($"desc{noteIndex}", desc.text);
            // PlayerPrefs.SetInt($"stars{noteIndex}", starsAmount);
            //
            //
            // PlayerPrefs.SetString($"hz{noteIndex}", idk.options[idk.value].text);


            //  noteName.text = "";
            //  idk.itemText.text = "Finnish bathhouse";
            //  adress.text = string.Empty;
            //  price.text = string.Empty;
            //  desc.text = string.Empty;

            //  ResetStars();


            PlayerPrefs.SetInt("noteId", noteIndex+1);

            //ServiceLocator.GetService<UIManager>().ChangeScreen("selection1");
            //ServiceLocator.GetService<UIManager>().ChangeScreen("selection31");

            calendar.Back();
            calendar.NoteContainer.gameObject.SetActive(false);
            calendar.NoteContainer.gameObject.SetActive(true);
        });
    }

    private void OnChangeStars(int amount)
    {
        SetStars(amount);
    }

    private void SetStars(int amount)
    {
        starsAmount = amount;
     //   for (int i = 0; i < stars.Count; i++)
     //   {
     //       if (i < amount)
     //       {
     //           stars[i].on.gameObject.SetActive(true);
     //       }
     //       else
     //       {
     //           stars[i].on.gameObject.SetActive(false);
     //       }
     //   }
    }

    private void ResetStars()
    {
    //    foreach (Star star in stars)
    //    {
    //        star.on.gameObject.SetActive(false);
    //    }
    }
}
