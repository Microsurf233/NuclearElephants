using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using static System.String;

namespace NuclearElephants.Helpers;

/// <summary>
/// 用于管理程序外置目录的帮助类.
/// </summary>
/// <param name="relativeDirectoryPath">
/// 基于可执行文件所在目录的相对路径.
/// 使用`Path.Combine()`传入二级及以上的目录层数. 
/// </param>
public class ExternalDirectoryHelper(string relativeDirectoryPath)
{
    private readonly Random _random = new();
    
    /// <summary>
    /// 实例化时指定的目录.
    /// </summary>
    public string ExternalDirectoryPath => Path.Combine(Environment.CurrentDirectory, relativeDirectoryPath);

    /// <summary>
    /// 目录下的所有文件(绝对路径).
    /// </summary>
    public List<string> Files
    {
        get
        {
            var isExist = CheckOrCreateExternalDirectoryFull(ExternalDirectoryPath);
            if (!isExist) return [];
            return [..Directory.GetFiles(ExternalDirectoryPath)];
        }
    }

    /// <summary>
    /// 从所有文件中随机挑选一个.
    /// </summary>
    /// <remarks>
    /// 当目录中没有文件时，返回`string.Empty`.
    /// </remarks>
    /// <returns>被选中的文件.</returns>
    public string PickFileRandomly() => Files.Count == 0 ? Empty : Files[_random.Next(Files.Count)];

    /// <summary>
    /// 在资源管理器中打开文件夹.
    /// </summary>
    public void OpenInFileExplorer() => Process.Start("explorer.exe", ExternalDirectoryPath);

    /// <summary>
    /// 检测目录是否存在. 若不存在则创建一个.
    /// </summary>
    /// <param name="relativePath">将被检测的相对目录.</param>
    /// <returns>被检测的目录是否存在.</returns>
    public static bool CheckOrCreateExternalDirectoryRelative(string relativePath)
    {
        var fullPath = Path.Combine(Environment.CurrentDirectory, relativePath);
        return CheckOrCreateExternalDirectoryFull(fullPath);
    }

    /// <summary>
    /// 检测目录是否存在. 若不存在则创建.
    /// </summary>
    /// <param name="relativePaths">将被检测的相对目录.</param>
    /// <returns>被检测的目录是否存在.</returns>
    public static List<bool> CheckOrCreateExternalDirectoryRelative(params string[] relativePaths)
    {
        var fullPaths = new string[relativePaths.Length];
        for (var i = 0; i < relativePaths.Length; i++)
        {
            var relativePath = relativePaths[i];
            fullPaths[i] = Path.Combine(Environment.CurrentDirectory, relativePath);
        }

        return CheckOrCreateExternalDirectoryFull(fullPaths);
    }

    /// <summary>
    /// 检测目录是否存在. 若不存在则创建一个.
    /// </summary>
    /// <param name="fullPath">将被检测的目录.</param>
    public static bool CheckOrCreateExternalDirectoryFull(string fullPath)
    {
        if (Directory.Exists(fullPath)) return true;
        Directory.CreateDirectory(fullPath);
        return false;
    }

    /// <summary>
    /// 检测目录是否存在. 若不存在则创建一个.
    /// </summary>
    /// <param name="fullPaths">将被检测的目录.</param>
    public static List<bool> CheckOrCreateExternalDirectoryFull(params string[] fullPaths)
    {
        List<bool> results = [];
        foreach (var path in fullPaths)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                results.Add(false);
            }
            else
            {
                results.Add(true);
            }
        }

        return results;
    }
}