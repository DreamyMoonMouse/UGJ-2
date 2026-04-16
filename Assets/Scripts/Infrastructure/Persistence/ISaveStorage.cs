public interface ISaveStorage
{
    void SaveInt(string key, int value);
    int LoadInt(string key, int defaultValue = 0);
    void SaveFloat(string key, float value);
    float LoadFloat(string key, float defaultValue = 0f);
    void SaveString(string key, string value);
    string LoadString(string key, string defaultValue = "");
    void DeleteKey(string key);
    void DeleteAll();
}