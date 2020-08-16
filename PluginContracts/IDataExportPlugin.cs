using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginContracts
{
    public interface IDataExportPlugin
    {
        /// <summary>
        /// Name des DataExporters
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Ausführung des Export.
        /// </summary>
        /// <param name="data">Collection, die exportiert werden soll</param>
        /// <param name="destinationPath">Zielpfad des Exports (Verzeichnis und Dateiname)</param>
        void Export(IEnumerable data, string destinationPath);
    }
}
