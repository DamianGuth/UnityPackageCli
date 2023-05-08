using System.Diagnostics;
using UnityPackageCli.Models;

public class Packagebuilder
{
    private readonly UnityPackageConfigElement? _config;

    private List<string> _filesToInclude = new List<string>();
    private List<string> _directoriesToInclude = new List<string>();


    public Packagebuilder(UnityPackageConfigElement config)
    {
        _config = config;
    }

    public bool BuildPackage()
    {
        bool success = false;

        try
        {
            FindPackageFilesToInclude();

            string pythonArray = CopyFilesToTempDirectory();

            BuildPythonScript(pythonArray);
            RunPythonScript();

            MovePackageFileToOutput();

            success = true;
        }
        catch (Exception ex)
        {
            success = false;
            Console.WriteLine(ex.ToString());
        }
        finally
        {
            CleanupFiles();
        }

        return success;
    }

    private void FindPackageFilesToInclude()
    {
        foreach (string directory in _config.directories)
        {
            FindPackageFilesToIncludeRekursive(directory);
        }
    }

    private void FindPackageFilesToIncludeRekursive(string searchPath)
    {

        // Find files.
        List<string> files = Directory.GetFiles(searchPath, "*").Where(file => !file.EndsWith(".meta")).ToList();
        _filesToInclude.AddRange(files);

        // Find directories.
        foreach (var dir in Directory.GetDirectories(searchPath))
        {
            _directoriesToInclude.Add(dir);
            FindPackageFilesToIncludeRekursive(dir);
        }
    }

    private string CopyFilesToTempDirectory()
    {
        Directory.CreateDirectory("packageTemp");

        string pythonArray = "[";

        foreach (string filePath in _filesToInclude)
        {
            string basePath = Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)).FullName;
            basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            string fullFilePath = basePath + "/" + filePath;
            string guid = ReadGuidFromMetaFile(fullFilePath);
            DirectoryInfo guidDirectory = Directory.CreateDirectory("packageTemp/" + guid);

            // Create asset file.
            File.Copy(fullFilePath, guidDirectory.FullName + "/asset");

            // Create meta file.
            File.Copy(fullFilePath + ".meta", guidDirectory.FullName + "/asset.meta");

            // Create pathname file.
            File.WriteAllText(guidDirectory.FullName + "/pathname", filePath.Replace("\\", "/"));

            pythonArray += "\"" + guid + "\",";

        }

        foreach (string directoryPath in _directoriesToInclude)
        {
            string basePath = Directory.GetParent(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)).FullName;
            basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            string fullFilePath = basePath + "/" + directoryPath;
            string guid = ReadGuidFromMetaFile(fullFilePath);
            DirectoryInfo guidDirectory = Directory.CreateDirectory("packageTemp/" + guid);

            // Create meta file.
            File.Copy(fullFilePath + ".meta", guidDirectory.FullName + "/asset.meta");

            // Create pathname file.
            File.WriteAllText(guidDirectory.FullName + "/pathname", directoryPath.Replace("\\", "/"));

            pythonArray += "\"" + guid + "\",";

        }

        return pythonArray;
    }

    /// <summary>
    /// Read file guid from unity meta file.
    /// </summary>
    /// <param name="mainFile">The file to read the meta file of.</param>
    /// <returns>The guid from the meta file.</returns>
    private string ReadGuidFromMetaFile(string mainFile)
    {
        string guid;

        string metaFilePath = mainFile += ".meta";

        if (!File.Exists(metaFilePath))
            return "";

        string[] metaFileContent = File.ReadAllLines(metaFilePath);

        if (metaFileContent.Length < 2)
            return "";

        guid = metaFileContent[1].Replace("guid: ", "").Trim();

        return guid;
    }

    private void BuildPythonScript(string pythonArray)
    {
        pythonArray = pythonArray.TrimEnd(',') + "]";

        string pythonScript = @"
import tarfile
tar = tarfile.open(""archtemp.tar.gz"", ""w:gz"")
for name in " + pythonArray + @":
    tar.add(name)
tar.close()";

        File.WriteAllText("packageTemp/pythontar.py", pythonScript);

    }

    /// <summary>
    /// Run the generated python script.
    /// </summary>
    private void RunPythonScript()
    {
        Process p = new Process();

        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = "python.exe";
        start.WorkingDirectory = "packageTemp";
        start.Arguments = "pythontar.py";
        start.UseShellExecute = true;

        p.StartInfo = start;
        p.Start();
        p.WaitForExit();

    }

    private void MovePackageFileToOutput()
    {
        string outputPath = _config.name.Replace(".unitypackage", "") + ".unitypackage";

        // Delete old packages.
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }

        // Check if our file exists.
        if (File.Exists("packageTemp/archtemp.tar.gz"))
        {
            // Wtf it actually worked.
            File.Move("packageTemp/archtemp.tar.gz", outputPath);
        }
    }

    private void CleanupFiles()
    {
        Directory.Delete("packageTemp", true);
    }

}