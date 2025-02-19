using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirstVsExtensibilityExtension.Services;

using Microsoft.VisualStudio.ProjectSystem.Query;

using FirstVsExtensibilityExtension.Common;

using Microsoft.VisualStudio.Extensibility.Shell;

namespace FirstVsExtensibilityExtension.Commands
{
    [VisualStudioContribution]
    public class CommandForTesting : Command
    {
        private readonly ITestService testService;


        public override CommandConfiguration CommandConfiguration => new("%CommandForTesting.DisplayName%")
        {
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
        };

        //public CommandForTesting(ITestService service)
        //    => this.testService = Requires.NotNull(service, nameof(service));

        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            var extensibility = context.Extensibility;

            // ZÍSKANÍ QUERIES PROJEKTŮ Z AKTIVNÍHO  SOLUTION
#pragma warning disable VSEXTPREVIEW_PROJECTQUERY_PROPERTIES_BUILDPROPERTIES // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var projects = await extensibility
                .Workspaces()
                .QuerySolutionAsync(
                    query => query.Get(solution => solution.Projects)
                        .With(p => p.Name)
                        .With(p => p.Properties),
                    cancellationToken);
#pragma warning restore VSEXTPREVIEW_PROJECTQUERY_PROPERTIES_BUILDPROPERTIES // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

#pragma warning disable VSEXTPREVIEW_PROJECTQUERY_PROPERTIES_BUILDPROPERTIES
            //var projectProperties = projects.First().Properties;
#pragma warning restore VSEXTPREVIEW_PROJECTQUERY_PROPERTIES_BUILDPROPERTIES

            #region SOLUTION CONFIGURATION QUERY

            // extensibility.Workspaces().QuerySolutionAsync(async query => await query.Get(solution => solution).SubscribeAsync(null, null), cancellationToken);
            var solutionConfiguration = await extensibility.Workspaces()
                .QuerySolutionAsync(
                    query => query.Get(solution => solution.SolutionConfigurations
                        .With(solution => solution.ProjectContexts)),
                    cancellationToken);

            var singleConfiguration = solutionConfiguration.FirstOrDefault();
            if (singleConfiguration is null)
            {
                return;
            }

            //var unsubsribe = await singleConfiguration
            //    .AsQueryable()
            //    .SubscribeAsync(...)

            #endregion

            var projectQuery = new CustomSubscriber<IProjectSnapshot>(
                onNextExecutor:
                async project =>
                {
                    await extensibility.Shell().ShowPromptAsync(
                        $"Project changed. Project name: {project.ItemType.Name}",
                        PromptOptions.OK,
                        cancellationToken);
                },
                onErrorExecutor:
                async ex => await extensibility.Shell().ShowPromptAsync("Exception: " + ex.Message, PromptOptions.OK, cancellationToken),
                onCompletedExecutor:
                async () => await extensibility.Shell().ShowPromptAsync("Completed...", PromptOptions.OK, cancellationToken));

            foreach (var project in projects)
            {
                // NOTION PONZ:: LZE POUŽÍT AsUpdtable tak AsQueryable a TrackUpdatesAsync 
                await project.AsQueryable().SubscribeAsync(projectQuery, cancellationToken);
            }

            // EXPLICITNÍ INICIALIZACE COMMANDŮ:
            // await this.InitializeCommandsAsync(...);

            await Task.CompletedTask;
        }
    }
}