using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FirstVsExtensibilityExtension.Common
{
    internal static class FileExtensions
    {
        public const string TestFile = "test";

        public const string TestFileRegexPattern = $@".*\.{TestFile}$";
    }
}