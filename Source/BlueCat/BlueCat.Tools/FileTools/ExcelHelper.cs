using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.Tools.FileTools
{
    public class ExcelHelper
    {

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文件流</param>
        /// <param name="headerRowIndex">Excel表头行索引，0表示自动识别</param>
        /// <param name="columnCount">Excel列数，0表示自动识别</param>
        /// <returns>DataSet</returns>
        private static DataSet ImportDataSetFromExcel(Stream excelFileStream, int headerRowIndex = 0, int columnCount = 0)
        {
            DataSet ds = new DataSet();
            IWorkbook workbook = null;
            //报存传入的首行下标
            int headerRowIndexParam = headerRowIndex;
            try
            {
                workbook = WorkbookFactory.Create(excelFileStream);
                for (int a = 0, b = workbook.NumberOfSheets; a < b; a++)
                {
                    ISheet sheet = workbook.GetSheetAt(a);
                    System.Data.DataTable table = new System.Data.DataTable(sheet.SheetName);

                    //将首行位置重新设回传入值，避免多sheet页时起始行被修改
                    headerRowIndex = headerRowIndexParam;
                    headerRowIndex = headerRowIndex > 0 ? headerRowIndex : sheet.FirstRowNum;
                    IRow headerRow = sheet.GetRow(headerRowIndex);
                    if (headerRow == null)
                        continue;
                    int cellCount = columnCount > 0 ? columnCount : headerRow.LastCellNum;
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        if (headerRow.GetCell(i) == null)
                        {
                            // 如果遇到第一个空列，则不再继续向后读取
                            cellCount = i + 1;
                            break;
                        }
                        DataColumn column = new DataColumn(headerRow.GetCell(i).ReadString(workbook));
                        table.Columns.Add(column);
                    }
                    string rowStrs = string.Empty;
                    for (int i = (headerRowIndex + 1); i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.GetCell(0) == null || (columnCount <= 0 && row.GetCell(0).ReadString(workbook).Trim() == string.Empty))
                        {
                            // 如果遇到第一个空行，则不再继续向后读取
                            break;
                        }

                        rowStrs = string.Empty;
                        DataRow dataRow = table.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                            {
                                dataRow[j] = row.GetCell(j).ReadString(workbook);
                                rowStrs += dataRow[j];
                            }
                        }
                        if (string.IsNullOrWhiteSpace(rowStrs))
                            break;
                        table.Rows.Add(dataRow);
                    }
                    ds.Tables.Add(table);
                }
                excelFileStream.Close();
                workbook = null;
                return ds;
            }
            finally
            {
                try
                {
                    excelFileStream.Close();
                    excelFileStream.Dispose();
                }
                catch { }
            }
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引，0表示自动识别</param>
        /// <param name="columnCount">Excel列数，0表示自动识别</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel(string excelFilePath, int headerRowIndex = 0, int columnCount = 0)
        {
            if (!IsExcel(excelFilePath))
                throw new Exception("文件无效！");
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataSetFromExcel(stream, headerRowIndex, columnCount);
            }
        }

        /// <summary>
        /// 由Excel导入DataSet，如果有多个工作表，则导入多个DataTable
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引，0表示自动识别</param>
        /// <returns>DataSet</returns>
        public static DataSet ImportDataSetFromExcel(string excelFilePath, int headerRowIndex = 0)
        {
            if (!IsExcel(excelFilePath))
                throw new Exception("文件无效！");
            using (FileStream stream = System.IO.File.OpenRead(excelFilePath))
            {
                return ImportDataSetFromExcel(stream, headerRowIndex, 0);
            }
        }

        /// <summary>
        /// 由Excel直接读取实体类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="excelFilePath">Excel文件路径，为物理路径。</param>
        /// <param name="headerRowIndex">Excel表头行索引，0表示自动识别</param>
        /// <param name="columnCount">Excel列数，0表示自动识别</param>
        /// <param name="tableName">Excel页签名称, 空表示第一个</param>
        /// <returns></returns>
        public static List<T> ImputFromExcel<T>(string excelFilePath, int headerRowIndex = 0, int columnCount = 0, string tableName = "") where T : new()
        {
            DataSet ds = ImportDataSetFromExcel(excelFilePath, headerRowIndex, columnCount);
            if (ds.Tables.Count < 1)
            {
                throw new Exception("由Excel直接读取实体类对象失败，Excel应至少有一个sheet页");
            }
            DataTable dataTable;
            if (ds.Tables.Contains(tableName))
            {
                dataTable = ds.Tables[tableName];
            }
            else
            {
                dataTable = ds.Tables[0];
            }
            return ConvertFromDataTable<T>(dataTable);
        }

        /// <summary>
        /// 由DataTable直接读取实体类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public static List<T> ConvertFromDataTable<T>(DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                list.Add(ConvertFromDataRow<T>(row));
            }

            return list;
        }

        /// <summary>
        /// 由DataRow转换为指定类型的对象
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="row">行</param>
        /// <returns></returns>
        public static T ConvertFromDataRow<T>(DataRow row) where T : new()
        {
            T entity = new T();

            Dictionary<string, PropertyInfo> pros = typeof(T).GetProperties().ToDictionary(p => { return p.Name; });
            Dictionary<string, FieldInfo> fileds = typeof(T).GetFields().ToDictionary(p => { return p.Name; });

            foreach (DataColumn dc in row.Table.Columns)
            {
                if (row.IsNull(dc))
                    continue;

                if (pros.ContainsKey(dc.ColumnName))
                {
                    if (pros[dc.ColumnName].CanWrite)
                    {
                        pros[dc.ColumnName].SetValue(entity,
                            TypeDescriptor.GetConverter(pros[dc.ColumnName].PropertyType).ConvertFromInvariantString(row[dc].ToString()),
                            null);
                    }
                }
                if (fileds.ContainsKey(dc.ColumnName))
                {
                    fileds[dc.ColumnName].SetValue(entity, TypeDescriptor.GetConverter(pros[dc.ColumnName].PropertyType).ConvertFromInvariantString(row[dc].ToString()));
                }
            }

            return entity;
        }

        /// <summary>
        /// 将对象列表转换为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList">要转换的对象列表</param>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列名</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(List<T> entityList, string tableName, List<string> columns)
        {
            if (tableName == null)
                tableName = "";

            DataTable table = new DataTable(tableName);
            Dictionary<string, PropertyInfo> pros = typeof(T).GetProperties().ToDictionary(p => { return p.Name; });
            Dictionary<string, FieldInfo> fields = typeof(T).GetFields().ToDictionary(p => { return p.Name; });
            if (columns == null || columns.Count < 1)
            {
                columns = new List<string>(pros.Keys);
                columns.AddRange(fields.Keys);
                columns.Sort();
            }

            foreach (string col in columns)
            {
                if (pros.ContainsKey(col))
                {
                    table.Columns.Add(col, pros[col].PropertyType);
                }
                else if (fields.ContainsKey(col))
                {
                    table.Columns.Add(col, fields[col].FieldType);
                }
                else
                {
                    table.Columns.Add(col, typeof(string));
                }
            }

            DataRow row = null;
            table.BeginLoadData();
            foreach (T entity in entityList)
            {
                row = table.NewRow();
                row.BeginEdit();
                foreach (DataColumn dc in table.Columns)
                {
                    //if (row.IsNull(dc))
                    //    continue;

                    if (pros.ContainsKey(dc.ColumnName))
                    {
                        if (pros[dc.ColumnName].CanRead)
                        {
                            row[dc] = pros[dc.ColumnName].GetValue(entity, null);
                        }
                    }
                    else if (fields.ContainsKey(dc.ColumnName))
                    {
                        row[dc] = fields[dc.ColumnName].GetValue(entity);
                    }
                }
                row.EndEdit();
                table.Rows.Add(row);
            }
            table.EndLoadData();

            return table;
        }

        /// <summary>
        /// 将对象列表转换为DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList">要转换的对象列表</param>
        /// <param name="tableName">表名</param>
        /// <param name="columns">列名与标题字典，在保存的Sheet页中第一行是标题，第二行是列名，第三行开始是数据</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable<T>(List<T> entityList, string tableName, Dictionary<string, string> columnCaptions)
        {
            if (tableName == null)
                tableName = "";

            DataTable table = new DataTable(tableName);
            Dictionary<string, PropertyInfo> pros = typeof(T).GetProperties().ToDictionary(p => { return p.Name; });
            Dictionary<string, FieldInfo> fields = typeof(T).GetFields().ToDictionary(p => { return p.Name; });
            List<string> columns = new List<string>();
            if (columnCaptions == null || columnCaptions.Count < 1)
            {
                columns.AddRange(pros.Keys);
                columns.AddRange(fields.Keys);
                columns.Sort();
            }
            else
            {
                columns.AddRange(columnCaptions.Keys);
            }

            DataColumn dataColumn = null;
            foreach (string col in columns)
            {
                if (pros.ContainsKey(col))
                {
                    dataColumn = table.Columns.Add(col, pros[col].PropertyType);
                }
                else if (fields.ContainsKey(col))
                {
                    dataColumn = table.Columns.Add(col, fields[col].FieldType);
                }
                else
                {
                    dataColumn = table.Columns.Add(col, typeof(string));
                }
                dataColumn.Caption = columnCaptions.ContainsKey(col) ? columnCaptions[col] : col;
            }

            table.BeginLoadData();
            DataRow row = null;
            foreach (T entity in entityList)
            {
                row = table.NewRow();
                row.BeginEdit();
                foreach (string dc in columns)
                {
                    if (pros.ContainsKey(dc))
                    {
                        if (pros[dc].CanRead)
                        {
                            row[dc] = pros[dc].GetValue(entity, null);
                        }
                    }
                    else if (fields.ContainsKey(dc))
                    {
                        row[dc] = fields[dc].GetValue(entity);
                    }
                }
                row.EndEdit();
                table.Rows.Add(row);
            }
            table.EndLoadData();

            return table;
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="excelFilePath"></param>
        /// <param name="columns">列名</param>
        /// <param name="tableName">Sheet名</param>
        public static void ExportToExcel<T>(List<T> entityList, string excelFilePath, List<string> columns, string tableName = "")
        {
            DataTable table = ConvertToDataTable(entityList, tableName, columns);
            ExportToExcel(table, excelFilePath);
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityList"></param>
        /// <param name="excelFilePath"></param>
        /// <param name="columns">列名与标题字典，在保存的Sheet页中第一行是标题，第二行是列名，第三行开始是数据</param>
        /// <param name="tableName">Sheet名</param>
        public static void ExportToExcel<T>(List<T> entityList, string excelFilePath, Dictionary<string, string> columns, string tableName = "")
        {
            DataTable table = ConvertToDataTable(entityList, tableName, columns);
            ExportToExcel(table, excelFilePath);
        }

        private static CellType GetCellType(Type type)
        {
            if (type == typeof(bool))
            {
                return CellType.Boolean;
            }
            else if (type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(decimal)
                 || type == typeof(short) || type == typeof(byte) || type == typeof(long))
            {
                return CellType.Numeric;
            }
            else
            {
                return CellType.String;
            }
        }

        /// <summary>
        /// 导出到Excel，Excel文件必须存在
        /// </summary>
        /// <param name="table"></param>
        /// <param name="excelFilePath"></param>
        public static void ExportToExcel(DataTable table, string excelFilePath)
        {
            IWorkbook workbook = null;

            try
            {
                using (FileStream stream = File.Open(excelFilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    string sheetName = string.IsNullOrEmpty(table.TableName) ? "Sheet1" : table.TableName;
                    FileInfo fi = new FileInfo(excelFilePath);
                    if (fi.Extension == ".xlsx") // 2007版本
                        workbook = new XSSFWorkbook();
                    else if (fi.Extension == ".xls") // 2003版本
                        workbook = new HSSFWorkbook();
                    else
                        workbook = new XSSFWorkbook();

                    ISheet sheet = workbook.GetSheet(sheetName);
                    if (sheet == null)
                    {
                        sheet = workbook.CreateSheet(sheetName);
                    }
                    else
                    {
                        workbook.RemoveSheetAt(workbook.GetSheetIndex(sheet));
                        sheet = workbook.CreateSheet(sheetName);
                    }

                    int rowIndex = 0;
                    int colCount = 0;
                    IRow row = null;
                    ICell cell = null;

                    DataColumn[] dcs = new DataColumn[table.Columns.Count];
                    table.Columns.CopyTo(dcs, 0);
                    if (dcs.Any(p => { return !string.IsNullOrEmpty(p.Caption); }))
                    {
                        row = sheet.CreateRow(rowIndex);
                        colCount = 0;
                        foreach (DataColumn dc in table.Columns)
                        {
                            cell = row.CreateCell(colCount, CellType.String);
                            cell.SetCellValue(dc.Caption);
                            colCount++;
                        }
                        rowIndex++;
                    }

                    row = sheet.CreateRow(rowIndex);
                    colCount = 0;
                    cell = null;
                    foreach (DataColumn dc in table.Columns)
                    {
                        cell = row.CreateCell(colCount, CellType.String);
                        cell.SetCellValue(dc.ColumnName);
                        colCount++;
                    }
                    rowIndex++;

                    CellType ctype = CellType.Unknown;
                    foreach (DataRow dr in table.Rows)
                    {
                        row = sheet.CreateRow(rowIndex);
                        colCount = 0;
                        foreach (DataColumn dc in table.Columns)
                        {
                            ctype = GetCellType(dc.DataType);
                            cell = row.CreateCell(colCount, ctype);
                            switch (ctype)
                            {
                                case CellType.Boolean:
                                    cell.SetCellValue((string)(dr[dc] == DBNull.Value ? "false" : dr[dc].ToString()));
                                    break;
                                case CellType.Numeric:
                                    cell.SetCellValue((string)(dr[dc] == DBNull.Value ? "" : dr[dc].ToString()));
                                    break;
                                case CellType.String:
                                    cell.SetCellValue((string)(dr[dc] == DBNull.Value ? "" : dr[dc].ToString()));
                                    break;
                            }
                            colCount++;
                        }
                        rowIndex++;
                    }

                    workbook.Write(stream);
                    workbook = null;
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 由对象转换为DataRow
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="entity">要转换的对象</param>
        /// <param name="row">行</param>
        /// <returns></returns>
        public static void ConvertFromDataRow<T>(T entity, ref DataRow row)
        {
            Dictionary<string, PropertyInfo> pros = typeof(T).GetProperties().ToDictionary(p => { return p.Name; });
            Dictionary<string, FieldInfo> fields = typeof(T).GetFields().ToDictionary(p => { return p.Name; });

            row.BeginEdit();
            foreach (DataColumn dc in row.Table.Columns)
            {
                if (row.IsNull(dc))
                    continue;
                if (pros.ContainsKey(dc.ColumnName))
                {
                    if (pros[dc.ColumnName].CanRead)
                    {
                        row[dc] = pros[dc.ColumnName].GetValue(entity, null);
                    }
                }
                else if (fields.ContainsKey(dc.ColumnName))
                {
                    row[dc] = fields[dc.ColumnName].GetValue(entity);
                }
            }
            row.EndEdit();
        }

        /// <summary>
        /// 由对象转换为DataRow
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="entity">要转换的对象</param>
        /// <param name="row">行</param>
        /// <returns></returns>
        public static List<string> GetColumns<T>()
        {
            Type type = typeof(T);
            PropertyInfo[] pinfos = type.GetProperties();
            FieldInfo[] finfos = type.GetFields();
            List<string> columns = new List<string>();
            if (pinfos.Length > 0)
                columns.AddRange(pinfos.Where(p => p.CanRead).Select(p => p.Name));
            if (finfos.Length > 0)
                columns.AddRange(finfos.Select(p => p.Name));

            return columns;
        }

        /// <summary>
        /// 验证Excel文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static bool IsExcel(string fileName)
        {
            try
            {
                FileInfo excelFile = new FileInfo(fileName);
                if (!excelFile.Exists)
                    return false;
                if (excelFile.Extension.ToLower() != ".xls" && excelFile.Extension.ToLower() != ".xlsx")
                    return false;
                return true;
            }
            catch (Exception ex){ return false; }
        }
    }

    /// <summary>
    /// excel单元格读取扩展类
    /// </summary>
    public static class CellToString
    {
        /// <summary>
        /// Excel单元格读取扩展方法
        /// </summary>
        /// <param name="cell">Excel单元格</param>
        /// <param name="workbook">文件对象，单元格格式是公式时使用</param>
        /// <returns></returns>
        public static string ReadString(this ICell cell, IWorkbook workbook)
        {
            if (cell == null) { return string.Empty; }
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue.ToString();
                    }
                    else
                    {
                        return cell.NumericCellValue.ToString();
                    }
                case CellType.Formula:
                    if (workbook is HSSFWorkbook)
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(workbook);
                        string str = e.Evaluate(cell).StringValue;
                        if (str != null)
                        {
                            return str;
                        }
                        return e.Evaluate(cell).NumberValue.ToString();
                    }
                    else
                    {
                        XSSFFormulaEvaluator e = new XSSFFormulaEvaluator(workbook);
                        string str = e.Evaluate(cell).StringValue;
                        if (str != null)
                        {
                            return str;
                        }
                        return e.Evaluate(cell).NumberValue.ToString();
                    }
                default:
                    return cell.ToString();
            }
        }
    }
}
