using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autofac;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.IoCContainer;
using MonitoringNuget.ViewModel;
using MonitoringNuget.ViewModel.Interface;

namespace MonitoringNuget.MonitoringControl.SubControls
{
    /// <summary>
    /// Interaction logic for Jahresvergleich.xaml
    /// </summary>
    public partial class Jahresvergleich : UserControl
    {
        private IViewModel viewModel;

        public Jahresvergleich()
        {
            InitializeComponent();

            var IoCContainer = new IoCContainer<JahresvergleichViewModel>();
            viewModel = IoCContainer.ResolveViewModel();
            this.DataContext = viewModel;
        }
    }
}
