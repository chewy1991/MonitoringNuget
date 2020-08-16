using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MonitoringNuget.DataAccess.EFAccess;
using MonitoringNuget.DataAccess.LinqToSQL;
using MonitoringNuget.DataAccess.StoredProcedures;
using MonitoringNuget.EntityFramework;
using MonitoringNuget.LinqDTO;
using MonitoringNuget.MonitoringControl.Commands;
using MonitoringNuget.Strategy;
using PluginContracts;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace MonitoringNuget.ViewModel
{
    public class PluginLoaderViewModel: DependencyObject
    {
        private static Dictionary<string, object> assemblies = new Dictionary<string, object>();
        private static ContextStrategy<Kunde> kundenRepo = new ContextStrategy<Kunde>(new KundenRepository(), new EFProcedure());
        private static ContextStrategy<VLogentriesDTO> logRepo = new ContextStrategy<VLogentriesDTO>(new LogentriesRepositoryLinq(), new LinqStoredProcedure());

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data"
                                                                                           , typeof(List<string>)
                                                                                           , typeof(PluginLoaderViewModel)
                                                                                           , new UIPropertyMetadata(new List<string>
                                                                                                                    {
                                                                                                                        "Kunden", "Logentries"
                                                                                                                    }));
        public static readonly DependencyProperty ExporterProperty = DependencyProperty.Register("Exporter"
                                                                                           , typeof(List<string>)
                                                                                           , typeof(PluginLoaderViewModel)
                                                                                           , new UIPropertyMetadata(LoadAssemblies()));

        public static readonly DependencyProperty ExporterNameProperty = DependencyProperty.Register("ExporterName"
                                                                                               , typeof(string)
                                                                                               , typeof(PluginLoaderViewModel));

        public static readonly DependencyProperty SelectedDataProperty = DependencyProperty.Register("SelectedData"
                                                                                                   , typeof(string)
                                                                                                   , typeof(PluginLoaderViewModel));


        public List<string> Data
        {
            get => (List<string>) GetValue(DataProperty);
            
        }

        public string SelectedData
        {
            get => (string) GetValue(SelectedDataProperty);
            set => SetValue(SelectedDataProperty, value);
        }

        public List<string> Exporter
        {
            get => (List<string>) GetValue(ExporterProperty);
            
        }

        public string ExporterName
        {
            get => (string) GetValue(ExporterNameProperty);
            set => SetValue(ExporterNameProperty, value);
        }

        private ICommand _doExportCommand;

        public ICommand DoExportCommand
        {
            get
            {
                return _doExportCommand
                    ?? ( _doExportCommand = new CommandHandler(() =>
                                                               {
                                                                   Export();
                                                               }
                                                             , () => DoExportCommandCanExecute) );
            }
        }

        public bool DoExportCommandCanExecute => true;

        private static List<string> LoadAssemblies()
        {
            var assemblyList = new List<string>();

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            path = Path.Combine(path, "Exporter");

            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                var a = Assembly.LoadFrom(dll);
                foreach (var t in a.GetTypes())
                {
                    if (typeof(IDataExportPlugin).IsAssignableFrom(t))
                    {
                        var obj = Activator.CreateInstance(t);
                        var propInfo = t.GetProperty("Name");
                        var name = Convert.ToString(propInfo.GetValue(obj));

                        assemblyList.Add(name);
                        assemblies.Add(name, obj);
                    }
                }
            }

            return assemblyList;
        }

        private void Export()
        {
            var safe = new SaveFileDialog();
            var path = string.Empty;

            var exporter = ExporterName;
            IEnumerable data;

            if (SelectedDataProperty.Equals("Kunden"))
                data = kundenRepo.GetAll().ToList();
            else
                data = logRepo.GetAll().ToList();

            var bOk = assemblies.TryGetValue(exporter, out object exporterobj);
            if (bOk)
            {
                var exp = exporterobj.GetType().GetMethod("Export");
                if (safe.ShowDialog() == DialogResult.OK)
                {
                    path = safe.FileName;
                    exp.Invoke(exporterobj, new object[] {data, path});
                }
                
            }
            
        }
    }
}
