﻿namespace MonitoringNuget.MonitoringControl.ViewModel
{
    public partial class MonitoringViewModel
    {
        private const string v_Logentries = "Select id AS Id,pod,location,hostname,severity,timestamp,message  FROM v_logentries;";
        private const string selectDevices = "Select pod.Bezeichnung AS PodName, Device.hostname, Device.ip_adresse, Device.anzahlports FROM Device INNER JOIN Adresse ON Device.AdressId = Adresse.id INNER JOIN point_of_delivery AS pod ON pod.podadresse = Adresse.id;";

        // Datenbank connection Infos
        private const string Datasource = @".\ZBW";
        private const string InitialCatalog = "testat";
        private const string UserId = "MonitoringWPF";
        private const string Password = "monitoring123";
}
}