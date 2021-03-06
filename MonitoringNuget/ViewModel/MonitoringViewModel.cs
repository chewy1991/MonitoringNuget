﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using MonitoringNuget.MonitoringControl.Commands;

namespace MonitoringNuget.ViewModel
{
    public class MonitoringViewModel : DependencyObject
    {
        private const string VLogentries = "Select id AS Id,pod,location,hostname,severity,timestamp,message  FROM v_logentries;";

        private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();

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

        #endregion

        #region Commandbindings

        // Monitoring
        private ICommand _loadCommand;

        public ICommand LoadCommand
        {
            get { return _loadCommand ?? ( _loadCommand = new CommandHandler(() => Logentries = Select(), () => LoadCanExecute) ); }
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

        private ICommand _addConnectionstringCommand;
        public ICommand AddConnectionstringCommand
        {
            get
            {
                return _addConnectionstringCommand
                    ?? ( _addConnectionstringCommand = new CommandHandler(() =>
                                                                          {
                                                                              SetConnectionString();
                                                                              UsercontrolVisibility = Visibility.Hidden;
                                                                              LogmessageGridRowSpan = 3;
                                                                          }
                                                                        , () => AddconnectionstringCanExecute) );
            }
        }

        public bool AddconnectionstringCanExecute
            => !string.IsNullOrEmpty(Datasource)
            && !string.IsNullOrEmpty(DatabaseName)
            && !string.IsNullOrEmpty(LoggingUserId)
            && !string.IsNullOrEmpty(LoggingPassword);

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
                    var dataAdapter = new SqlDataAdapter(new SqlCommand(VLogentries, conn));
                    dataAdapter.Fill(dt);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                LogmessageGridRowSpan = 2;
                UsercontrolVisibility = Visibility.Visible;
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
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    LogmessageGridRowSpan = 2;
                    UsercontrolVisibility = Visibility.Visible;
                }
        }

        /// <summary>
        ///     Fügt mithilfe der Stored Procedure einen neuen Log-Eintrag hinzu.
        /// </summary>
        private void AddMessage()
        {
            try
            {
                var severityId = Severity.Rows[SelectedIndexSeverity]["Id"];
                using (var conn = new SqlConnection(_builder.ConnectionString))
                {
                    using (var cmd = new SqlCommand("LogMessageAdd", conn))
                    {
                        cmd.CommandType                                             = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@logmessage", SqlDbType.NVarChar).Value = Message;
                        cmd.Parameters.Add("@PodName", SqlDbType.NVarChar).Value    = PodName;
                        cmd.Parameters.Add("@Severity", SqlDbType.Int).Value        = severityId;
                        cmd.Parameters.Add("@hostname", SqlDbType.NVarChar).Value   = HostName;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
                row["Id"]       = key;
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
            var builder = new SqlConnectionStringBuilder
                          {
                              DataSource = Datasource
                            , InitialCatalog = DatabaseName
                            , UserID = LoggingUserId
                            , Password = LoggingPassword
                          };

            _builder = builder;
        }

        #endregion
    }
}