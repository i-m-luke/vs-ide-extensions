using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstVsExtensibilityExtension
{
    internal static class Helpers
    {
        public const string Root = "FirstVsExtensibilityExtension";

        public static string CreateStringResourcesPath(string pathAfterRoot) => $"%{Root}.{pathAfterRoot}%";
    }
}