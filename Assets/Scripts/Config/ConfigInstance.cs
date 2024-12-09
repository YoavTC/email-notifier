using System;

[Serializable]
public class ConfigInstance
{
    public int monitorIndex;
    public int monitorResolutionWidth;
    public int monitorResolutionHeight;
    public string calendarKey;
    
    //Empty constructor for YamlDotNet
    public ConfigInstance() {}
    
    public ConfigInstance(int monitorIndex, int monitorResolutionWidth, int monitorResolutionHeight, string calendarKey)
    {
        this.monitorIndex = monitorIndex;
        this.monitorResolutionWidth = monitorResolutionWidth;
        this.monitorResolutionHeight = monitorResolutionHeight;
        this.calendarKey = calendarKey;
    }
}