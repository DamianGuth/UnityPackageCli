using UnityPackageCli.Models;

public static class PackageHandler
{
    public static void HandlePackages(UnityPackageConfig config)
    {
        bool anyBuildFailed = false;
        foreach(UnityPackageConfigElement configElement in config.packages)
        {
            Console.WriteLine("Building package: " + configElement.name);

            Packagebuilder packagebuilder = new Packagebuilder(configElement);
            bool successState = packagebuilder.BuildPackage();

            if(!successState)
                anyBuildFailed = true;

            Console.WriteLine("Package built. State: " + (successState ? "sucessfully" : "failed"));
        }

        if (anyBuildFailed)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        Console.WriteLine("Done");
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}