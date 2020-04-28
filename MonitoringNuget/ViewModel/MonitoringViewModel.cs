using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DuplicateCheckerLib;
using MonitoringNuget.Annotations;
using MonitoringNuget.EntityClasses;
using MonitoringNuget.MonitoringControl.View.Commands;

namespace MonitoringNuget.ViewModel
{
    public partial class MonitoringViewModel : DependencyObject, INotifyPropertyChanged
    {
        private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
            DependencyProperty.Register("Logentries", typeof(DataTable), typeof(MonitoringViewModel));

        // Logmessage hinzufügen
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message"
                                      , typeof(string)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty SeverityProperty =
            DependencyProperty.Register("Severity", typeof(DataTable), typeof(MonitoringViewModel));

        public static readonly DependencyProperty SelectedIndexSeverityProperty =
            DependencyProperty.Register("SelectedIndexSeverity"
                                      , typeof(int)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty DevicesProperty =
            DependencyProperty.Register("Devices", typeof(DataTable), typeof(MonitoringViewModel));

        public static readonly DependencyProperty SelectedindexDevicesProperty =
            DependencyProperty.Register("SelectedindexDevices"
                                      , typeof(int)
                                      , typeof(MonitoringViewModel)
                                      , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty DuplicateListProperty =
            DependencyProperty.Register("DuplicateList", typeof(List<LogentriesEntity>), typeof(MonitoringViewModel));

        #endregion

        #region Binding Properties

        #region Database Connection

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

        private Visibility _usercontrolVisibility = Visibility.Visible;
        public Visibility UsercontrolVisibility
        {
            get => _usercontrolVisibility;
            set
            {
                _usercontrolVisibility = value;
                OnPropertyChanged(nameof(UsercontrolVisibility));
            }
        }

        private Visibility _duplicatesFindVisibility = Visibility.Hidden;
        public Visibility DuplicatesFindVisibility
        {
            get => _duplicatesFindVisibility;
            set
            {
                _duplicatesFindVisibility = value;
                OnPropertyChanged(nameof(DuplicatesFindVisibility));
            }
        }

        #endregion

        // Monitoring 
        public int SelectedIndex
        {
            get => (int) GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public DataTable Logentries
        {
            get => (DataTable) GetValue(LogentriesProperty);
            set => SetValue(LogentriesProperty, value);
        }

        // Logmessage hinzufügen
        public string Message
        {
            get => (string) GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
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

        public DataTable Devices
        {
            get => (DataTable) GetValue(DevicesProperty);
            set => SetValue(DevicesProperty, value);
        }

        public int SelectedindexDevices
        {
            get => (int) GetValue(SelectedindexDevicesProperty);
            set => SetValue(SelectedindexDevicesProperty, value);
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
            get
            {
                return _loadCommand
                    ?? ( _loadCommand = new CommandHandler(() => Logentries = Select(), () => LoadCanExecute) );
            }
        }

        public bool LoadCanExecute => true;

        private ICommand _logClearCommand;

        public ICommand LogClearCommand
        {
            get
            {
                return _logClearCommand
                    ?? ( _logClearCommand = new CommandHandler(() => LogClear(), () => LogCanExecute) );
            }
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
                                                                  SelectedIndexSeverity = -1;
                                                                  SelectedindexDevices  = -1;
                                                                  Message               = string.Empty;
                                                              }
                                                            , () => AddCanExecute) );
            }
        }

        public bool AddCanExecute
            => SelectedindexDevices >= 0 && SelectedIndexSeverity >= 0 && !string.IsNullOrEmpty(Message);

        private ICommand _addConnectionstringCommand;
        public ICommand AddConnectionstringCommand
        {
            get
            {
                return _addConnectionstringCommand
                    ?? ( _addConnectionstringCommand = new CommandHandler(() =>
                                                                          {
                                                                              SetConnectionString();
                                                                              UsercontrolVisibility =
                                                                                  Visibility.Hidden;
                                                                              DuplicatesFindVisibility =
                                                                                  Visibility.Visible;
                                                                          }
                                                                        , () => AddconnectionstringCanExecute) );
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
            get
            {
                return _findDuplicates
                    ?? ( _findDuplicates =
                           new CommandHandler(() => { GetDuplicates(); }, () => FindDuplicatesCanExecute) );
            }
        }

        public bool FindDuplicatesCanExecute => true;

        #endregion

        #region Methoden

        // Monitoring
        /// <summary>
        ///     Selectiert alle Datensätze der View v_Logentries
        /// </summary>
        /// <returns></returns>
        private DataTable Select()
        {
            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    var dataAdapter = new SqlDataAdapter(new SqlCommand(v_Logentries, conn));
                    dataAdapter.Fill(dt);
                }
            }
            catch (Exception)
            {
                DuplicatesFindVisibility = Visibility.Hidden;
                UsercontrolVisibility    = Visibility.Visible;
                return dt;
            }

