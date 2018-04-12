using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.TaskManage
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum Priority
    {
        FirstLevel,
        SecondLevel,
        ThirdLevel
    }

    /// <summary>
    /// 任务执行状态
    /// </summary>
    public enum TaskResult
    {
        创建,
        待执行,
        执行中,
        暂停,
        等待,
        撤销,
        完成,
        失败,
        异常
    }
}
