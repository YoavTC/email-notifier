using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CalendarParser : Singleton<CalendarParser>
{
    private string iCalKey = "";

    public void DownloadCalendar()
    {
        StartCoroutine(DownloadCalendarRoutine());
    }
    
    private IEnumerator DownloadCalendarRoutine()
    {
        iCalKey = ConfigDirectoryManager.Instance.currentConfigInstance.calendarKey;

        using (UnityWebRequest request = UnityWebRequest.Get(iCalKey))
        {
            yield return request.SendWebRequest();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                byte[] iCalData = request.downloadHandler.data;
                File.WriteAllBytes(Application.persistentDataPath + "/ical.ics", iCalData);
                
            } else {
                Debug.Log(iCalKey);
                ErrorMessenger.DisplayMessage(request.error);
            }
        }
    }
    
    void Update()
    {
        
    }
}
