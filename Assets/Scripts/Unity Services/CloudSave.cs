using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;

public class CloudSave
{
    public async void Save(string key, object data)
    {
        var dataToSave = new Dictionary<string, object>
        {
            { key, data }
        };

        // Save the data to the cloud
        await Call(CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave));
    }

    public async Task<T> Load<T>(string key) where T : class, new()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings();

        var data = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
        if (data.TryGetValue(key, out var pair))
        {
            return JsonConvert.DeserializeObject<T>(pair.Value.GetAs<string>(), settings);
        }
        else
        {
            Save(key, new T());
            return new T();
        }
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
