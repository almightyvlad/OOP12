using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

public static class SVRLog
{
    private static string filename = "svrlogfile.txt";
    private static string path = $"C:\\Users\\vlad\\source\\repos\\OOP12\\{filename}";
    private static DateTime ExtractDateFromLog(string log)
    {
        string pattern = @"\d{2}\.\d{2}\.\d{4} \d{2}:\d{2}:\d{2}";

        Match match = Regex.Match(log, pattern);

        if (match.Success)
        {
            DateTime date = DateTime.Parse(match.Value);
            return date;
        }
        else
        {
            Console.WriteLine("Дата не найдена в логе.");
            return DateTime.MinValue; 
        }
    }

    public static void WriteToFile(string action)
    {
        DateTime dateTime = DateTime.Now;

        string line = $"Action: {action}, details: {filename}, time: {path}, {dateTime}";

        using (StreamWriter writer = new StreamWriter(path, true))
        {
            writer.WriteLine(line);
        }
    }
    public static void ReadFromFile()
    {
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }
    }
    public static void FindInfo(string action)
    {
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line.Contains(action));
            }
        }
    }

    public static void FindInfoByDate(DateTime date)
    {
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains(date.ToString("yyyy-MM-dd")))
                {
                    Console.WriteLine(line);
                }
            }
        }
    }

    public static void FindInfoByTimeRange(DateTime startTime, DateTime endTime)
    {
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                DateTime logTime = ExtractDateFromLog(line);
                if (logTime >= startTime && logTime <= endTime)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
    public static void CountRecords()
    {
        int count = 0;
        string line;
        using (StreamReader reader = new StreamReader(path))
        {
            while ((line = reader.ReadLine()) != null)
            {
                count++;
            }
        }
        Console.WriteLine($"Количество записей: {count}");
    }
    public static void DeleteOldRecords()
    {
        DateTime currentHour = DateTime.Now;
        string[] lines = File.ReadAllLines(path);
        var currentHourLogs = new List<string>();

        foreach (var line in lines)
        {
            DateTime logTime = ExtractDateFromLog(line);
            if (logTime.Hour == currentHour.Hour && logTime.Date == currentHour.Date)
            {
                currentHourLogs.Add(line);
            }
        }
        File.WriteAllLines(path, currentHourLogs);
    }
}


public static class SVRDiskInfo
{
    public static void ShowFreeSpace()
    {
        DriveInfo[] driveInfo = DriveInfo.GetDrives();

        foreach (DriveInfo drive in driveInfo)
        {
            if (drive.IsReady)
            {
                Console.WriteLine($"Free space on disk {drive.Name}: {drive.AvailableFreeSpace}");
            }
        }
    }
    public static void ShowFileSystemInfo()
    {
        DriveInfo[] driveInfo = DriveInfo.GetDrives();

        foreach (DriveInfo drive in driveInfo)
        {
            if (drive.IsReady)
            {
                Console.WriteLine($"Name: {drive.Name}, Format: {drive.DriveFormat}");
            }
        }
    }

    public static void ShowAllDisksInfo()
    {
        DriveInfo[] driveInfo = DriveInfo.GetDrives();

        foreach (DriveInfo drive in driveInfo)
        {
            if (drive.IsReady)
            {
                Console.WriteLine($"Name: {drive.Name}, Total size: {drive.TotalSize}, Free space: {drive.AvailableFreeSpace}, Volume label: {drive.VolumeLabel}");
            }
        }
    }
}

public static class SVRFileInfo
{
    public static void ShowFullPath(string file)
    {
        Console.WriteLine(Path.GetFullPath(file));
    }
    public static void ShowSizeAndExtension(string path)
    {
        FileInfo fileInfo = new FileInfo(path);

        Console.WriteLine($"Name: {fileInfo.Name}, Extension: {fileInfo.Extension}, Length: {fileInfo.Length}");
    }
    public static void ShowCreationAndChangeDate(string file)
    {
        Console.WriteLine($"Creation time: {File.GetCreationTime(file)}, Change time: {File.GetLastWriteTime(file)}");
    }
}

