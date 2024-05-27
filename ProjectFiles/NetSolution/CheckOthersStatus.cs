#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using HtmlAgilityPack;
using FTOptix.NativeUI;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.VisualBasic;
using FTOptix.DataLogger;
using FTOptix.OPCUAServer;
using FTOptix.Report;
using FTOptix.System;
using FTOptix.RAEtherNetIP;
using FTOptix.CommunicationDriver;
#endregion

public class CheckOthersStatus : BaseNetLogic
{
[ExportMethod]
    public void Check()
    {
       var myLongRunningTask = new LongRunningTask(checkLifeCycle, LogicObject);
        myLongRunningTask.Start();           
    }


    private void checkLifeCycle(LongRunningTask task)
    {

        var project = FTOptix.HMIProject.Project.Current;
        var myStore = project.GetObject("DataStores").Get<FTOptix.Store.Store>("EmbeddedDatabase1");

        object[,] resultSet;
        string[] header;
        object[,] resultSets;
        string[] headers;

        myStore.Query("SELECT * FROM Others", out header, out resultSet);

        if (resultSet.Rank != 2)
            return;

        var rowCount = resultSet != null ? resultSet.GetLength(0) : 0;
        var columnCount = header != null ? header.Length : 0;

        var doc = new HtmlDocument();

        if (rowCount > 0 && columnCount > 0)
        {
            for(int i = 0;i < rowCount;i++)
        {
            var catalogNumber = "https://www.rockwellautomation.com.cn/products/details." + resultSet[i, 1] + ".html";
        // doc = new HtmlWeb().Load("https://www.rockwellautomation.com.cn/products/details.1756-L85EP.html");
            doc = new HtmlWeb().Load(catalogNumber);
            var node =  doc.DocumentNode;
            // var result = node.SelectNodes("//div[contains(@class, 'status')]");
            var node1 = doc.GetElementbyId("lifecycle-details");
            var node2 = node1.SelectSingleNode(".//span[@class='value']");
            // Log.Info(resultSet[i, 0].ToString());
            // Log.Info(node2.InnerText.Trim());

            myStore.Query("UPDATE Others SET Status = " + "\""+ node2.InnerText.Trim()+ "\"" + " WHERE ID = " + resultSet[i, 0] , out headers, out resultSets);
        }

        }  
    }
    private LongRunningTask myLongRunningTask;
}
