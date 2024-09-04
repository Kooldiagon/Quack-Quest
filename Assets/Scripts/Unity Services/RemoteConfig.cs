using UnityEngine;
using Unity.Services.RemoteConfig;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.IO;

public class RemoteConfig
{
    // User attributes for targeting
    public struct userAttributes 
    {
        public string countryCode, languageCode;
    }

    // App attributes for targeting
    public struct appAttributes 
    {
        public string appVersion, platform, osVersion, deviceModel;
    }

    // Initialising remote config
    public async Task Initialise() 
    {
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteConfig(ConfigResponse configResponse)
    {
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                Debug.LogWarning("No settings loaded this session and no local cache file exists; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log("New settings loaded this session; update values accordingly.");
                break;
        }
    }

    public T Json<T>(string key) where T : class
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        string jsonString = RemoteConfigService.Instance.appConfig.GetJson(key, File.ReadAllText($"{Application.dataPath}/Data/Default Configs/{key}.json"));
        try
        {
            return JsonConvert.DeserializeObject<T>(jsonString, settings);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to parse JSON data for key '{key}': {ex.Message}");
        }
        Debug.LogWarning($"Key '{key}' not found in remote config.");
        return null;
    }
}