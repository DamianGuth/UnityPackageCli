public class ArgumentHandler
{
    public static void HandleArguments(string[] args)
    {
        if(args.Length == 0)
            return;

        if(args[0] == "create-config")
        {
            ConfigHandler.CreateDefaultConfig();
        }

        if(args[0] == "create")
        {
            PackageHandler.HandlePackages(ConfigHandler.ReadConfig());
        }
    }
}