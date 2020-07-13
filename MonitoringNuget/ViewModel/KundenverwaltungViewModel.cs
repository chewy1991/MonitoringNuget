using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.EntityClasses;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.MonitoringControl.Commands;
using MonitoringNuget.Strategy;

namespace MonitoringNuget.ViewModel
{
    public class KundenverwaltungViewModel : DependencyObject
    {
        public static ContextStrategy<Kunde> kundenRepo = new ContextStrategy<Kunde>(new KundenRepository());

        public static readonly DependencyProperty KundenlistProperty = DependencyProperty.Register("Kundenlist"
                                                                                                    , typeof(List<Kunde>)
                                                                                                    , typeof(KundenverwaltungViewModel)
                                                                                                   , new UIPropertyMetadata(kundenRepo.GetAll()));


        public static readonly DependencyProperty SelectedClientProperty = DependencyProperty.Register("SelectedClient"
                                                                                                     , typeof(int)
                                                                                                     , typeof(KundenverwaltungViewModel)
                                                                                                     , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText"
                                                                                                 , typeof(string)
                                                                                                 , typeof(KundenverwaltungViewModel)
                                                                                                 , new UIPropertyMetadata(string.Empty));

        public string SearchText
        {
            get => (string) GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        public int SelectedClient
        {
            get => (int) GetValue(SelectedClientProperty);
            set => SetValue(SelectedClientProperty, value);
        }

        public List<Kunde> Kundenlist
        {
            get => (List<Kunde>) GetValue(KundenlistProperty);
            set => SetValue(KundenlistProperty, value);
        }

        private ICommand _addClientCommand;

        public ICommand AddClientCommand
        {
            get { return _addClientCommand ?? ( _addClientCommand = new CommandHandler(() =>
            {
                foreach (Kunde client in Kundenlist)
                {
                    kundenRepo.Update(client);
                }
                Kundenlist = kundenRepo.GetAll();
            }, () => AddClientCanExecute) ); }
        }

        public bool AddClientCanExecute => true; 

        private ICommand _delClientCommand;

        public ICommand DelClientCommand
        {
            get
            {
                return _delClientCommand
                    ?? ( _delClientCommand = new CommandHandler(() =>
                                                                {
                                                                    var client = Kundenlist[SelectedClient];
                                                                    kundenRepo.Delete(client);
                                                                    SelectedClient = -1;
                                                                    Kundenlist = kundenRepo.GetAll();
                                                                }
                                                              , () => DelClientCanExecute) );
            }
        }

        public bool DelClientCanExecute
            => SelectedClient >= 0;

        private ICommand _showallclientcommand;

        public ICommand ShowAllClientCommand
        {
            get
            {
                return _showallclientcommand ?? (_showallclientcommand = new CommandHandler(() =>
                {
                    Kundenlist = kundenRepo.GetAll();
                    SelectedClient = -1;
                }, () => ShowAllCLientCanExecute ));
            }
        }

        public bool ShowAllCLientCanExecute => true;

        private ICommand _searchallclientcommand;

        public ICommand SearchClientCommand
        {
            get
            {
                return _searchallclientcommand
                    ?? ( _searchallclientcommand = new CommandHandler(() =>
                                                                      {
                                                                          var searchdict = new Dictionary<string,object>();
                                                                          searchdict.Add("Bezeichnung",SearchText);
                                                                          Kundenlist     = kundenRepo.GetAll("",searchdict);
                                                                          SelectedClient = -1;
                                                                      }
                                                                    , () => SearchCLientCanExecute) );
            }
        }

        public bool SearchCLientCanExecute => !string.IsNullOrEmpty(SearchText);
    }
}
