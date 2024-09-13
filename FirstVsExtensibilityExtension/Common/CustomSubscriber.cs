using Microsoft.VisualStudio.ProjectSystem.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstVsExtensibilityExtension.Common
{
    internal class CustomSubscriber<T>(
        CustomSubscriber<T>.OnNextExecutor onNextExecutor,
        CustomSubscriber<T>.OnErrorExecutor? onErrorExecutor = null,
        CustomSubscriber<T>.OnCompletedExecutor? onCompletedExecutor = null)
        : IObserver<IQueryResults<T>>
    {
        public delegate Task OnNextExecutor(IQueryResults<T> queryResult);

        public delegate Task OnErrorExecutor(Exception error);

        public delegate Task OnCompletedExecutor();

        // ReSharper disable ReplaceWithPrimaryConstructorParameter
        private readonly OnNextExecutor onNextExecutor = onNextExecutor;
        private readonly OnErrorExecutor? onErrorExecutor = onErrorExecutor;
        private readonly OnCompletedExecutor? onCompletedExecutor = onCompletedExecutor;
        // ReSharper restore ReplaceWithPrimaryConstructorParameter

        public void OnNext(IQueryResults<T> value)
            => _ = this.onNextExecutor(value);

        public void OnError(Exception error)
            => _ = this.onErrorExecutor?.Invoke(error);

        public void OnCompleted()
            => _ = this.onCompletedExecutor?.Invoke();
    }
}