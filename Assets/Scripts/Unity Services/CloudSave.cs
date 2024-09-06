using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSave
{
    public async Task Initialise()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();
        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "SaveData" });
        if (data.TryGetValue("SaveData", out var pair))
        {
            GameManager.Instance.SaveData = JsonConvert.DeserializeObject<SaveData>(pair.Value.GetAs<string>(), settings);
        }
        else
        {
            GameManager.Instance.SaveData = new SaveData();
            Save();
        }
    }

    public async void Save()
    {
        var dataToSave = new Dictionary<string, object>
        {
            { "SaveData", GameManager.Instance.SaveData }
        };

        // Save the data to the cloud
        await Call(CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave));
    }

    private async Task Call(Task action)
    {
        try
        {
            await action;
        }
        catch (CloudSaveValidationException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
}
