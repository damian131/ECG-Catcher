using Caliburn.Micro;
using Callisto.Controls;
using RGraphics.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;

namespace ECGCatcher.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Shows the dialog asynchronous.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static async Task ShowDialogAsync(this IDialog viewModel)
        {
            var view = ViewLocator.LocateForModel(viewModel, null, null);

            if (view as CustomDialog == null)
                throw new NotSupportedException(String.Format("{0} must inherit from Callisto.CustomDialog", view.GetType().Name));

            ViewModelBinder.Bind(viewModel, view, null);

            var activator = viewModel as IActivate;
            if (activator != null)
                activator.Activate();

            viewModel.IsOpen = true;

            await ThreadPool.RunAsync(new WorkItemHandler((IAsyncAction action) =>
            {
                while (viewModel.IsOpen)
                    Task.Delay(1000);
            }));
        }
    }
}
