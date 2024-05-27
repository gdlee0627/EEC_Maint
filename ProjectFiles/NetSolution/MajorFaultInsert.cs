#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.SQLiteStore;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.CoreBase;
using FTOptix.System;
using FTOptix.Store;
using FTOptix.Report;
using FTOptix.OPCUAServer;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.NetLogic;
using FTOptix.Core;
using FTOptix.NativeUI;
#endregion

public class MajorFaultInsert : BaseNetLogic
{

        IUAVariable variable1 = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultBit");
        IUAVariable Type = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/storefault/Type");
        IUAVariable Code = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/storefault/Code");
        IUAVariable Year = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/Yr");
        IUAVariable Month = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/Mo");
        IUAVariable Day = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/Da");
        IUAVariable Hour = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/Hr");
        IUAVariable Minute = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/Min");
        IUAVariable Second = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/Sec");
        IUAVariable uSecond = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultTimeStamp_Last/uSec");

    public override void Start()
    {
        synchronizer = new RemoteVariableSynchronizer(TimeSpan.FromMilliseconds(2000));
        synchronizer.Add(variable1);
        synchronizer.Add(Type);
        synchronizer.Add(Code);
        synchronizer.Add(Year);
        synchronizer.Add(Month);
        synchronizer.Add(Day);
        synchronizer.Add(Hour);
        synchronizer.Add(Minute);
        synchronizer.Add(Second);
        synchronizer.Add(uSecond);
        
        variable1.VariableChange += Variable1_VariableChange;
        //Log.Info(variable1.Value.ToString());  
    }

    private void Variable1_VariableChange(object sender, VariableChangeEventArgs e)
    {      
            
        if ((bool)variable1.Value)
        {    
                   
            DateTime TimeStamp = new DateTime(Year.Value,Month.Value,Day.Value,Hour.Value,Minute.Value,Second.Value);

            var store = Project.Current.GetObject("DataStores"); ;
            string[] columnName = { "Type", "Code", "LastTimeStamping"};
            var values = new Object[1, 3];
            var internalDatabase = store.Children.Get<FTOptix.Store.Store>("EmbeddedDatabase1");
            var table = internalDatabase.Tables.Get<FTOptix.Store.Table>("FaultStoreTable");

         
            values[0, 0] = (int)Type.Value;
            Log.Info(Type.Value.ToString()); 
            values[0, 1] = (int)Code.Value;
            values[0, 2] = TimeStamp;//DateTime.Now;
            Log.Info(DateTime.Now.ToString()); 
            Log.Info(values[0, 1].ToString());
            table.Insert(columnName, values);

            variable1.SetValue(false);
                          
        }
        
    }

    public override void Stop()
    {
        synchronizer.Dispose();

        IUAVariable variable1 = Project.Current.GetVariable("CommDrivers/RAEtherNet_IPDriver1/RAEtherNet_IPStation1/Tags/Controller Tags/MajorFaultBit");
        variable1.VariableChange -= Variable1_VariableChange;
    }

    RemoteVariableSynchronizer synchronizer;
}
