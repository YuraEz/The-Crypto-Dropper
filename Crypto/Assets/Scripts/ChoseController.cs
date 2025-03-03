using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoseController : MonoBehaviour
{
    [SerializeField] private List<Button> btnsList;
    [SerializeField] private Sprite notActiveBtn;
    [SerializeField] private Sprite activeBtn;

    [SerializeField] private Color color;

    [SerializeField] private bool isMultiSelectEnabled = false; // ����� �������������� ������
    private List<Button> activeButtons = new List<Button>();    // ������ �������� ������

    private void Start()
    {
        foreach (var btn in btnsList)
        {
            SetButtonInactive(btn);
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    private void OnButtonClick(Button clickedButton)
    {
        if (isMultiSelectEnabled)
        {
            // � ������ �������������� ������ ��������� ��� ������� ������ �� ��������
            if (activeButtons.Contains(clickedButton))
            {
                SetButtonInactive(clickedButton);
                activeButtons.Remove(clickedButton);
            }
            else
            {
                SetButtonActive(clickedButton);
                activeButtons.Add(clickedButton);
            }
        }
        else
        {
            // � ������� ������ ������������ ���������� �������� ������
            foreach (var button in activeButtons)
            {
                SetButtonInactive(button);
            }
            activeButtons.Clear();

            // ���������� ������� ������
            if (!activeButtons.Contains(clickedButton))
            {
                SetButtonActive(clickedButton);
                activeButtons.Add(clickedButton);
            }
        }
    }

    private void SetButtonActive(Button button)
    {
        button.image.color = color;
        //button.image.sprite = activeBtn;
    }

    private void SetButtonInactive(Button button)
    {
        button.image.color = Color.white;
       // button.image.sprite = notActiveBtn;
    }
} 
