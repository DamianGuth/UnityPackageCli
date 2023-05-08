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

        if (!Directory.Exists("packagebuilderconfig"))
            Directory.CreateDirectory("packagebuilderconfig");

        if (!File.Exists(_configPath))
        {
            File.WriteAllText(_configPath, JsonSerializer.Serialize(defaultConfig));
        }
    }

    public static UnityPackageConfig ReadConfig()
    {
        _configPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "/" + _configPath;

        UnityPackageConfig config;

        if (Directory.Exists("packagebuilderconfig") && File.Exists(_configPath))
        {
            _configText = File.ReadAllText(_configPath);
            
            config = JsonSerializer.Deserialize<UnityPackageConfig>(_configText);
        }
        else
        {
            config = new UnityPackageConfig();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No config found. You can create one using the 'create-config' parameter");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        return config;
    }
}