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
                            view.Name = this.TaskParam.OperateFieldValue;
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
                            table.Name = this.TaskParam.OperateFieldValue;
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
                            column.FieldName = this.TaskParam.OperateFieldValue;
                            return true;
                        }
                        else if (TaskParam.OperateType == OperateType.Delete)
                        {
                            table.Columns.Remove(column);
                        }
                    }
                    break;
            }
            return false;
        }
    }
}
