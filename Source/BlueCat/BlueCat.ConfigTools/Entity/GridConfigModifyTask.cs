using BlueCat.TaskManage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    /// <summary>
    /// 表格修改任务
    /// </summary>
    public class GridConfigModifyTask : IBCTask<GridConfigModifyTaskParam>
    {
        #region 属性
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskID { get; set; }
        /// <summary>
        /// 任务描述
        /// </summary>
        [DefaultValue("表格配置文件修改")]
        public string TaskDescription { get; set; }
        /// <summary>
        /// 任务优先级
        /// </summary>
        public Priority TaskPriority { get; set; }
        /// <summary>
        /// 任务参数
        /// </summary>
        public GridConfigModifyTaskParam TaskParam { get; set; }
        /// <summary>
        /// 任务执行状态
        /// </summary>
        [DefaultValue(TaskResult.待执行)]
        public TaskResult TaskResult { get; set; }
        #endregion

        /// <summary>
        /// 任务处理函数：修改xml各分支节点Name字段
        /// </summary>
        /// <returns></returns>
        public bool TaskHandle()
        {
            TaskResult = TaskResult.执行中;
            if (TaskParam == null || TaskParam.GridConfigInfo == null)
            {
                TaskResult = TaskResult.失败;
                return false;
            }

            switch (TaskParam.KeyField)
            {
                case "View":
                    {
                        GridLayoutViewInfo view = TaskParam.GridConfigInfo.Views.Where(p => p.Name.Equals(TaskParam.View)).First();
                        if (view == null)
                        {
                            TaskResult = TaskResult.失败;
                            return false;
                        }
                        if (TaskParam.OperateType == OperateType.Modify)
                        {
                            foreach (Operate ope in TaskParam.Operates)
                            {
                                //view.Name = TaskParam.OperateFieldValue;
                                SetPropertyValue(ope.OperateField, ope.OperateFieldValue, view);
                            }
                            TaskResult = TaskResult.完成;
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
                            TaskResult = TaskResult.失败;
                            return false;
                        }
                        GridLayoutTableInfo table = view.Tables.Where(p => p.Name.Equals(TaskParam.Table)).First();
                        if (table == null)
                        {
                            TaskResult = TaskResult.失败;
                            return false;
                        }
                        if (TaskParam.OperateType == OperateType.Modify)
                        {
                            foreach (Operate ope in TaskParam.Operates)
                            {
                                //table.Name = TaskParam.OperateFieldValue;
                                SetPropertyValue(ope.OperateField, ope.OperateFieldValue, table);
                            }
                            TaskResult = TaskResult.完成;
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
                            TaskResult = TaskResult.失败;
                            return false;
                        }
                        GridLayoutTableInfo table = view.Tables.Where(p => p.Name.Equals(TaskParam.Table)).First();
                        if (table == null)
                        {
                            TaskResult = TaskResult.失败;
                            return false;
                        }
                        GridLayoutColumnInfo column = table.Columns.Where(p => p.FieldName.Equals(TaskParam.Column)).First();
                        if (column == null)
                        {
                            TaskResult = TaskResult.失败;
                            return false;
                        }
                        if (TaskParam.OperateType == OperateType.Modify)
                        {
                            foreach (Operate ope in TaskParam.Operates)
                            {
                                //column.FieldName = TaskParam.OperateFieldValue;
                                SetPropertyValue(ope.OperateField, ope.OperateFieldValue, column);
                            }
                            TaskResult = TaskResult.完成;
                            return true;
                        }
                        else if (TaskParam.OperateType == OperateType.Delete)
                        {
                            table.Columns.Remove(column);
                        }
                    }
                    break;
            }
            TaskResult = TaskResult.完成;
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
