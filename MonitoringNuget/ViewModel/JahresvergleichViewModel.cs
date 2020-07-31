using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.MonitoringControl.Commands;
using MonitoringNuget.Strategy;

namespace MonitoringNuget.ViewModel
{
    public class JahresvergleichViewModel : DependencyObject
    {
        public static ContextStrategy<Jahresvergleich> kundenRepo = new ContextStrategy<Jahresvergleich>(new JahresvergleichRepo(), new EFProcedure());

        public static readonly DependencyProperty JahresvergleichProperty = DependencyProperty.Register("Jahresvergleich"
                                                                                                      , typeof(List<Jahresvergleich>)
                                                                                                      , typeof(JahresvergleichViewModel)
                                                                                                      , new UIPropertyMetadata(kundenRepo.GetAll().ToList()));

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
                                                                 Jahresvergleich = kundenRepo.GetAll().ToList();
                                                             }
                                                           , () => ReloadCommandCanExecute) );
            }
        }

        public bool ReloadCommandCanExecute => true;

    }
}
