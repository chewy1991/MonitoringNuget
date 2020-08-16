using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PluginContracts;

namespace DataSerializers
{
    public class BinaryDataExporter : IDataExportPlugin
    {
        /// <summary>
        /// Name des DataExporters
        /// </summary>
        public string Name { get; } = "BinaryExporter";

        /// <summary>
        /// Ausführung des Export.
        /// </summary>
        /// <param name="data">Collection, die exportiert werden soll</param>
        /// <param name="destinationPath">Zielpfad des Exports (Verzeichnis und Dateiname)</param>
        public void Export(IEnumerable data, string destinationPath)
        {
            var stream = new FileStream(destinationPath,FileMode.Create);

            var binaryformatter = new BinaryFormatter();
            binaryformatter.Serialize(stream, data);
        }
    }
}
