#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.SQLiteStore;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.CoreBase;
using FTOptix.Store;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.NetLogic;
using FTOptix.Core;
using FTOptix.DataLogger;
using FTOptix.OPCUAServer;
using FTOptix.Report;
using FTOptix.System;
#endregion

public class InsertMtnInfo : BaseNetLogic
{

[ExportMethod]
public void CheckIn(){

    var MtnClassInput = Owner.Owner.Children.Get<TextBox>("MtnClassInput");
    var MtnObjectInput = Owner.Owner.Children.Get<TextBox>("MtnObjectInput");
    var MtnContentInput = Owner.Owner.Children.Get<TextBox>("MtnContentInput");
    var MtnProcedureInput = Owner.Owner.Children.Get<TextBox>("MtnProcedureInput");
    var StartTime = Owner.Owner.Children.Get<DateTimePicker>("StartTime");
    var StopTime = Owner.Owner.Children.Get<DateTimePicker>("StopTime");
    var MtnOwnerInput = Owner.Owner.Children.Get<TextBox>("MtnOwnerInput");

    // StartTime.Value = DateTime.Now;
    var store = Project.Current.GetObject("DataStores"); ;
    string[] columnName = { "Class", "Object", "Descriptions", "Procedures", "StartTIme","StopTime","Owner" };
    var values = new Object[1,7];
    var internalDatabase = store.Children.Get<FTOptix.Store.Store>("EmbeddedDatabase1");
    var table = internalDatabase.Tables.Get<FTOptix.Store.Table>("MtnTable");
    values[0,0] = MtnClassInput.Text;
    values[0,1] = MtnObjectInput.Text;
    values[0,2] = MtnContentInput.Text;
    values[0,3] = MtnProcedureInput.Text;
    values[0,4] = StartTime.Value;
    values[0,5] = StopTime.Value;
    values[0,6] = MtnOwnerInput.Text;
    Log.Info(values[0,0].ToString());

    table.Insert(columnName, values);
}
}
