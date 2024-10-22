using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConfigDirectoryManager : MonoBehaviour
{
    [SerializeField] private string icoFileUrl = "https://raw.githubusercontent.com/YoavTC/email-notifier/refs/heads/main/Assets/Resources/icon.ico";
    
    void Awake()
    {
        //Icon file
        string iconPath = Application.persistentDataPath + "/icon.ico";
        if (!File.Exists(iconPath))
        {
            StartCoroutine(GetIconFile());
        }
        
        //Config file
        string configPath = Application.persistentDataPath + "/config.yaml";
        if (!File.Exists(configPath))
        {
            string defaultConfigFileData = Resources.Load<TextAsset>("config").text;
            File.WriteAllText(configPath, defaultConfigFileData);
        }
    }

    private IEnumerator GetIconFile()
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
}
