using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChallengeCalendar : MonoBehaviour
{
    [SerializeField] private TMP_Text monthName;
    [SerializeField] private TMP_Text monthScore;
    [SerializeField] private CalendarDrawer calendarDrawer;

    public void ChangeMonth(int monthOffsetFromCurrent)
    {
        var selectedMonth = DateTime.Today.AddMonths(monthOffsetFromCurrent);
        var daysInMonth = DateTime.DaysInMonth(selectedMonth.Year, selectedMonth.Month);
        
        monthName.text = selectedMonth.ToString("MMMM yyyy");
        monthScore.text = $"{Random.Range(0, daysInMonth + 1)}/{daysInMonth}";
        calendarDrawer.DrawMonth(GetFirstDayOfMonth(selectedMonth), monthOffsetFromCurrent == 0 ? selectedMonth.Day : daysInMonth);
    }

    private DateTime GetFirstDayOfMonth(DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, 1);
}
