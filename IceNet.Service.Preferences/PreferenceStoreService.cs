using System.Text.Json;

namespace IceNet.Service.Preferences;

public class PreferenceStoreService : IPreferenceStoreService
{
    // <summary>
    /// Store an element using any kind of key (if it doesnt exist)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(string key, object value)
    {
        string keyvalue = JsonSerializer.Serialize(value);
        if (keyvalue != null && !string.IsNullOrEmpty(keyvalue))
        {
            Microsoft.Maui.Storage.Preferences.Set(key, keyvalue);
        }
    }

    /// <summary>
    /// Get an element using a certain key, with type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T Get<T>(string key)
    {
        T? UnpackedValue = default;
        string keyvalue = Microsoft.Maui.Storage.Preferences.Get(key, string.Empty);

        if (keyvalue != null && !string.IsNullOrEmpty(keyvalue))
        {
            UnpackedValue = JsonSerializer.Deserialize<T>(keyvalue);
        }
        return UnpackedValue;
    }

    /// <summary>
    /// Delete an element with a certain key
    /// </summary>
    /// <param name="key"></param>
    public void Delete(string key)
    {
        Microsoft.Maui.Storage.Preferences.Remove(key);
    }

    /// <summary>
    /// Check if an element with a certain key exists
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Exists(string key)
    {
        return Microsoft.Maui.Storage.Preferences.ContainsKey(key);
    }

    /// <summary>
    /// ATTENTION: Clears the whole Preferences-Store
    /// </summary>
    public void ClearAll()
    {
        Microsoft.Maui.Storage.Preferences.Clear();
    }
}