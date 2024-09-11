using Microsoft.VisualStudio.Extensibility;

using Moq;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using FirstVsExtensibilityExtension.Commands;

namespace FirstVsExtensibilityExtensionTests
{
    [TestFixture]
    public class CommandForTestingTests
    {
        [OneTimeSetUp]
        public void OTSU()
        {
            _ = new ExtensionEntrypoint();
        }

        [Test]
        public async Task SomeTestCaseAsync()
        {
            // given
            var command = new CommandForTesting();
            var cts = new CancellationTokenSource();
            // when
            await command.InitializeAsync(cts.Token);
            await command.ExecuteCommandAsync(Mock.Of<IClientContext>(), cts.Token);
            // then
            // ...
        }
    }
}