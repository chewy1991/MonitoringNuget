using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using DuplicateCheckerLib;
using MonitoringNuget.DataAccess.RepositoryClasses;
using MonitoringNuget.EntityClasses;
using MonitoringNuget.Models;
using MonitoringNuget.MonitoringControl.Commands;

namespace MonitoringNuget.ViewModel
{
    public class MonitoringViewModel : DependencyObject
    {
        private readonly LoggingRepository loggingrepo = new LoggingRepository();

        private bool ConnstringSet = false;

        #region Dependency Properties

        public static readonly DependencyProperty DatasourceProperty =
            DependencyProperty.Register("Datasource"
                                      , typeof(string)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(@".\ZBW"));

        public static readonly DependencyProperty DatabaseNameProperty =
            DependencyProperty.Register("DatabaseName"
                                      , typeof(string)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata("testat"));

        public static readonly DependencyProperty LoggingUserIdProperty =
            DependencyProperty.Register("LoggingUserId"
                                      , typeof(string)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata("MonitoringWPF"));

        public static readonly DependencyProperty LoggingPasswordProperty =
            DependencyProperty.Register("LoggingPassword"
                                      , typeof(string)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata("monitoring123"));

        // Monitoring Part
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex"
                                      , typeof(int)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty LogentriesProperty =
            DependencyProperty.Register("Logentries", typeof(List<Logging>), typeof(MonitoringViewModel));

        // Logmessage hinzufügen
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message"
                                                                                              , typeof(string)
                                                                                              , typeof(MonitoringViewModel)
                                                                                              , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register("Severity"
                                      , typeof(DataTable)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(FillSeverity()));

