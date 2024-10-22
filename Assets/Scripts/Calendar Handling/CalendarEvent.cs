using System;
using UnityEngine;

[Serializable]
public class CalendarEvent
{
    public string title;
    public Color labelColour;
    public string dayOfWeekLabel;
    
    public DateTime startTime;
    public DateTime endTime;
    public bool wholeDay;

    public CalendarEvent(string title, Color labelColour, DateTime startTime, DateTime endTime)
    {
        this.endTime = endTime;
        wholeDay = false;
        ConstructorBase(title, labelColour, startTime);
    }
    
    public CalendarEvent(string title, Color labelColour, DateTime startTime)
    {
        wholeDay = true;
        ConstructorBase(title, labelColour, startTime);
    }

    private void ConstructorBase(string title, Color labelColour, DateTime startTime)
    {
        this.title = title;
        this.labelColour = labelColour;
        this.startTime = startTime;

        dayOfWeekLabel = startTime.DayOfWeek.ToString();
    }
}