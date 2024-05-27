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

public class SafetyFaultsQuery : BaseNetLogic
{
[ExportMethod]
    public void RunQuery()
    {
        var Displaylabel = Owner.Owner.Children.Get<Label>("DisplayLabel");
        var Typelabel = Owner.Owner.Children.Get<Label>("TypeLabel");   
        var Reasonlabel = Owner.Owner.Children.Get<Label>("ReasonLabel"); 
        var Correctionlabel = Owner.Owner.Children.Get<Label>("CorrectionLabel"); 
       


        var project = FTOptix.HMIProject.Project.Current;
        var myStore = project.GetObject("DataStores").Get<FTOptix.Store.Store>("EmbeddedDatabase1");
        object[,] resultSet;
        string[] header;        
        //
        string FaultTypeInput = Owner.Owner.Get<TextBox>("FaultTypeInput").Text;
        string FaultCodeInput = Owner.Owner.Get<TextBox>("FaultCodeInput").Text;
        
        string queryState = String.Format("SELECT * FROM SafetyFaults WHERE FaultType={0} AND FaultCode={1} ORDER BY FaultType",FaultTypeInput,FaultCodeInput);
        myStore.Query(queryState, out header, out resultSet);

        if (resultSet.Rank != 2)
            return;

        var rowCount = resultSet != null ? resultSet.GetLength(0) : 0;
        var columnCount = header != null ? header.Length : 0;
        Log.Info(rowCount.ToString());
        Log.Info(resultSet[0,2].ToString());
        Log.Info(resultSet[0,3].ToString());
        Log.Info(resultSet[0,4].ToString());
        Log.Info(resultSet[0,5].ToString());
        // var sb = new StringBuilder("Record Count: ");
        // sb.Append(resultSet[0,4].ToString());

        Displaylabel.Text = resultSet[0,2].ToString();
        Typelabel.Text = resultSet[0,5].ToString();
        Reasonlabel.Text = resultSet[0,3].ToString();
        Correctionlabel.Text = resultSet[0,4].ToString();
        // var queryResultLabel = Owner.Get<Label>("Label1");
        // // queryResultLabel.Text = sb.ToString();
        // queryResultLabel.Text = sb.ToString();
       

    }
}
