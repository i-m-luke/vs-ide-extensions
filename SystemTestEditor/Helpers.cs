using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTestEditor
{
    internal static class Helpers
    {
        public const string Root = "SystemTestEditor";

        public static string CreateStringResourcesPath(string pathAfterRoot) => $"%{Root}.{pathAfterRoot}%";
    }
}