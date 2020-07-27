using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MonitoringNuget.CTEClass;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.EntityClasses;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.MonitoringControl.Commands;
using MonitoringNuget.Strategy;
using MonitoringNuget.RegexValidation;
using TreeView = System.Windows.Controls.TreeView;

namespace MonitoringNuget.ViewModel
{
    public class KundenverwaltungViewModel : DependencyObject
    {
        public static ContextStrategy<Kunde> kundenRepo = new ContextStrategy<Kunde>(new KundenRepository(), new EFProcedure());

        public static readonly DependencyProperty KundenlistProperty = DependencyProperty.Register("Kundenlist"
                                                                                                    , typeof(List<Kunde>)
                                                                                                    , typeof(KundenverwaltungViewModel)
                                                                                                   , new UIPropertyMetadata(kundenRepo.GetAll().ToList()));


        public static readonly DependencyProperty SelectedClientProperty = DependencyProperty.Register("SelectedClient"
                                                                                                     , typeof(int)
                                                                                                     , typeof(KundenverwaltungViewModel)
                                                                                                     , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register("SearchText"
                                                                                                 , typeof(string)
                                                                                                 , typeof(KundenverwaltungViewModel)
                                                                                                 , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty LocationHierarchyProperty = DependencyProperty.Register("LocationHierarchy"
                                                                                                        , typeof(ObservableCollection<LocationHist>)
                                                                                                        , typeof(KundenverwaltungViewModel)
                                                                                                        , new UIPropertyMetadata(LoadHierarchyList()));

        public static readonly DependencyProperty KontonummerProperty = DependencyProperty.Register("Kontonummer"
                                                                                                  , typeof(string)
                                                                                                  , typeof(KundenverwaltungViewModel)
                                                                                                  , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty BezeichnungProperty = DependencyProperty.Register("Bezeichnung"
                                                                                                  , typeof(string)
                                                                                                  , typeof(KundenverwaltungViewModel)
                                                                                                  , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty GuthabenProperty = DependencyProperty.Register("Guthaben"
                                                                                                  , typeof(decimal)
                                                                                                  , typeof(KundenverwaltungViewModel));

        public static readonly DependencyProperty BetragslimitProperty = DependencyProperty.Register("Betragslimit"
                                                                                                   , typeof(decimal)
                                                                                                   , typeof(KundenverwaltungViewModel));

        public static readonly DependencyProperty TelefonnummerProperty = DependencyProperty.Register("Telefonnummer"
                                                                                                    , typeof(string)
                                                                                                    , typeof(KundenverwaltungViewModel)
                                                                                                    , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty EMailProperty = DependencyProperty.Register("EMail"
                                                                                            , typeof(string)
                                                                                            , typeof(KundenverwaltungViewModel)
                                                                                            , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty HomepageProperty = DependencyProperty.Register("Homepage"
                                                                                               , typeof(string)
                                                                                               , typeof(KundenverwaltungViewModel)
                                                                                               , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password"
                                                                                               , typeof(string)
                                                                                               , typeof(KundenverwaltungViewModel)
                                                                                               , new UIPropertyMetadata(string.Empty));

        public ObservableCollection<LocationHist> LocationHierarchy
        {
            get => (ObservableCollection<LocationHist>) GetValue(LocationHierarchyProperty);
            set => SetValue(LocationHierarchyProperty, value);
        }

        public int? Id { get; set; } = null;

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

        public string Kontonummer
        {
            get => (string) GetValue(KontonummerProperty);
            set => SetValue(KontonummerProperty, value);
        }

        public string Bezeichnung
        {
            get => (string) GetValue(BezeichnungProperty);
            set => SetValue(BezeichnungProperty, value);
        }

        public decimal Guthaben
        {
            get => (decimal) GetValue(GuthabenProperty);
            set => SetValue(GuthabenProperty, value);
        }

        public decimal Betragslimit
        {
            get => (decimal) GetValue(BetragslimitProperty);
            set => SetValue(BetragslimitProperty, value);
        }

        public string Telefonnummer
        {
            get => (string) GetValue(TelefonnummerProperty);
            set => SetValue(TelefonnummerProperty, value);
        }

        public string EMail
        {
            get => (string) GetValue(EMailProperty);
            set => SetValue(EMailProperty, value);
        }

        public string Homepage
        {
            get => (string) GetValue(HomepageProperty);
            set => SetValue(HomepageProperty, value);
        } 
        public string Password
        {
            get => (string) GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        private ICommand _addClientCommand;

        public ICommand AddClientCommand
        {
            get { return _addClientCommand ?? ( _addClientCommand = new CommandHandler(() =>
            {
                AddorChangeClientData();
                Kundenlist = kundenRepo.GetAll().ToList();
            }, () => AddClientCanExecute) ); }
        }

        public bool AddClientCanExecute
            => RegexValidation.RegexValidation.CustomerNrValidation(Kontonummer)
            && RegexValidation.RegexValidation.PasswordValidation(Password)
            && ( RegexValidation.RegexValidation.EMailValidation(EMail) || string.IsNullOrEmpty(EMail) )
            && ( RegexValidation.RegexValidation.HomepageValidation(Homepage) || string.IsNullOrEmpty(Homepage) ); 

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
                                                                    Kundenlist = kundenRepo.GetAll().ToList();
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
                    Kundenlist = kundenRepo.GetAll().ToList<Kunde>();
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
                                                                          Kundenlist     = kundenRepo.GetAll("",searchdict).ToList();
                                                                          SelectedClient = -1;
                                                                      }
                                                                    , () => SearchCLientCanExecute) );
            }
        }

        public bool SearchCLientCanExecute => !string.IsNullOrEmpty(SearchText);

        private ICommand _refreshHierarchieCommand;

        public ICommand RefreshHierarchieCommand
        {
            get
            {
                return _refreshHierarchieCommand
                    ?? ( _refreshHierarchieCommand = new CommandHandler(() =>
                                                                        {
                                                                            LocationHierarchy = LoadHierarchyList();
                                                                        }
                                                                      , () => RefreshHierarchieCommandCanExecute) );
            }
        }

        public bool RefreshHierarchieCommandCanExecute => true;

        private ICommand _chooseClientCommand;

        public ICommand ChooseClientCommand
        {
            get
            {
                return _chooseClientCommand
                    ?? ( _chooseClientCommand = new CommandHandler(() => { ChooseClient(); }
                                                                 , () => ChooseClientCommandCanExecute) );
            }
        }

        public bool ChooseClientCommandCanExecute => SelectedClient >= 0;

        private static ObservableCollection<LocationHist> LoadHierarchyList()
        {
            var lstPodname = kundenRepo.LoadHierarchy().Select(x => x.PodName).Distinct();

            var nodelist = new ObservableCollection<LocationHist>();

            foreach (var pod in lstPodname)
            {
                var locroot = new LocationHist() {Name = pod};

                var query = kundenRepo.LoadHierarchy().Where((x) => x.Locationparent == null && x.PodName.Equals(pod));

                foreach (var node in query)
                {
                    var loc = new LocationHist() {Name     = node.Locationname, Id = node.Id};
                    var childNodes = kundenRepo.LoadHierarchy();

                    loc.Add(childNodes.ToList());

                    locroot.LocationChilds.Add(loc);
                }

                nodelist.Add(locroot);
            }

            return nodelist;
        }

        private void AddorChangeClientData()
        {
            var client = new Kunde();
            if (Id != null)
                client.Id = (int)Id;

            client.Guthaben = Guthaben;
            client.Betragslimit = Betragslimit;
            if(!string.IsNullOrEmpty(EMail))
                client.EMail = EMail.Trim();
            if(!string.IsNullOrEmpty(Homepage))
                client.Homepage = Homepage.Trim();
            client.Kontonr = Kontonummer;
            client.Bezeichnung = Bezeichnung;
            client.Password = SkryptPw();

            kundenRepo.Update(client);
            ClearProperties();

        }

        private void ChooseClient()
        {
            var client = new Kunde();
            client = Kundenlist[SelectedClient];

            Id = client.Id;
            Kontonummer = client.Kontonr;
            Bezeichnung = client.Bezeichnung;
            if(!string.IsNullOrEmpty(client.EMail))
                EMail = client.EMail.Trim();
            if(!string.IsNullOrEmpty(client.Homepage))
                Homepage = client.Homepage.Trim();
            Guthaben = client.Guthaben;
            Betragslimit = client.Betragslimit;
            Password = EnSkryptPw(client.Password);
        }

        private void ClearProperties()
        {
            Id = null;
            Kontonummer = string.Empty;
            Bezeichnung = string.Empty;
            EMail = string.Empty;
            Homepage = string.Empty;
            Guthaben = default(decimal);
            Betragslimit = default(decimal);
            Password = string.Empty;
        }

        private byte[] SkryptPw()
        {
            return Encoding.UTF8.GetBytes(Password);
        }

        private string EnSkryptPw(byte[] pw)
        {
            return Encoding.UTF8.GetString(pw);
        }
    }
}