        public static readonly DependencyProperty SelectedIndexSeverityProperty =
            DependencyProperty.Register("SelectedIndexSeverity"
                                      , typeof(int)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty LogmessageGridRowSpanProperty =
            DependencyProperty.Register("LogmessageGridRowSpan"
                                      , typeof(int)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(2));

        public static readonly DependencyProperty PodNameProperty = DependencyProperty.Register("PodName"
                                                                                              , typeof(string)
                                                                                              , typeof(MonitoringViewModel)
                                                                                              , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty HostNameProperty =
            DependencyProperty.Register("HostName"
                                      , typeof(string)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DuplicateListProperty =
            DependencyProperty.Register("DuplicateList", typeof(List<LogentriesEntity>), typeof(MonitoringViewModel));

        public static readonly DependencyProperty UsercontrolVisibilityProperty =
            DependencyProperty.Register("UsercontrolVisibility"
                                      , typeof(Visibility)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(Visibility.Visible));

        #endregion

        #region Binding Properties

        #region Database Connection

        public int LogmessageGridRowSpan
        {
            get => (int) GetValue(LogmessageGridRowSpanProperty);
            set => SetValue(LogmessageGridRowSpanProperty, value);
        }

        public string Datasource
        {
            get => (string) GetValue(DatasourceProperty);
            set => SetValue(DatasourceProperty, value);
        }

        public string DatabaseName
        {
            get => (string) GetValue(DatabaseNameProperty);
            set => SetValue(DatabaseNameProperty, value);
        }

        public string LoggingUserId
        {
            get => (string) GetValue(LoggingUserIdProperty);
            set => SetValue(LoggingUserIdProperty, value);
        }

        public string LoggingPassword
        {
            get => (string) GetValue(LoggingPasswordProperty);
            set => SetValue(LoggingPasswordProperty, value);
        }

        #endregion

        #region UserControl Property

        //private Visibility _usercontrolVisibility = Visibility.Visible;
        public Visibility UsercontrolVisibility
        {
            get => (Visibility) GetValue(UsercontrolVisibilityProperty);
            set => SetValue(UsercontrolVisibilityProperty, value);
        }

        #endregion

        // Monitoring 
        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public List<Logging> Logentries
        {
            get => (List<Logging>) GetValue(LogentriesProperty);
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

        public bool LoadCanExecute => ConnstringSet;

        private ICommand _logClearCommand;

        public ICommand LogClearCommand
        {
            get { return _logClearCommand ?? ( _logClearCommand = new CommandHandler(() => LogClear(), () => LogCanExecute) ); }
        }

        public bool LogCanExecute => SelectedIndex >= 0 && ConnstringSet;
        private ICommand _addDataCommand;

        public ICommand AddDataCommand
        {
            get
            {
                return _addDataCommand
                    ?? ( _addDataCommand = new CommandHandler(() =>
                                                              {
                                                                  AddMessage();
                                                                  Message = string.Empty;
                                                                  PodName = string.Empty;
                                                                  HostName = string.Empty;
                                                                  SelectedIndexSeverity = -1;
                                                              }
                                                            , () => AddCanExecute) );
            }
        }

        public bool AddCanExecute => !string.IsNullOrEmpty(Message) 
                                  && !string.IsNullOrEmpty(PodName) 
                                  && !string.IsNullOrEmpty(HostName) 
                                  && SelectedIndexSeverity >= 0 
                                  && ConnstringSet;

        private ICommand _addConnectionstringCommand;
        public ICommand AddConnectionstringCommand
        {
            get
            {
                return _addConnectionstringCommand
                    ?? ( _addConnectionstringCommand = new CommandHandler(() => { SetConnectionString();}, () => AddconnectionstringCanExecute));
            }
        }

        public bool AddconnectionstringCanExecute
            => !string.IsNullOrEmpty(Datasource)
            && !string.IsNullOrEmpty(DatabaseName)
            && !string.IsNullOrEmpty(LoggingUserId)
            && !string.IsNullOrEmpty(LoggingPassword);

        private ICommand _findDuplicates;

        public ICommand FindDuplicates
        {
            get { return _findDuplicates ?? ( _findDuplicates = new CommandHandler(() => { GetDuplicates(); }, () => FindDuplicatesCanExecute) ); }
        }

        public bool FindDuplicatesCanExecute => ConnstringSet;

        #endregion

        #region Methoden

        private void GetLogentries()
        {
            try { Logentries = loggingrepo.GetAll(); }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        /// <summary>
        ///     Bestätigt den ausgewählten Datensatz im Datagrid Logmessages
        /// </summary>
        private void LogClear()
        {
            var bOk = loggingrepo.LogClear(Logentries[SelectedIndex].Id);
            if (bOk) { Logentries = loggingrepo.GetAll(); }
            else
            {
                LogmessageGridRowSpan = 2;
                UsercontrolVisibility = Visibility.Visible;
            }
        }

        /// <summary>
        ///     Fügt mithilfe der Stored Procedure einen neuen Log-Eintrag hinzu.
        /// </summary>
        private void AddMessage()
        {
            var severityId = (int) Severity.Rows[SelectedIndexSeverity]["Id"];
            var bOK = loggingrepo.AddMessage(Message
                                           , PodName
                                           , severityId
                                           , HostName);
            if (!bOK)
            {
                LogmessageGridRowSpan = 2;
                UsercontrolVisibility = Visibility.Visible;
            }
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
                row["Id"] = key;
                row["Severity"] = severityDict[key];
                severityTable.Rows.Add(row);
            }

            return severityTable;
        }

        /// <summary>
        ///     Bildet den Connectionstring und aktualisiert das Severity und Geräte Datagrid
        /// </summary>
        private void SetConnectionString()
        {
            var bOK = loggingrepo.SetConnectionstring(Datasource,DatabaseName,LoggingUserId,LoggingPassword);

            if (bOK)
            {
                UsercontrolVisibility = Visibility.Hidden;
                LogmessageGridRowSpan = 3;
                ConnstringSet = true;
            }
        }

        /// <summary>
        ///     Findet alle Duplikate.
        /// </summary>
        private void GetDuplicates()
        {
            var loglist = loggingrepo.GetAll();
            var dupliChecker = new DuplicateChecker();
            var logentryList = new List<LogentriesEntity>();

            foreach (var log in loglist)
            {
                var entity = new LogentriesEntity {Id = log.Id, Severity = log.severity, Logmessage = log.message};

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