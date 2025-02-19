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
    //[AppliesToProject("SystemTestPackage")]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc />
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            RequiresInProcessHosting = true,
            // TÍMTO BY MĚLO BÝT MOŽNÉ OMEZIT EXTENSION POUZE PRO DANÝ TYP PROJEKTU (jako je řešeno u současného řešení)
            // SystemTestPackage je definovan v SystemTestPackage.vstemplate
            // Další možnosti metadat, které lze template projektu nastavit: TemplateID, ...
            //LoadedWhen = ActivationConstraint.ActiveProjectCapability(ProjectCapability.Custom("SystemTestPackage")),
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

            await Task.CompletedTask;
        }
    }
}