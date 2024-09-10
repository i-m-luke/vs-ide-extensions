using System.Linq;

namespace FirstExtension.Commands
{
    [Command(PackageIds.FirstCommand)]
    internal sealed class FirstCommand : BaseCommand<FirstCommand>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            // Zobrazení messageboxu: await VS.MessageBox.ShowWarningAsync("FirstExtension", "Button clicked");
            await VS.MessageBox.ShowWarningAsync("FIRST MESSAGE BOX", "...");

            var docView = await VS.Documents.GetActiveDocumentViewAsync();
            if (docView is null)
            {
                return;
            }

            var selection = docView.TextView?.Selection.SelectedSpans.FirstOrDefault();
            if (selection is null)
            {
                return;
            }

            docView.TextBuffer?.Replace(selection.Value, Guid.NewGuid().ToString());
        }
    }
}