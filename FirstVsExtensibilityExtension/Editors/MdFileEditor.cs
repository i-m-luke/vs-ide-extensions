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
using Microsoft.VisualStudio.Utilities;

namespace FirstVsExtensibilityExtension.Editors
{
    // NOTE:
    // ExtensionPart určí exponovanou část extensionu
    // Rozhraní pak určí další specifické vlastnosti části

    // EditorExtensibility.Editor().EditAsync: Přístup k aktuálně editovanoho dokumentu
    [VisualStudioContribution]
    internal class MdFileEditor : ExtensionPart, ITextViewCreationListener, ITextViewOpenClosedListener, ITextViewChangedListener
    {
        public TextViewExtensionConfiguration TextViewExtensionConfiguration => new()
        {
            AppliesTo = [DocumentFilter.FromGlobPattern("**/*.md", true)],
        };

        public Task TextViewChangedAsync(TextViewChangedArgs args, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task TextViewClosedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void TextViewCreated(ITextView textView)
        {
            //textView.Close();
        }

        public async Task TextViewOpenedAsync(ITextViewSnapshot textView, CancellationToken cancellationToken)
        {
            var cts = new CancellationTokenSource();
            await this.Extensibility.Shell().ShowPromptAsync("MD file opened", PromptOptions.OK, cts.Token);
        }
    }
}