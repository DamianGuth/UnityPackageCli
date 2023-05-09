# Unity Package Cli
A simple tool to export unity packages from cli! I have no idea why unity does not support this having pipelines in mind but here you go.


## Requirements
- .NET 7
- Python


## Config
Create a config using

```cmd
cmd > UnityPackageCli.exe create-config
```
This will create a **config.json** in the **packagebuilderconfig** directory.

Here is a basic config containing two different packages to create:

```json
{
    "packages": [
        {
            "name": "test01",
            "outputpath": "Test",
            "directories": [
                "Assets/Test01"
            ]
        },
        {
            "name": "test02",
            "outputpath": "Test",
            "directories": [
                "Assets/Test02"
            ]
        }
    ]
}
```

Use **outputpath** to define a **directory** to move the package to.

## Generate packages
After setting up the config you can use the following command:

```cmd
cmd > UnityPackageCli.exe create
```

This will generate the packages next to the UnityPackageCli.exe (or in your custom output path).
Now you can load up a unity project and import the package.
