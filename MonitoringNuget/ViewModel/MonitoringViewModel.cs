using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DuplicateCheckerLib;
using GenericRepository;
using MonitoringNuget.DataAccess.LinqToSQL;
using MonitoringNuget.DataAccess.RepositoryClasses;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.DataAccess.StoredProcedures.Interface;
using MonitoringNuget.EntityClasses;
using MonitoringNuget.IoCContainer;
using MonitoringNuget.LinqDTO;
using MonitoringNuget.MonitoringControl.Commands;
using MonitoringNuget.Strategy;
using MonitoringNuget.ViewModel.Interface;

namespace MonitoringNuget.ViewModel
{
    public class MonitoringViewModel : DependencyObject, IViewModel
    {
        private ContextStrategy<VLogentriesDTO> loggingrepo;
        private IoCContainer<ContextStrategy<VLogentriesDTO>> ioCContainer = new IoCContainer<ContextStrategy<VLogentriesDTO>>();

        #region Dependency Properties

        // Monitoring Part
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex"
                                                                                                    , typeof(int)
                                                                                                    , typeof(MonitoringViewModel)
                                                                                                    , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty LogentriesProperty = DependencyProperty.Register("Logentries"
                                                                                                 , typeof(List<VLogentriesDTO>)
                                                                                                 , typeof(MonitoringViewModel));

        // Logmessage hinzufügen
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message"
                                                                                              , typeof(string)
                                                                                              , typeof(MonitoringViewModel)
                                                                                              , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register("Severity"
                                                                                               , typeof(DataTable)
                                                                                               , typeof(MonitoringViewModel)
                                                                                               , new UIPropertyMetadata(FillSeverity()));

