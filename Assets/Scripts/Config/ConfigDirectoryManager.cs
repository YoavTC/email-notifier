using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class ConfigDirectoryManager : Singleton<ConfigDirectoryManager>
{
    [SerializeField] private string configFileName = "config.yaml";
    [SerializeField] private string iconFileName = "icon.ico";
    [SerializeField] private string icoFileUrl = "https://raw.githubusercontent.com/YoavTC/email-notifier/refs/heads/main/Assets/Resources/icon.ico";

    [SerializeField] private TMP_Text debugText;

    public ConfigInstance currentConfigInstance;
    
    IEnumerator Start()
    {
        yield return new WaitUntil(HandleMissingFiles());
        yield return new WaitUntil(RetrieveSettings());
        
        CalendarParser.Instance.DownloadCalendar();
    }

    private Func<bool> RetrieveSettings()
    {
        string configPath = GetConfigPath();
        string configContent = File.ReadAllText(configPath);
        
        //debugging
        debugText.text = configContent;

        IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();
        currentConfigInstance = deserializer.Deserialize<ConfigInstance>(configContent);

        return () => true;
    }
    
    private Func<bool> HandleMissingFiles()
    {
        try
        {
            //Icon file
            string iconPath = GetIconPath();
            if (!File.Exists(iconPath))
            {
                StartCoroutine(SaveIconFile());
            }
        
            //Config file
            string configPath = GetConfigPath();
            if (!File.Exists(configPath))
            {
                string defaultConfigFileData = Resources.Load<TextAsset>("config").text;
                File.WriteAllText(configPath, defaultConfigFileData);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return () => false;
        }
        return () => true;
    }

    private IEnumerator SaveIconFile()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(icoFileUrl))
        {
            yield return request.SendWebRequest();
                
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error downloading ICO file: {request.error}");
            }
            else
            {
                // Get the downloaded data as a byte array
                byte[] icoData = request.downloadHandler.data;

                // Define the destination path
                string destinationPath = Application.persistentDataPath + "/icon.ico";

                // Save the ICO file to persistentDataPath
                File.WriteAllBytes(destinationPath, icoData);
                Debug.Log($"ICO file downloaded and saved to: {destinationPath}");
            }
        }
    }
    
    private string GetIconPath() => $"{Application.persistentDataPath}/{iconFileName}";
    private string GetConfigPath() => $"{Application.persistentDataPath}/{configFileName}";
}
