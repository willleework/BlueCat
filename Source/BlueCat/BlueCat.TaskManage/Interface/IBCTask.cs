using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.TaskManage
{
    public interface IBCTask<T>
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        Guid TaskID { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        string TaskDescription {get;set;}

        /// <summary>
        /// 任务优先级
        /// </summary>
        Priority TaskPriority { get; set; }

        /// <summary>
        /// 任务参数
        /// </summary>
        T TaskParam { get; set; }

        /// <summary>
        /// 任务处理
        /// </summary>
        /// <returns></returns>
        bool TaskHandle();
    }
}
