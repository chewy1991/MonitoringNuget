using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GenericRepository;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.IoCContainer;
using MonitoringNuget.MonitoringControl.Commands;
using MonitoringNuget.Strategy;
using MonitoringNuget.ViewModel.Interface;

namespace MonitoringNuget.ViewModel
{
    public class JahresvergleichViewModel : DependencyObject, IViewModel
    {
        public ContextStrategy<Jahresvergleich> JahresVergleichRepo;
        private IoCContainer<ContextStrategy<Jahresvergleich>> IoCContainer = new IoCContainer<ContextStrategy<Jahresvergleich>>();

        public static readonly DependencyProperty JahresvergleichProperty = DependencyProperty.Register("Jahresvergleich"
                                                                                                      , typeof(List<Jahresvergleich>)
                                                                                                      , typeof(JahresvergleichViewModel));

        public JahresvergleichViewModel()
        {
            this.JahresVergleichRepo = IoCContainer.ResolveContextStrategy<JahresvergleichRepo, EFProcedure>();
            this.Jahresvergleich = JahresVergleichRepo.GetAll().ToList();
        }

        public List<Jahresvergleich> Jahresvergleich
        {
            get => (List<Jahresvergleich>) GetValue(JahresvergleichProperty);
            set => SetValue(JahresvergleichProperty, value);
        }

        private ICommand _reloadCommand;

        public ICommand ReloadCommand
        {
            get
            {
                return _reloadCommand
                    ?? ( _reloadCommand = new CommandHandler(() =>
                                                             {
                                                                 Jahresvergleich = JahresVergleichRepo.GetAll().ToList();
                                                             }
                                                           , () => ReloadCommandCanExecute) );
            }
        }

        public bool ReloadCommandCanExecute => true;

    }
}
