using System;
using NaughtyAttributes;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{
    [SerializeField] private RectTransform entriesContainer;
    [SerializeField] private CalendarEventEntry eventEntryPrefab;

    private void Start()
    {
        AddEvent(new CalendarEvent(
            "Test event 2",
            Color.red, 
            DateTime.Now.AddYears(-1).AddDays(1).AddMinutes(50)));
        
        AddEvent(new CalendarEvent(
            "Test event 2",
            Color.green, 
            DateTime.Now,
            DateTime.Now.AddDays(3)));
        
        AddEvent(new CalendarEvent(
            "Test event 2",
            Color.blue, 
            DateTime.Now.AddYears(1).AddDays(1).AddMinutes(50)));
    }

    public void AddEvent(CalendarEvent newEvent)
    {
        Instantiate(eventEntryPrefab, entriesContainer).Init(newEvent);
    }
}