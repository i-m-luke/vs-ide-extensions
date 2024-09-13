using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

using FirstVsExtensibilityExtension.Services;

using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.ProjectSystem.Query;

using System;

using Microsoft.VisualStudio.Extensibility.Shell;

using FirstVsExtensibilityExtension.Common;

using Microsoft.VisualStudio.Utilities;

// ReSharper disable UnusedMember.Global

namespace FirstVsExtensibilityExtension
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    // Lze i tímto vyřešit, kdy má být extension funkční?
    // [AppliesToProject("SystemTestPackage")]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc />
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            RequiresInProcessHosting = true,
            // TÍMTO BY MĚLO BÝT MOŽNÉ OMEZIT EXTENSION POUZE PRO DANÝ TYP PROJEKTU (jako je řešeno u současného řešení)
            // SystemTestPackage je definovan v SystemTestPackage.vstemplate
            // Další možnosti metadat, které lze template projektu nastavit: TemplateID, ...
            LoadedWhen = ActivationConstraint.ActiveProjectCapability(ProjectCapability.Custom("SystemTestPackage")),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.

            // Inicializace služby TestService: služba je poté přístupná z konstruktoru VisualStudioContribution objektů
            serviceCollection.AddScoped<ITestService>(provider
                => new TestService(provider.GetRequiredService<VisualStudioExtensibility>()));
        }

        /// USECASE: Při inicializaci extensionu
        /// viz: https://learn.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.extensibility.extensioncore.oninitializedasync?view=vs-extensibility#microsoft-visualstudio-extensibility-extensioncore-oninitializedasync(microsoft-visualstudio-extensibility-visualstudioextensibility-system-threading-cancellationtoken
        protected override async Task OnInitializedAsync(VisualStudioExtensibility extensibility, CancellationToken cancellationToken)
        {
            Console.WriteLine("EXTENSION INITIALIZED!");

            // ZÍSKANÍ QUERIES PROJEKTŮ Z AKTIVNÍHO  SOLUTION
            var projects = await extensibility
                .Workspaces()
                .QuerySolutionAsync(
                    query => query.Get(solution => solution.Projects)
                        .With(p => p.Name)
                        .With(p => p.TypeGuid)
                    // U současného extensionu se specifikuje project type GUID (PackageProjectUnconfigured.cs)
                    /*.Where(p => p.TypeGuid.ToString() == "some-guid")*/,
                    cancellationToken);

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