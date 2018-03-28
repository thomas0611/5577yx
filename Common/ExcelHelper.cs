using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class ExcelHelper
    {
        /// <summary>
        /// 返回生成的Excel文件路径
        /// </summary>
        /// <returns></returns>
        //public static string ExportToExcel(string fileName)
        //{
        //    HSSFWorkbook book = new HSSFWorkbook();
        //    ISheet sheet = book.CreateSheet("Sheet1");
        //    IRow rowHeader = sheet.CreateRow(1);
        //    rowHeader.CreateCell(0, CellType.STRING).SetCellValue("");
        //    //string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //    //context.Server.MapPath("~/upload/" + fileName);
        //    using(Stream stream = File.OpenWrite(fileName))
        //    {
        //        book.Write(stream);
        //    }

        //}
    }
}
