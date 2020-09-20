using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;
using UnityEditor;

namespace Assets.Scripts.Excel
{
    public static class ExcelTest
    {
        [MenuItem("Tools/ExportExcel")]
        public static void ExportExcel()
        {
            string outPutDir =  "ExcelDir\\SaleData.xls";//Application.dataPath +
            FileInfo newFile = new FileInfo(outPutDir);
            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook  
                newFile = new FileInfo(outPutDir);
            }
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("车位售卖数据");
                worksheet.Cells[1, 1].Value = "序号";
                worksheet.Cells[1, 2].Value = "车位编号";
                worksheet.Cells[1, 3].Value = "长(mm)";
                worksheet.Cells[1, 4].Value = "宽(mm)";
                worksheet.Cells[1, 5].Value = "状态";
                int i = 1;
                //List<SationNode> sationNodeList = StaticMemory.SastionInfomation.OrderBy(p => p.No).ToList();
                //foreach (SationNode node in sationNodeList)
                //{
                //    i++;
                //    worksheet.Cells[i, 1].Value = i.ToString();
                //    worksheet.Cells[i, 2].Value = node.No;
                //    worksheet.Cells[i, 3].Value = node.SationLong;
                //    worksheet.Cells[i, 4].Value = node.SationWidth;
                //    string state = "未售";
                //    int saleFlag = PlayerPrefs.GetInt(node.Id);
                //    switch (saleFlag)
                //    {
                //        case 0: state = "未售"; break;
                //        case 1: state = "预售"; break;
                //        case 2: state = "已售"; break;
                //    }
                //    worksheet.Cells[i, 5].Value = state;
                //}
                package.Save();
            }
        }
    }
}