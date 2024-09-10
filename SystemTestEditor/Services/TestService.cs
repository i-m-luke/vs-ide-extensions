using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Helpers;
using Microsoft.VisualStudio.Extensibility.Shell;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTestEditor.Services
{
    internal class TestService : DisposableObject, ITestService
    {
        private readonly VisualStudioExtensibility extensibility;

        public TestService(VisualStudioExtensibility extensibility)
            => this.extensibility = extensibility;

        public async Task ShowPromptAsync()
        {
            var cts = new CancellationTokenSource();
            await this.extensibility.Shell().ShowPromptAsync("Hello from Test Service :)", PromptOptions.OK, cts.Token);
        }
    }
}