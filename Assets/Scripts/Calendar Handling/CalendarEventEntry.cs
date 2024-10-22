using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarEventEntry : MonoBehaviour
{
    private CalendarEvent eventInstance;
    
    [SerializeField] private Image labelImage;
    [SerializeField] private TMP_Text dayOfWeekLabelDisplay;
    [SerializeField] private TMP_Text titleDisplay;
    [SerializeField] private TMP_Text dateDisplay;

    [SerializeField] private string wholeDayDateFormat;
    [SerializeField] private string betweenDatesDateFormat;

    public void Init(CalendarEvent eventInstance)
    {
        this.eventInstance = eventInstance;

        labelImage.color = eventInstance.labelColour;
        dayOfWeekLabelDisplay.text = eventInstance.dayOfWeekLabel.Substring(0, 2);
        titleDisplay.text = eventInstance.title;

        dateDisplay.text = "";
        if (eventInstance.wholeDay)
        {
            dateDisplay.text = String.Format(wholeDayDateFormat,
                eventInstance.startTime.ToString("MM/dd/yy"),
                eventInstance.startTime.ToString("t"),
                eventInstance.endTime.ToString("t"));
        } else {
            dateDisplay.text = String.Format(betweenDatesDateFormat,
                eventInstance.startTime.ToString("MM/dd/yy"),
                eventInstance.endTime.ToString("MM/dd/yy"),
                eventInstance.startTime.ToString("t"),
                eventInstance.endTime.ToString("t"));
        }
    }
}