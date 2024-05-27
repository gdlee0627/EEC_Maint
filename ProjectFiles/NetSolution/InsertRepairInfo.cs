#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.CoreBase;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.Core;
using FTOptix.DataLogger;
using FTOptix.OPCUAServer;
using FTOptix.Report;
using FTOptix.System;
#endregion

public class InsertRepairInfo : BaseNetLogic
{
[ExportMethod]
public void CheckIn(){

    var RepairClassInput = Owner.Owner.Children.Get<TextBox>("RepairClassInput");
    var RepairItemInput = Owner.Owner.Children.Get<TextBox>("RepairItemInput");
    var FaultRespInput = Owner.Owner.Children.Get<TextBox>("FaultRespInput");
    var RepairMethodInput = Owner.Owner.Children.Get<TextBox>("RepairMethodInput");
    var RepairDoneInput = Owner.Owner.Children.Get<TextBox>("RepairDoneInput");
    var StartTime = Owner.Owner.Children.Get<DateTimePicker>("StartTime");
    var StopTime = Owner.Owner.Children.Get<DateTimePicker>("StopTime");
    var RepairOwnerInput = Owner.Owner.Children.Get<TextBox>("RepairOwnerInput");

    // StartTime.Value = DateTime.Now;
    var store = Project.Current.GetObject("DataStores"); ;
    string[] columnName = { "Class", "Object", "Descriptions", "Methods", "Done", "StartTIme","StopTime","Owner" };
    var values = new Object[1,8];
    var internalDatabase = store.Children.Get<FTOptix.Store.Store>("EmbeddedDatabase1");
    var table = internalDatabase.Tables.Get<FTOptix.Store.Table>("RepairTable");
    values[0,0] = RepairClassInput.Text;
    values[0,1] = RepairItemInput.Text;
    values[0,2] = FaultRespInput.Text;
    values[0,3] = RepairMethodInput.Text;
    values[0,4] = RepairDoneInput.Text;
    values[0,5] = StartTime.Value;
    values[0,6] = StopTime.Value;
    values[0,7] = RepairOwnerInput.Text;
    Log.Info(values[0,0].ToString());

    table.Insert(columnName, values);
}

}
