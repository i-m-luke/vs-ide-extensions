using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

using SystemTestEditor.Services;

namespace SystemTestEditor
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc />
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            RequiresInProcessHosting = true,
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.

            // Inicializace služby TestService: služba je poté přístupná z konstruktoru VisualStudioContribution objektů
            serviceCollection.AddScoped<ITestService>(provider => new TestService(provider.GetRequiredService<VisualStudioExtensibility>()));
        }
    }
}