public static class SVRDirInfo
{
    public static void ShowFileCount(string path)
    {
        if (Directory.Exists(path))
        {
            Console.WriteLine(Directory.GetFiles(path).Length);
        } else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
    public static void ShowCreationTime(string path) 
    {
        if (Directory.Exists(path))
        {
            Console.WriteLine(Directory.GetCreationTime(path));
        }
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
    public static void ShowSubdirectories(string path)
    {
        if (Directory.Exists(path))
        {
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                Console.WriteLine(directory);
            }
        }
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
    public static void ShowParentDirectories(string path)
    {
        if (Directory.Exists(path))
        {
            var parentDirectories = new List<string>();
            var currentDirectories = new DirectoryInfo(path);

            while (currentDirectories.Parent != null)
            {
                parentDirectories.Add(currentDirectories.Parent.ToString());
                currentDirectories = currentDirectories.Parent;
            }

            foreach (string directory in parentDirectories)
            {
                Console.WriteLine(directory);
            }
        }
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
} 
public static class SVRFileManager
{
    public static string[] ListFilesAndFolders(string path)
    {
        string[] files = Directory.GetFileSystemEntries(path);
        return files;
    }
    public static void ShowListFilesAndFolders(string path)
    {
        string[] files = ListFilesAndFolders(path);
        
        foreach (string file in files)
        {
            Console.WriteLine(file);
        }
    }
    public static void CreateDirectory(string path, string directoryName)
    {
        string inspectDirectory = Path.Combine(path, directoryName);

        if (!Directory.Exists(inspectDirectory))
        {
            Directory.CreateDirectory(inspectDirectory);
        }
    }
    public static void SaveDirectoryInfo(string path)
    {
        if (Directory.Exists(path))
        {
            string inspectDirectory = Path.Combine(path, "SVRInspect", "svrdirinfo.txt");
            using (StreamWriter writer = new StreamWriter(inspectDirectory))
            {
                foreach (string file in ListFilesAndFolders(path))
                {
                    writer.WriteLine(file);
                }
            }
        } 
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
    public static void CreateCopyAndRename(string path, string destinationPath)
    {

        string inspectDirectory = Path.Combine(path, "SVRInspect", "svrdirinfo.txt");
        if (File.Exists(inspectDirectory))
        {
            File.Copy(inspectDirectory, destinationPath);
        } 
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
    public static void DeleteFile(string path)
    {

        string inspectDirectory = Path.Combine(path, "SVRInspect", "svrdirinfo.txt");
        if (File.Exists(inspectDirectory))
        {
            File.Delete(inspectDirectory);
        } 
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
    public static void CopyFilesWithExtension(string path, string destPath, string extension)
    {
        string[] files = Directory.GetFiles(path, $"*.{extension}");
        foreach (var file in files)
        {
            string fileName = Path.GetFileName(file);
            string destinationPath = Path.Combine(destPath, fileName);
            File.Copy(file, destinationPath);
        }
    }
    public static void MoveDirectory(string path, string destPath)
    {
        string sourcePath = Path.Combine(path, "SVRFiles");
        string destinationPath = Path.Combine(destPath, "SVRFiles");

        if (Directory.Exists(sourcePath))
        {
            Directory.Move(sourcePath, destinationPath);
        }
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }

    public static void CreateArchive(string path, string archivePath)
    {
        string sourcePath = Path.Combine(path, "SVRFiles");
        
        if (Directory.Exists(sourcePath))
        {
            string zipPath = Path.Combine(archivePath, "SVRFiles.zip");
            ZipFile.CreateFromDirectory(sourcePath, zipPath);
        }
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }

    public static void ExtractArchive(string path, string archivePath)
    {
        string zipPath = Path.Combine(archivePath, "SVRFiles.zip");

        if (File.Exists(zipPath))
        {
            string destinationPath = Path.Combine(path, "ExtractedSVRFiles");
            ZipFile.ExtractToDirectory(zipPath, destinationPath);
        }
        else
        {
            Console.WriteLine("Такого пути не существует");
        }
    }
}
namespace OOP12
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // 1, 6
            //SVRLog.WriteToFile("User login");
            //SVRLog.ReadFromFile();

            //SVRLog.FindInfo("login");

            //DateTime date = new DateTime(2024, 11, 21);
            //SVRLog.FindInfoByDate(date);

            //DateTime start = new DateTime(2024, 11, 21, 0, 0, 0);
            //DateTime end = new DateTime(2024, 11, 23, 0, 0, 0);
            //SVRLog.FindInfoByTimeRange(start, end);

            //SVRLog.CountRecords();

            //SVRLog.DeleteOldRecords();

            // 2

            //SVRDiskInfo.ShowFreeSpace();
            //SVRDiskInfo.ShowFileSystemInfo();
            //SVRDiskInfo.ShowAllDisksInfo();

            // 3

            //SVRFileInfo.ShowFullPath("OOP12.sln");
            //SVRFileInfo.ShowSizeAndExtension("C:\\Users\\vlad\\source\\repos\\OOP12\\OOP12.sln");
            //SVRFileInfo.ShowCreationAndChangeDate("OOP12.sln");

            //  4

            //SVRDirInfo.ShowFileCount("C:\\Users\\vlad\\source\\repos\\OOP12");
            //SVRDirInfo.ShowCreationTime("C:\\Users\\vlad\\source\\repos\\OOP12");
            //SVRDirInfo.ShowSubdirectories("C:\\Users\\vlad\\source\\repos\\OOP12");
            //SVRDirInfo.ShowParentDirectories("C:\\Users\\vlad\\source\\repos\\OOP12");

            //5

            //SVRFileManager.ShowListFilesAndFolders("C:\\Users\\vlad\\source\\repos\\OOP12");
            //Console.WriteLine();

            //SVRFileManager.CreateDirectory("C:\\Users\\vlad\\source\\repos\\OOP12", "SVRInspect");
            //SVRFileManager.ShowListFilesAndFolders("C:\\Users\\vlad\\source\\repos\\OOP12");
            //Console.WriteLine();

            //SVRFileManager.SaveDirectoryInfo("C:\\Users\\vlad\\source\\repos\\OOP12");
            //SVRFileManager.CreateCopyAndRename("C:\\Users\\vlad\\source\\repos\\OOP12", "C:\\Users\\vlad\\source\\repos\\OOP11\\svrdirinfo.txt")
            //SVRFileManager.DeleteFile("C:\\Users\\vlad\\source\\repos\\OOP12");


            //SVRFileManager.CreateDirectory("C:\\Users\\vlad\\source\\repos\\OOP12", "SVRInspect");
            SVRFileManager.CreateDirectory("C:\\Users\\vlad\\source\\repos\\OOP12", "SVRFiles");
            //SVRFileManager.CopyFilesWithExtension("C:\\Users\\vlad\\source\\repos\\OOP11\\", "C:\\Users\\vlad\\source\\repos\\OOP12\\SVRFiles", "txt");
            //SVRFileManager.MoveDirectory("C:\\Users\\vlad\\source\\repos\\OOP12", "C:\\Users\\vlad\\source\\repos\\OOP12\\SVRInspect");

            SVRFileManager.CreateArchive("C:\\Users\\vlad\\source\\repos\\OOP12", "C:\\Users\\vlad\\source\\repos\\OOP12");
            SVRFileManager.ExtractArchive("C:\\Users\\vlad\\source\\repos\\OOP12", "C:\\Users\\vlad\\source\\repos\\OOP12");





        }
    }
}
