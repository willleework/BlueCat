using BlueCat.TaskManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 
    /// </summary>
    public class GridConfigModifyTask : IBCTask<GridConfigModifyTaskParam>
    {
        public Guid TaskID { get; set; }
        public string TaskDescription { get; set; }
        public Priority TaskPriority { get; set; }
        public GridConfigModifyTaskParam TaskParam { get; set; }

        /// <summary>
        /// 任务处理函数：修改xml各分支节点Name字段
        /// </summary>
        /// <returns></returns>
        public bool TaskHandle()
        {
            if (TaskParam == null || TaskParam.GridConfigInfo == null)
            {
                return false;
            }

            switch (TaskParam.KeyField)
            {
                case "View":
                    {
                        GridLayoutViewInfo view = TaskParam.GridConfigInfo.Views.Where(p => p.Name.Equals(TaskParam.View)).First();
                        if (view == null)
                        {
                            return false;
                        }
                        if (TaskParam.OperateType == OperateType.Modify)
                        {
                            foreach (Operate ope in TaskParam.Operates)
                            {
                                //view.Name = TaskParam.OperateFieldValue;
                                SetPropertyValue(ope.OperateField, ope.OperateFieldValue, view);
                            }
                            return true;
                        }
                        else if (TaskParam.OperateType == OperateType.Delete)
                        {
                            TaskParam.GridConfigInfo.Views.Remove(view);
                        }
                    }
                    break;
                case "Table":
                    {
                        GridLayoutViewInfo view = TaskParam.GridConfigInfo.Views.Where(p => p.Name.Equals(TaskParam.View)).First();
                        if (view == null)
                        {
                            return false;
                        }
                        GridLayoutTableInfo table = view.Tables.Where(p => p.Name.Equals(TaskParam.Table)).First();
                        if (table == null)
                        {
                            return false;
                        }
                        if (TaskParam.OperateType == OperateType.Modify)
                        {
                            foreach (Operate ope in TaskParam.Operates)
                            {
                                //table.Name = TaskParam.OperateFieldValue;
                                SetPropertyValue(ope.OperateField, ope.OperateFieldValue, table);
                            }
                            return true;
                        }
                        else if (TaskParam.OperateType == OperateType.Delete)
                        {
                            view.Tables.Remove(table);
                        }
                    }
                    break;
                case "Column":
                    {
                        GridLayoutViewInfo view = TaskParam.GridConfigInfo.Views.Where(p => p.Name.Equals(TaskParam.View)).First();
                        if (view == null)
                        {
                            return false;
                        }
                        GridLayoutTableInfo table = view.Tables.Where(p => p.Name.Equals(TaskParam.Table)).First();
                        if (table == null)
                        {
                            return false;
                        }
                        GridLayoutColumnInfo column = table.Columns.Where(p => p.FieldName.Equals(TaskParam.Column)).First();
                        if (column == null)
                        {
                            return false;
                        }
                        if (TaskParam.OperateType == OperateType.Modify)
                        {
                            foreach (Operate ope in TaskParam.Operates)
                            {
                                //column.FieldName = TaskParam.OperateFieldValue;
                                SetPropertyValue(ope.OperateField, ope.OperateFieldValue, column);
                            }
                            return true;
                        }
                        else if (TaskParam.OperateType == OperateType.Delete)
                        {
                            table.Columns.Remove(column);
                        }
                    }
                    break;
            }
            return true;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool SetPropertyValue(string fieldName, string value, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object v = Convert.ChangeType(value, Ts.GetProperty(fieldName).PropertyType);
                Ts.GetProperty(fieldName).SetValue(obj, v, null);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