        public static readonly DependencyProperty SelectedIndexSeverityProperty = DependencyProperty.Register("SelectedIndexSeverity"
                                                                                                            , typeof(int)
                                                                                                            , typeof(MonitoringViewModel)
                                                                                                            , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty PodNameProperty = DependencyProperty.Register("PodName"
                                                                                              , typeof(string)
                                                                                              , typeof(MonitoringViewModel)
                                                                                              , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty HostNameProperty = DependencyProperty.Register("HostName"
                                                                                               , typeof(string)
                                                                                               , typeof(MonitoringViewModel)
                                                                                               , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DuplicateListProperty = DependencyProperty.Register("DuplicateList"
                                                                                                    , typeof(List<LogentriesEntity>)
                                                                                                    , typeof(MonitoringViewModel));

        public static readonly DependencyProperty IsAdoProperty = DependencyProperty.Register("IsAdo"
                                                                                                , typeof(bool)
                                                                                                , typeof(MonitoringViewModel)
                                                                                                , new PropertyMetadata(true));

        public static readonly DependencyProperty IsLinqProperty = DependencyProperty.Register("IsLinq"
                                                                                                 , typeof(bool)
                                                                                                 , typeof(MonitoringViewModel)
                                                                                                 , new PropertyMetadata(false));

        #endregion

        public MonitoringViewModel()
        {
            ChooseRepo();
        }

        #region Binding Properties

        // Monitoring 
        public bool IsAdo
        {
            get => (bool) GetValue(IsAdoProperty);
            set => SetValue(IsAdoProperty, value);
        }

        public bool IsLinq
        {
            get => (bool) GetValue(IsLinqProperty);
            set => SetValue(IsLinqProperty, value);
        }

        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public List<VLogentriesDTO> Logentries
        {
            get => (List<VLogentriesDTO>) GetValue(LogentriesProperty);
            set => SetValue(LogentriesProperty, value);
        }

        // Logmessage hinzufügen
        public string Message
        {
            get => (string) GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public string PodName
        {
            get => (string) GetValue(PodNameProperty);
            set => SetValue(PodNameProperty, value);
        }

        public string HostName
        {
            get => (string) GetValue(HostNameProperty);
            set => SetValue(HostNameProperty, value);
        }

        public DataTable Severity
        {
            get => (DataTable) GetValue(SeverityProperty);
            set => SetValue(SeverityProperty, value);
        }

        public int SelectedIndexSeverity
        {
            get => (int) GetValue(SelectedIndexSeverityProperty);
            set => SetValue(SelectedIndexSeverityProperty, value);
        }

        public List<LogentriesEntity> DuplicateList
        {
            get => (List<LogentriesEntity>) GetValue(DuplicateListProperty);
            set => SetValue(DuplicateListProperty, value);
        }

        #endregion

        #region Commandbindings

        // Monitoring
        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get { return _loadCommand ?? ( _loadCommand = new CommandHandler(() => GetLogentries(), () => LoadCanExecute) ); }
        }

        public bool LoadCanExecute => true;

        private ICommand _logClearCommand;

        public ICommand LogClearCommand
        {
            get { return _logClearCommand ?? ( _logClearCommand = new CommandHandler(() => LogClear(), () => LogCanExecute) ); }
        }

        public bool LogCanExecute => SelectedIndex >= 0;
        private ICommand _addDataCommand;

        public ICommand AddDataCommand
        {
            get
            {
                return _addDataCommand
                    ?? ( _addDataCommand = new CommandHandler(() =>
                                                              {
                                                                  AddMessage();
                                                                  Message               = string.Empty;
                                                                  PodName               = string.Empty;
                                                                  HostName              = string.Empty;
                                                                  SelectedIndexSeverity = -1;
                                                              }
                                                            , () => AddCanExecute) );
            }
        }

        public bool AddCanExecute
            => !string.IsNullOrEmpty(Message) && !string.IsNullOrEmpty(PodName) && !string.IsNullOrEmpty(HostName) && SelectedIndexSeverity >= 0;

        private ICommand _findDuplicates;

        public ICommand FindDuplicates
        {
            get { return _findDuplicates ?? ( _findDuplicates = new CommandHandler(() => { GetDuplicates(); }, () => FindDuplicatesCanExecute) ); }
        }

        public bool FindDuplicatesCanExecute => true;

        private ICommand _chooseRepoCommand;

        public ICommand ChooseRepoCommand
        {
            get { return _chooseRepoCommand ?? ( _chooseRepoCommand = new CommandHandler(() => { ChooseRepo(); }, () => ChooseRepoCanExecute) ); }
        }

        public bool ChooseRepoCanExecute => true;

        #endregion

        #region Methoden

        private void GetLogentries()
        {
            try { Logentries = loggingrepo.GetAll().ToList(); }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        /// <summary>
        ///     Bestätigt den ausgewählten Datensatz im Datagrid Logmessages
        /// </summary>
        private void LogClear()
        {
            loggingrepo.LogClear(Logentries[SelectedIndex].Id);
            Logentries = loggingrepo.GetAll().ToList();
        }
        
        private void ChooseRepo()
        {
            loggingrepo = IsAdo ? ioCContainer.ResolveContextStrategy<LoggingRepository, AdoProcedures>() : ioCContainer.ResolveContextStrategy<LogentriesRepositoryLinq, LinqStoredProcedure>();
        }

        /// <summary>
        ///     Fügt mithilfe der Stored Procedure einen neuen Log-Eintrag hinzu.
        /// </summary>
        private void AddMessage()
        {
            var severityId = (int) Severity.Rows[SelectedIndexSeverity]["Id"];
            loggingrepo.AddMessage(Message
                                 , PodName
                                 , severityId
                                 , HostName);
            Logentries = loggingrepo.GetAll().ToList();
        }

        private static DataTable FillSeverity()
        {
            var severityDict = new Dictionary<int, string>();
            severityDict.Add(10, "INFO");
            severityDict.Add(20, "DEBUG");
            severityDict.Add(30, "WARNING");
            severityDict.Add(40, "ERROR");
            severityDict.Add(50, "TRACE");
            var severityTable = new DataTable();
            severityTable.Columns.Add("Id", typeof(int));
            severityTable.Columns.Add("Severity", typeof(string));

            foreach (var key in severityDict.Keys)
            {
                var row = severityTable.NewRow();
                row["Id"]       = key;
                row["Severity"] = severityDict[key];
                severityTable.Rows.Add(row);
            }

            return severityTable;
        }

        /// <summary>
        ///     Findet alle Duplikate.
        /// </summary>
        private void GetDuplicates()
        {
            var loglist = loggingrepo.GetAll().ToList();
            var dupliChecker = new DuplicateChecker();
            var logentryList = new List<LogentriesEntity>();

            foreach (var log in loglist)
            {
                var entity = new LogentriesEntity {Id = log.Id, Severity = log.Severity, Logmessage = log.Message};

                logentryList.Add(entity);
            }

            var duplilist = dupliChecker.FindDuplicates(logentryList);
            var helplist = new List<LogentriesEntity>();
            foreach (var entity in duplilist)
            {
                var log = (LogentriesEntity) entity;
                helplist.Add(log);
            }

            DuplicateList = helplist;
        }

        #endregion
    }
}