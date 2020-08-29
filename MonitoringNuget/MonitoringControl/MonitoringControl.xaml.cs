using System.Windows.Controls;
using Autofac;
using MonitoringNuget.DataAccess.RepositoryClasses;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.IoCContainer;
using MonitoringNuget.ViewModel;
using MonitoringNuget.ViewModel.Interface;

namespace MonitoringNuget.MonitoringControl
{
    /// <summary>
    ///     Interaction logic for MonitoringControl.xaml
    /// </summary>
    public partial class MonitoringControl : UserControl
    {
        private IViewModel viewModel;

        public MonitoringControl()
        {
            InitializeComponent();

            var IoCContainer = new IoCContainer<MonitoringViewModel>();
            viewModel = IoCContainer.ResolveViewModel();

            this.DataContext = viewModel;
        }
    }
}