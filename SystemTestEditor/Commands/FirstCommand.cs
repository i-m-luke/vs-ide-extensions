using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using Microsoft.VisualStudio.ProjectSystem.Query;

using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

using SystemTestEditor.Services;

namespace SystemTestEditor.Commands
{
    /// <summary>
    /// Command1 handler.
    /// </summary>
    [VisualStudioContribution]
    internal class FirstCommand : Command
    {
        private readonly TraceSource logger;
        private readonly ITestService testService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstCommand"/> class.
        /// </summary>
        /// <param name="traceSource">Trace source instance to utilize.</param>
        public FirstCommand(
            TraceSource traceSource, /* Je injektováno pomocí DI; DI je inicializováno v entrypoint (to samé platí pro testService) */
            ITestService testService)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this.logger = Requires.NotNull(traceSource, nameof(traceSource));
            this.testService = Requires.NotNull(testService, nameof(testService));
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration
            => new("%SystemTestEditor.FirstCommand.DisplayName%") // Cesta skrze props json souboru .vsextension\string-resources.json
            {
                // Use this object initializer to set optional parameters for the command. The required parameter,
                // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
                Icon = new(ImageMoniker.KnownValues.NewAttribute, IconSettings.IconAndText),
                Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu], // VsctParent: Umožní integraci i do jiných částí IDE pomocí VSCT?
                // Podmínky: ActivationConstraint...And() / ...Or() 
                EnabledWhen = ActivationConstraint.And(
                    // Command bude enabled při plně načteném solutionu
                    ActivationConstraint.SolutionState(SolutionState.FullyLoaded),
                    // Command bude enabled při navolení souboru s příponout ".md"
                    ActivationConstraint.ClientContext(ClientContextKey.Shell.ActiveEditorFileName,
                        @"\.md$"))
            };

        /// <inheritdoc />
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            // IClientContext: Slouží k získání kontextu ve chvíli a v místě provedení commandu

            var fileName = await context.GetSelectedPathAsync(cancellationToken);
            logger.TraceInformation($"FirstCommand executed!. Selected file path: {fileName}"); // TODO: zprovoznit logging

            if (await context.Extensibility.Shell()
                    .ShowPromptAsync("Display names of all markdown files?", PromptOptions.OKCancel, cancellationToken))
            {
                var markdownFiles = await context.Extensibility.Workspaces().QueryProjectsAsync(
                    query => query.Get(project => project.Files).With(file => file.Path).With(file => file.FileName)
                        .Where(file => file.Extension == ".md"),
                    cancellationToken);

                // Aby bylo možné přistoupit k property, musí být nejprve získána skrze query
                // V tomto případě můžeme přistoupit k FileName a FilePath, protože u query bylo použito With(file => file.Path) a With(file => file.FileName);
                var markdownFilesNames = markdownFiles.Select(file => $"{file.FileName}: {file.Path}").ToArray();

                await context.Extensibility.Shell().ShowPromptAsync(string.Join("\n", markdownFilesNames),
                    PromptOptions.OK,
                    cancellationToken);

                return;
            }

            // Při stisknutí Cancel se provede metoda z injektované služby
            await testService.ShowPromptAsync();
        }
    }
}