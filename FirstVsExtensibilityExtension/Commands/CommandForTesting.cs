﻿using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirstVsExtensibilityExtension.Services;

namespace FirstVsExtensibilityExtension.Commands
{
    [VisualStudioContribution]
    public class _CommandForTesting : Command
    {
        private readonly ITestService testService;

        //public CommandForTesting(ITestService service)
        //    => this.testService = Requires.NotNull(service, nameof(service));

        public override Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public override CommandConfiguration CommandConfiguration => new("%FirstFirstVsExtensibilityExtension.CommandForTesting.DisplayName%")
        {
        };
    }
}