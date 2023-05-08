using System.Text.Json;
using UnityPackageCli.Models;

public static class ConfigHandler
{
    private static string _configPath = "packagebuilderconfig/config.json";
    private static string? _configText;

    public static void CreateDefaultConfig()
    {
        UnityPackageConfig defaultConfig = new ();
        UnityPackageConfigElement unityPackageConfigElement = new UnityPackageConfigElement();
        unityPackageConfigElement.outputpath = "Test";
        unityPackageConfigElement.name = "test";
        unityPackageConfigElement.directories.Add("test");

        defaultConfig.packages.Add(unityPackageConfigElement);

        _configPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "/" + _configPath;

        if (!File.Exists(_configPath))
        {
            File.WriteAllText(_configPath, JsonSerializer.Serialize(defaultConfig));
        }
    }

    public static UnityPackageConfig ReadConfig()
    {
        _configPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "/" + _configPath;

        UnityPackageConfig config;

        if (File.Exists(_configPath))
        {
            _configText = File.ReadAllText(_configPath);
            
            config = JsonSerializer.Deserialize<UnityPackageConfig>(_configText);
        }
        else
        {
            config = new UnityPackageConfig();
        }

        return config;
    }
}