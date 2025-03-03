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
    public GameObject dayPrefab; // ������ ��� ���
    public Transform gridParent; // Grid Layout Group ��� ����
    public TextMeshProUGUI monthText;
    public TextMeshProUGUI yearText;
    public Button nextMonthButton, prevMonthButton; // ������ ��� ������������ �������
    public Button nextYearButton, prevYearButton; // ������ ��� ������������ �����

    [Space]
    public Color prevMonthColor;
    public Color thisYearColor;
    public Color todayColor;

    [Space]
    public GameObject Panel;
    public Button backBtn;

    private DateTime currentDate; // ������� ����
    private List<GameObject> dayObjects = new List<GameObject>(); // ������ ��� �������� ��������� ����

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

        currentDate = DateTime.Now; // ������������� ������� ����
        UpdateCalendar(); // ��������� ���������

        // ����������� ������
        nextMonthButton.onClick.AddListener(() => ChangeMonth(1));
        prevMonthButton.onClick.AddListener(() => ChangeMonth(-1));
        if (nextYearButton) nextYearButton.onClick.AddListener(() => ChangeYear(1));
        if (prevYearButton) prevYearButton.onClick.AddListener(() => ChangeYear(-1));
    }

    // ��������� ���������
    private void UpdateCalendar()
    {
        // ������� ������ ���
        foreach (var day in dayObjects)
        {
            Destroy(day);
        }
        dayObjects.Clear();

        // ������������� �������� ������ � ���� � ������� "/ Month Year"
        monthText.text = $"{currentDate.ToString("MMMM", CultureInfo.InvariantCulture)}";
        yearText.text = $"{currentDate.ToString("yyyy", CultureInfo.InvariantCulture)}";

        // �������� ���� ������� ��� ������
        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        // ����������, �� ����� ���� ������ ���������� ������ ���� ������
        int startDay = (int)firstDayOfMonth.DayOfWeek;
        if (startDay == 0) startDay = 7; // ���������� ����������� � ����� ������

        // �������� ���������� ���� � ������
        int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

        // ������� ������ ������ ����� ������ ���� ������
        //  for (int i = 1; i < startDay; i++)
        //  {
        //      CreateEmptyDay();
        //  }

        DateTime prevMonthDate = currentDate.AddMonths(-1);
        int prevMonthDays = DateTime.DaysInMonth(prevMonthDate.Year, prevMonthDate.Month);

        // ������� ������ ������ ����� ������ ���� ������
        for (int i = startDay - 1; i > 0; i--)
        {
            CreateEmptyDay(prevMonthDays - (i - 1));
        }

        // ������� ��� ������
        for (int day = 1; day <= daysInMonth; day++)
        {
            CreateDay(day);
        }
    }

    // ������� ������ ����
    private void CreateEmptyDay(int dayNumber)
    {
        GameObject emptyDay = Instantiate(dayPrefab, gridParent);
        TextMeshProUGUI dayText = emptyDay.GetComponentInChildren<TextMeshProUGUI>();

        // ������������� ����� � ����
        dayText.text = dayNumber.ToString();
        dayText.color = prevMonthColor;

        dayObjects.Add(emptyDay);
    }

    // ������� ���� � �������
    private void CreateDay(int day)
    {
        GameObject dayObj = Instantiate(dayPrefab, gridParent);
        TextMeshProUGUI dayText = dayObj.GetComponentInChildren<TextMeshProUGUI>();
        dayText.gameObject.SetActive(true);
        dayText.text = day.ToString(); // ������������� ����� ���

        Button dayButton = dayObj.GetComponent<Button>();
        DateTime dayDate = new DateTime(currentDate.Year, currentDate.Month, day);
        dayButton.onClick.AddListener(() => OnDayClick(dayDate, dayButton)); // ��������� ���������� �����

        // ���������, �������� �� ���� �����������
        if (dayDate.Date == DateTime.Now.Date)
        {
            dayText.color = todayColor; // ������ ���� ������
            if (dayObj.transform.childCount > 1)
            {
                dayObj.transform.GetChild(1).gameObject.SetActive(true); // ���������� ������ �������� ������
            }
        }
        else
        {
            dayText.color = thisYearColor; // ���� ������� ����
        }

        dayObjects.Add(dayObj);
    }

    // ������� ��� ����� �� ����
    public TextMeshProUGUI tx;

    private void OnDayClick(DateTime dayDate, Button self)
    {


        if (Panel) Panel.SetActive(true);

        Debug.Log(dayDate.ToString("MMMM d, yyyy"));

        Debug.Log("������� ����: " + dayDate.ToString("dd.MM.yyyy"));

        // �������� ������ �������� ������ � ����������� Image
        Transform firstChild = self.transform.GetChild(0);
        if (firstChild)
        {
            // ���������, ���� �� � ������� ������� ��������� Image � ���������� ���
            Image childImage = firstChild.GetComponent<Image>();
            if (childImage)
            {
                if (!Panel) childImage.gameObject.SetActive(true); // ���������� ������
            }
            else
            {
                Debug.LogWarning("Image �� ������ � ������� ��������� �������.");
            }

            // ���� �������� ������ � ������� �������, ������� �������� TextMeshProUGUI
            Transform textChild = firstChild.GetChild(0);
            if (textChild)
            {
                TextMeshProUGUI childText = textChild.GetComponent<TextMeshProUGUI>();
                if (childText)
                {
                    if (!Panel) childText.text = dayDate.Day.ToString(); // ������������� ������� ����
                }
                else
                {
                    Debug.LogWarning("TextMeshProUGUI �� ������ � ��������� �������.");
                }
            }
            else
            {
                Debug.LogWarning("� ������� ������� ������ ��� ��������� �������.");
            }
        }
        else
        {
            Debug.LogWarning("������ �� ����� �������� ��������.");
        }

        // ������� ��������� ���� � ������� "June 7, 2024"
        if (self.GetComponentInChildren<TextMeshProUGUI>() is TextMeshProUGUI selectedDateText)
        {
           if (tx)  tx.text = dayDate.ToString("MMMM d, yyyy");
            // selectedDateText.text = "�� �������: " + dayDate.ToString("MMMM d, yyyy");
        }

        Debug.Log("������� ����: " + dayDate.ToString("dd.MM.yyyy"));
        // ����� ����� ������� ����� ��� �������� �������
    }

    // �������� �����
    private void ChangeMonth(int delta)
    {
        currentDate = currentDate.AddMonths(delta);
        UpdateCalendar();
    }

    // �������� ���
    private void ChangeYear(int delta)
    {
        currentDate = currentDate.AddYears(delta);
        UpdateCalendar();
    }
}
