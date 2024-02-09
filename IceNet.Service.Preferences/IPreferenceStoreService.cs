namespace IceNet.Service.Preferences;

public interface IPreferenceStoreService
{
    void ClearAll();
    void Delete(string key);
    bool Exists(string key);
    T Get<T>(string key);
    void Set(string key, object value);
}