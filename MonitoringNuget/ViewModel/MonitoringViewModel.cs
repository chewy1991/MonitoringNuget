using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using MonitoringNuget.MonitoringControl.View.Commands;

namespace MonitoringNuget.ViewModel
{
    public partial class MonitoringViewModel : DependencyObject
    {
        #region Dependency Properties

        // Monitoring Part
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex"
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

        public static readonly DependencyProperty SeverityProperty = DependencyProperty.Register("Severity"
                                                                                               , typeof(DataTable)
                                                                                               , typeof(MonitoringViewModel)
                                                                                               , new UIPropertyMetadata(SelectSeverity()));

        public static readonly DependencyProperty SelectedIndexSeverityProperty = DependencyProperty.Register("SelectedIndexSeverity"
                                                                                                            , typeof(int)
                                                                                                            , typeof(MonitoringViewModel)
                                                                                                            , new UIPropertyMetadata(-1));

        public static readonly DependencyProperty DevicesProperty = DependencyProperty.Register("Devices"
                                                                                              , typeof(DataTable)
                                                                                              , typeof(MonitoringViewModel)
                                                                                              , new UIPropertyMetadata(SelectDevices()));

        public static readonly DependencyProperty SelectedindexDevicesProperty = DependencyProperty.Register("SelectedindexDevices"
                                                                                                           , typeof(int)
                                                                                                           , typeof(MonitoringViewModel)
                                                                                                           , new UIPropertyMetadata(-1));

        #endregion

        #region Binding Properties

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
                                                                  SelectedIndexSeverity = -1;
                                                                  SelectedindexDevices  = -1;
                                                                  Message               = string.Empty;
                                                              }
                                                            , () => AddCanExecute) );
            }
        }

        public bool AddCanExecute => SelectedindexDevices >= 0 && SelectedIndexSeverity >= 0 && !string.IsNullOrEmpty(Message);

        #endregion

        #region Methoden

        // Monitoring
        private static DataTable Select()
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetDbConnection())
                {
                    var dataAdapter = new SqlDataAdapter(new SqlCommand(v_Logentries, conn));
                    dataAdapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return dt;
            }
        }

        private void LogClear()
        {
            var bOk = int.TryParse(Logentries.Rows[SelectedIndex]["Id"].ToString(), out var logId);
            if (bOk)
                try
                {
                    using (var conn = GetDbConnection())
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
                catch (Exception e) { MessageBox.Show(e.Message); }
        }

        // Logmessage hinzufügen
        private static DataTable SelectDevices()
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetDbConnection())
                {
                    var dataAdapter = new SqlDataAdapter(new SqlCommand(selectDevices, conn));
                    dataAdapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return dt;
            }
        }

        private static DataTable SelectSeverity()
        {
            var dt = new DataTable();
            try
            {
                using (var conn = GetDbConnection())
                {
                    var dataAdapter = new SqlDataAdapter(new SqlCommand("SELECT * FROM Severity", conn));
                    dataAdapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return dt;
            }
        }

        private void AddMessage()
        {
            var hostname = Devices.Rows[SelectedindexDevices]["hostname"].ToString();
            var podName = Devices.Rows[SelectedindexDevices]["PodName"].ToString();
            var severityId = Convert.ToInt32(Severity.Rows[SelectedIndexSeverity]["Id"].ToString());
            try
            {
                using (var conn = GetDbConnection())
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
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private static SqlConnection GetDbConnection()
        {
            var builder = new SqlConnectionStringBuilder
                          {
                              DataSource = Datasource, InitialCatalog = InitialCatalog, UserID = UserId, Password = Password
                          };
            var connection = new SqlConnection(builder.ConnectionString);

            return connection;
        }

        #endregion
    }
}