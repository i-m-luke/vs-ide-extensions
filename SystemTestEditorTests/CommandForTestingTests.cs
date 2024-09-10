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

using SystemTestEditor.Commands;

namespace SystemTestEditorTests
{
    [TestFixture]
    public class CommandForTestingTests
    {
        [Test]
        public async Task SomeTestCase()
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