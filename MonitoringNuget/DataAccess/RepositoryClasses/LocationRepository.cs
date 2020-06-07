using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using MonitoringNuget.DataAccess.AbstractClass;
using MonitoringNuget.Models;

namespace MonitoringNuget.DataAccess.RepositoryClasses
{
    internal class LocationRepository : RepositoryBase<Location>
    {
        /// <summary>Gibt den Tabellennamen zurück, auf die sich das Repository bezieht</summary>
        public override string TableName => "Locations";

        /// <summary>Aktualisiert das Model-Objekt in der Datenbank hinzu (Update)</summary>
        /// <param name="entity">zu aktualisierendes Model-Object</param>
        public override void Update(Location entity)
        {
            if (this.GetSingle(entity.Id) != null)
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText =
                            $"UPDATE {TableName} SET Id = {entity.Id}, LocationName = {entity.LocationName}, Locationparent = {entity.Locationparent};";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        ///     Gibt eine Liste von Model-Objekten vom Typ M zurück,
        ///     die gemäss der WhereBedingung geladen wurden. Die Werte der
        ///     Where-Bedingung können als separat übergeben werden,
        ///     damit diese für PreparedStatements verwendet werden können.
        ///     (Verhinderung von SQL-Injection)
        /// </summary>
        /// <param name="whereCondition">
        ///     WhereBedingung als string
        ///     z.B. "NetPrice &gt; @netPrice and Active = @active and Description like @desc
        /// </param>
        /// <param name="parameterValues">
        ///     Parameter-Werte für die Wherebedingung
        ///     bspw: {{"netPrice", 10.5}, {"active", true}, {"desc", "Wolle%"}}
        /// </param>
        /// <returns></returns>
        public override List<Location> GetAll(string whereCondition, Dictionary<string, object> parameterValues)
        {
            var locationlist = new List<Location>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName} WHERE {whereCondition};";

                    foreach (var key in parameterValues)
                        cmd.Parameters.AddWithValue($@"@{key.Key}", key.Value);
                    cmd.Prepare();

                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        var loc = new Location
                                  {
                                      Id = (int) row["Id"]
                                    , LocationName = (string) row["LocationName"]
                                    , Locationparent = (int?) row["Locationparent"]
                                  };
                        locationlist.Add(loc);
                    }
                }
            }

            return locationlist;
        }

        /// <summary>Gibt eine Liste aller in der DB vorhandenen Model-Objekte vom Typ M zurück</summary>
        /// <returns></returns>
        public override List<Location> GetAll()
        {
            var locationlist = new List<Location>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName};";

                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        var loc = new Location
                                  {
                                      Id = (int) row["Id"]
                                    , LocationName = (string) row["LocationName"]
                                    , Locationparent = (int?) row["Locationparent"]
                                  };
                        locationlist.Add(loc);
                    }
                }
            }

            return locationlist;
        }

        /// <summary>
        ///     Liefert ein einzelnes Model-Objekt vom Typ M zurück,
        ///     welches anhand dem übergebenen PrimaryKey geladen wird.
        /// </summary>
        /// <typeparam name="P">Type des PrimaryKey</typeparam>
        /// <param name="pkValue">Wert des PrimaryKey</param>
        /// <returns>gefundenes Model-Objekt, ansonsten null</returns>
        public override Location GetSingle<P>(P pkValue)
        {
            Location location = null;
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = $"SELECT * FROM {TableName} WHERE Id = {pkValue};";

                    var dt = new DataTable();
                    var dataAdapter = new SqlDataAdapter(cmd.CommandText, conn);
                    dataAdapter.Fill(dt);

                    if (dt.Rows.Count == 1)
                    {
                        location = new Location
                                   {
                                       Id = (int) dt.Rows[0]["Id"]
                                     , LocationName = (string) dt.Rows[0]["LocationName"]
                                     , Locationparent = (int?) dt.Rows[0]["Locationparent"]
                                   };
                    }
                }
            }

            return location;
        }

        /// <summary>Fügt das Model-Objekt zur Datenbank hinzu (Insert)</summary>
        /// <param name="entity">zu speicherndes Model-Object</param>
        public override void Add(Location entity)
        {
            if (this.GetSingle(entity.Locationparent) != null)
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText =
                            $"INSERT INTO {TableName} (Id,LocationName,Locationparent) VALUES ({entity.Id},{entity.LocationName},{entity.Locationparent});";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}