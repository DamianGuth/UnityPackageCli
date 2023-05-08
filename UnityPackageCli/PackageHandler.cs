using UnityPackageCli.Models;

public static class PackageHandler
{
    public static void HandlePackages(UnityPackageConfig config)
    {
        foreach(UnityPackageConfigElement configElement in config.packages)
        {
            Console.WriteLine("Building package: " + configElement.name);

            Packagebuilder packagebuilder = new Packagebuilder(configElement);
            bool successState = packagebuilder.BuildPackage();

            Console.WriteLine("Package built. State: " + (successState ? "sucessfully" : "failed"));
        }
    }
}