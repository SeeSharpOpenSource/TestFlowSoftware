using System;
using System.IO;
using System.Text.RegularExpressions;

namespace JYProductOperationPanel
{
    internal static class Utility
    {
        public static string GetAbsolutePath(string path, string[] availableDirs)
        {
            string fullPath = TryGetAbsolutePath(path, availableDirs);
            if (string.IsNullOrWhiteSpace(fullPath))
            {
                throw new ApplicationException($"The panel form assembly {path} cannot be found.");
            }
            return fullPath;
        }

        public static string TryGetAbsolutePath(string path, string[] availableDirs)
        {
            const string absolutePathFormat = @"^[a-zA-Z]:{0}";
            char dirDelim = Path.DirectorySeparatorChar;
            // \在正则表达式中需要转义
            string absolutePathRegexStr = dirDelim.Equals('\\')
                ? string.Format(absolutePathFormat, @"\\")
                : string.Format(absolutePathFormat, dirDelim);
            if (Regex.IsMatch(path, absolutePathRegexStr))
            {
                return path;
            }

            foreach (string availableDir in availableDirs)
            {
                string fullPath = availableDir + path;
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return string.Empty;
        }

        public static string GetRelativePath(string path, string[] availableDirs)
        {
            const string absolutePathFormat = @"^[a-zA-Z]:{0}";
            char dirDelim = Path.DirectorySeparatorChar;
            // \在正则表达式中需要转义
            string absolutePathRegexStr = dirDelim.Equals('\\')
                ? string.Format(absolutePathFormat, @"\\")
                : string.Format(absolutePathFormat, dirDelim);
            if (!Regex.IsMatch(path, absolutePathRegexStr))
            {
                return path;
            }

            foreach (string availableDir in availableDirs)
            {
                if (path.StartsWith(availableDir))
                {
                    return path.Substring(availableDir.Length, path.Length - availableDir.Length);
                }
            }
            return path;
        }
    }

}