using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
//using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Calendar : MonoBehaviour
{
    public GameObject dayPrefab; // Префаб для дня
    public Transform gridParent; // Grid Layout Group для дней
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI yearText;
    public Button nextMonthButton, prevMonthButton; // Кнопки для переключения месяцев
    public Button nextYearButton, prevYearButton; // Кнопки для переключения годов

    [Space]
    public Color prevMonthColor;
    public Color thisYearColor;
    public Color todayColor;

    [Space]
    public GameObject Panel;
    public Button backBtn;

    private DateTime currentDate; // Текущая дата
    private List<GameObject> dayObjects = new List<GameObject>(); // Список для хранения созданных дней

    public NoteLoader NoteContainer;

    public bool isTr;

    public void Back()
    {
        NoteContainer.SpawnNotes(PlayerPrefs.GetInt("noteId", 0));
        NoteContainer.gameObject.SetActive(false);

        Invoke(nameof(backk), 0.2f);

    }

    public void backk()
    {
        NoteContainer.gameObject.SetActive(true);
        Panel.SetActive(false);
    }

    public Button Add;
    public Button Remove;

    private void Start()
    {
        DateTime fissrstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        Add.onClick.AddListener(() => OnDayClick(fissrstDayOfMonth, Add));
        Remove.onClick.AddListener(() => OnDayClick(fissrstDayOfMonth, Remove));

        if (Panel) Panel.SetActive(false);

        if (backBtn) backBtn.onClick.AddListener(() =>
        {
             Back();
            UI.Instance.ChangeScreen("premium");
            UI.Instance.ChangeScreen("screen_2_1");
        });

        currentDate = DateTime.Now; // Устанавливаем текущую дату
        UpdateCalendar(); // Обновляем календарь

        // Привязываем кнопки
        nextMonthButton.onClick.AddListener(() => ChangeMonth(1));
        prevMonthButton.onClick.AddListener(() => ChangeMonth(-1));
        if (nextYearButton) nextYearButton.onClick.AddListener(() => ChangeYear(1));
        if (prevYearButton) prevYearButton.onClick.AddListener(() => ChangeYear(-1));
    }

    // Обновляем календарь
    private void UpdateCalendar()
    {
        // Очищаем старые дни
        foreach (var day in dayObjects)
        {
            Destroy(day);
        }
        dayObjects.Clear();

        // Устанавливаем название месяца и года в формате "/ Month Year"
        monthText.text = $"{currentDate.ToString("MMMM", CultureInfo.InvariantCulture)}";
        yearText.text = $"{currentDate.ToString("yyyy", CultureInfo.InvariantCulture)}";

        // Получаем дату первого дня месяца
        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        // Определяем, на какой день недели приходится первый день месяца
        int startDay = (int)firstDayOfMonth.DayOfWeek;
        if (startDay == 0) startDay = 7; // Перемещаем воскресенье в конец недели

        // Получаем количество дней в месяце
        int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

        // Создаем пустые ячейки перед первым днем месяца
        //  for (int i = 1; i < startDay; i++)
        //  {
        //      CreateEmptyDay();
        //  }

        DateTime prevMonthDate = currentDate.AddMonths(-1);
        int prevMonthDays = DateTime.DaysInMonth(prevMonthDate.Year, prevMonthDate.Month);

        // Создаем пустые ячейки перед первым днем месяца
        for (int i = startDay - 1; i > 0; i--)
        {
            CreateEmptyDay(prevMonthDays - (i - 1));
        }

        // Создаем дни месяца
        for (int day = 1; day <= daysInMonth; day++)
        {
            CreateDay(day);
        }
    }

    // Создаем пустой день
    private void CreateEmptyDay(int dayNumber)
    {
        GameObject emptyDay = Instantiate(dayPrefab, gridParent);
        TextMeshProUGUI dayText = emptyDay.GetComponentInChildren<TextMeshProUGUI>();

        // Устанавливаем текст и цвет
        dayText.text = dayNumber.ToString();
        dayText.color = prevMonthColor;

        dayObjects.Add(emptyDay);
    }

    // Создаем день с номером
    private void CreateDay(int day)
    {
        GameObject dayObj = Instantiate(dayPrefab, gridParent);
        TextMeshProUGUI dayText = dayObj.GetComponentInChildren<TextMeshProUGUI>();
        dayText.gameObject.SetActive(true);
        dayText.text = day.ToString(); // Устанавливаем текст дня

        Button dayButton = dayObj.GetComponent<Button>();
        DateTime dayDate = new DateTime(currentDate.Year, currentDate.Month, day);
        dayButton.onClick.AddListener(() => OnDayClick(dayDate, dayButton)); // Добавляем обработчик клика

        // Проверяем, является ли день сегодняшним
        if (dayDate.Date == DateTime.Now.Date)
        {
            dayText.color = todayColor; // Меняем цвет текста
            if (dayObj.transform.childCount > 1)
            {
                dayObj.transform.GetChild(1).gameObject.SetActive(true); // Активируем второй дочерний объект
            }
        }
        else
        {
            dayText.color = thisYearColor; // Цвет обычных дней
        }

        dayObjects.Add(dayObj);
    }

    // Событие при клике на день
    public TextMeshProUGUI tx;

    private void OnDayClick(DateTime dayDate, Button self)
    {


        if (Panel) Panel.SetActive(true);

        Debug.Log(dayDate.ToString("MMMM d, yyyy"));

        Debug.Log("Выбрана дата: " + dayDate.ToString("dd.MM.yyyy"));

        // Получаем первый дочерний объект с компонентом Image
        Transform firstChild = self.transform.GetChild(0);
        if (firstChild)
        {
            // Проверяем, есть ли у первого ребенка компонент Image и активируем его
            Image childImage = firstChild.GetComponent<Image>();
            if (childImage)
            {
                if (!Panel) childImage.gameObject.SetActive(true); // Активируем объект
            }
            else
            {
                Debug.LogWarning("Image не найден у первого дочернего объекта.");
            }

            // Ищем дочерний объект у первого ребенка, который содержит TextMeshProUGUI
            Transform textChild = firstChild.GetChild(0);
            if (textChild)
            {
                TextMeshProUGUI childText = textChild.GetComponent<TextMeshProUGUI>();
                if (childText)
                {
                    if (!Panel) childText.text = dayDate.Day.ToString(); // Устанавливаем текущую дату
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI не найден у дочернего объекта.");
                }
            }
            else
            {
                Debug.LogWarning("У первого ребенка кнопки нет дочернего объекта.");
            }
        }
        else
        {
            Debug.LogWarning("Кнопка не имеет дочерних объектов.");
        }

        // Выводим выбранную дату в формате "June 7, 2024"
        if (self.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI selectedDateText)
        {
           if (tx)  tx.text = dayDate.ToString("MMMM d, yyyy");
            // selectedDateText.text = "Вы выбрали: " + dayDate.ToString("MMMM d, yyyy");
        }

        Debug.Log("Выбрана дата: " + dayDate.ToString("dd.MM.yyyy"));
        // Здесь можно открыть форму для создания заметки
    }

    // Изменяем месяц
    private void ChangeMonth(int delta)
    {
        currentDate = currentDate.AddMonths(delta);
        UpdateCalendar();
    }

    // Изменяем год
    private void ChangeYear(int delta)
    {
        currentDate = currentDate.AddYears(delta);
        UpdateCalendar();
    }
}
