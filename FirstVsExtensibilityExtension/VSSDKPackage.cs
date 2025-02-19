using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FirstVsExtensibilityExtension;

[Guid(GuidString)]
[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
internal class VSSDKPackage : AsyncPackage
{
    public const string GuidString = "2a57055e-2026-4cbb-b3e3-adaf0582407a";

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
    {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
    }
}