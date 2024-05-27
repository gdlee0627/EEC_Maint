#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.SQLiteStore;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.UI;
using FTOptix.Store;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CommunicationDriver;
using FTOptix.CoreBase;
using FTOptix.Core;
using FTOptix.NetLogic;
using FTOptix.WebUI;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using FTOptix.DataLogger;
using FTOptix.OPCUAServer;
using FTOptix.Report;
using FTOptix.System;
#endregion

public class InsertOthers : BaseNetLogic
{
[ExportMethod]
    public void InsertEntries()
    {
        try
        {
            string ExcelPathVariable = LogicObject.GetVariable("Others").Value;
            var ExcelPath = new ResourceUri(ExcelPathVariable).Uri;
            // IWorkbook workbook;
            FileStream fs = new FileStream(ExcelPath, FileMode.Open, FileAccess.Read);
            ISheet sheet = new XSSFWorkbook(fs).GetSheetAt(0);
            if (sheet != null)
            {
                int rowCount = 12;//sheet.LastRowNum; // This may not be valid row count.
                                                 // Log.Info(rowCount.ToString());
                Log.Info(rowCount.ToString());

                var store = Project.Current.GetObject("DataStores"); ;
                string[] columnName = { "ID", "Catalog", "Descriptions","Status" };
                var values = new Object[rowCount, 4];
                var internalDatabase = store.Children.Get<FTOptix.Store.Store>("EmbeddedDatabase1");
                var table = internalDatabase.Tables.Get<FTOptix.Store.Table>("Others");
                // If first row is table head, i starts from 1
                for (int i = 0; i < rowCount; i++)
                {
                
                    IRow curRow = sheet.GetRow(i);
                    // Works for consecutive data. Use continue otherwise 
                    if (curRow == null)
                    {
                        // Valid row count
                        rowCount = i - 1;
                        break;
                    }
                    // Get data from all columns
                    var ID = curRow.GetCell(0).NumericCellValue;
                    var Catalog = curRow.GetCell(1).StringCellValue.Trim();
                    var Descriptions = curRow.GetCell(2).StringCellValue.Trim();
                    var status = curRow.GetCell(3).StringCellValue.Trim();

                    values[i, 0] = ID;
                    values[i, 1] = Catalog;
                    values[i, 2] = Descriptions;
                    values[i, 3] = status;
                }
                table.Insert(columnName, values);
            }
        }
        catch (Exception e)
        {
            Log.Info(e.Message);
        }

    }
}