            return dt;
        }

        /// <summary>
        ///     Bestätigt den ausgewählten Datensatz im Datagrid Logmessages
        /// </summary>
        private void LogClear()
        {
            var bOk = int.TryParse(Logentries.Rows[SelectedIndex]["Id"].ToString(), out var logId);
            if (bOk)
                try
                {
                    using (var conn = new SqlConnection(_builder.ConnectionString))
                    {
                        using (var cmd = new SqlCommand("LogClear", conn))
                        {
                            cmd.CommandType                                = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = logId;
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    Logentries = Select();
                }
                catch (Exception)
                {
                    DuplicatesFindVisibility = Visibility.Hidden;
                    UsercontrolVisibility    = Visibility.Visible;
                }
        }

        // Logmessage hinzufügen
        /// <summary>
        ///     Selectiert alle Geräte der Datenbank
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable SelectDevices()
        {
            var dt = new DataTable();

            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    var dataAdapter = new SqlDataAdapter(new SqlCommand(selectDevices, conn));
                    dataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                DuplicatesFindVisibility = Visibility.Hidden;
                UsercontrolVisibility    = Visibility.Visible;
                return dt;
            }
        }

        /// <summary>
        ///     Selektiert alle Datensätze aus der Severity Tabelle
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable SelectSeverity()
        {
            var dt = new DataTable();
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    var dataAdapter = new SqlDataAdapter(new SqlCommand("SELECT * FROM Severity", conn));
                    dataAdapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception)
            {
                DuplicatesFindVisibility = Visibility.Hidden;
                UsercontrolVisibility    = Visibility.Visible;
                return dt;
            }
        }

        /// <summary>
        ///     Fügt mithilfe der Stored Procedure einen neuen Log-Eintrag hinzu.
        /// </summary>
        private void AddMessage()
        {
            var hostname = Devices.Rows[SelectedindexDevices]["hostname"].ToString();
            var podName = Devices.Rows[SelectedindexDevices]["PodName"].ToString();
            var severityId = Convert.ToInt32(Severity.Rows[SelectedIndexSeverity]["Id"].ToString());
            try
            {
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    using (var cmd = new SqlCommand("LogMessageAdd", conn))
                    {
                        cmd.CommandType                                             = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@logmessage", SqlDbType.NVarChar).Value = Message;
                        cmd.Parameters.Add("@PodName", SqlDbType.NVarChar).Value    = podName;
                        cmd.Parameters.Add("@Severity", SqlDbType.Int).Value        = severityId;
                        cmd.Parameters.Add("@hostname", SqlDbType.NVarChar).Value   = hostname;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                DuplicatesFindVisibility = Visibility.Hidden;
                UsercontrolVisibility    = Visibility.Visible;
            }
        }

        /// <summary>
        ///     Bildet den Connectionstring und aktualisiert das Severity und Geräte Datagrid
        /// </summary>
        private void SetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
                          {
                              DataSource     = Datasource
                            , InitialCatalog = DatabaseName
                            , UserID         = LoggingUserId
                            , Password       = LoggingPassword
                          };

            _builder = builder;

            Severity = SelectSeverity();
            Devices  = SelectDevices();
        }

        /// <summary>
        /// Findet alle Duplikate.
        /// </summary>
        private void GetDuplicates()
        {
            var dupliTable = Select();
            var dupliChecker = new DuplicateChecker();
            var logentryList = new List<LogentriesEntity>();

            for (var i = 0; i < dupliTable.Rows.Count; i++)
            {
                var entity = new LogentriesEntity
                             {
                                 Id         = Convert.ToInt32(dupliTable.Rows[i]["Id"].ToString())
                               , Severity   = Convert.ToInt32(dupliTable.Rows[i]["severity"].ToString())
                               , Logmessage = dupliTable.Rows[i]["message"].ToString()
                             };

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