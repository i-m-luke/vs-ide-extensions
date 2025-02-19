using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.ProjectSystem.Query;
using Microsoft.VisualStudio.RpcContracts.ProgressReporting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstVsExtensibilityExtension.Commands
{
    [VisualStudioContribution]
    internal class CommandWithProgressbar : Command
    {
        public override CommandConfiguration CommandConfiguration => new("%CommandWithProgressBar.DisplayName%")
        {
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
            Icon = new CommandIconConfiguration(ImageMoniker.KnownValues.AboutBox, IconSettings.IconAndText)
        };

        public static ProgressStatus CreateProgressStatus(int current, int max)
            => new(current / 2 * 100, "Please wait...");

        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            // NOTE: Nutné provést disposing, jinak se progress bar neukončí
            using var progress = await context.Extensibility.Shell().StartProgressReportingAsync(
                Strings.CommandWithProgressBar_Running,
                new ProgressReporterOptions(true), // Přidá cancel button do progressbaru
                cancellationToken);

            try
            {
                const int max = 10;
                for (var i = 0; i < max; i++)
                {
                    progress.Report(CreateProgressStatus(i, max));
                    progress.CancellationToken.ThrowIfCancellationRequested();
                    await Task.Delay(1000, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // ...
            }

            await Extensibility.Shell().ShowPromptAsync(":))", PromptOptions.OK, cancellationToken);
        }
    }
}