using UnityPackageCli.Models;

namespace UnityPackageCli;

public static class Program
{
    public static void Main(string[] args)
    {
        ArgumentHandler.HandleArguments(args);
    }
}
