using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Extensibility.Editor;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Extensibility.Shell;

namespace SystemTestEditor.Editors
{
    // NOTE:
    // ExtensionPart určí exponovanou část extensionu
    // Rozhraní pak určí další specifické vlastnosti části

    // EditorExtensibility.EditAsync: Přístup k aktuálně editovanoho dokumentu
    [VisualStudioContribution]
    internal class TxtFileEditor : ExtensionPart, ITextViewOpenClosedListener
    {
        public TextViewExtensionConfiguration TextViewExtensionConfiguration => new()
        {
            AppliesTo = [DocumentFilter.FromGlobPattern("**/*.md", true)]
        };

        public Task TextViewClosedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task TextViewOpenedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
        {
            var cts = new CancellationTokenSource();

            // Nějak se nevypisuje do output win ...
#pragma warning disable VSEXTPREVIEW_OUTPUTWINDOW
            var output = this.Extensibility.Views().Output.GetChannelAsync("id", "display name", cts.Token).Result;
#pragma warning restore VSEXTPREVIEW_OUTPUTWINDOW
            if (output != null)
            {
#pragma warning disable VSEXTPREVIEW_OUTPUTWINDOW
                await output.Writer.WriteLineAsync("TEXT VIEW CREATED");
#pragma warning restore VSEXTPREVIEW_OUTPUTWINDOW
            }

            await this.Extensibility.Shell().ShowPromptAsync("MD file opened", PromptOptions.OK, cts.Token);
        }
    }
}