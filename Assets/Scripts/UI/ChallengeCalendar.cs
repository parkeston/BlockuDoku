using System;
using TMPro;
using UnityEngine;

public class ChallengeCalendar : MonoBehaviour
{
    [SerializeField] private TMP_Text monthName;
    [SerializeField] private TMP_Text monthScore;
    [SerializeField] private CalendarDrawer calendarDrawer;

    private DateTime selectedMonth;
    
    public void ChangeMonth(MonthChallengeSet monthChallengeSet)
    {
        (int month, int year) = monthChallengeSet.Date;
        var today = DateTime.Today;
        int challengeCurrentDay = (today.Month == month && today.Year == year) ? today.Day : DateTime.DaysInMonth(year, month);
        selectedMonth = new DateTime(year, month, 1);
        
        monthName.text = selectedMonth.ToString("MMMM yyyy");
        monthScore.text = monthChallengeSet.GetProgressString();
        calendarDrawer.DrawMonth(selectedMonth, challengeCurrentDay,monthChallengeSet.GetCompletionState());
    }

    public DateTime GetSelectedDate() =>
        new DateTime(selectedMonth.Year, selectedMonth.Month, calendarDrawer.SelectedDay);
}
