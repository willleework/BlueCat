﻿using BlueCat.ConfigTools;
using BlueCat.Tools.FileTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueCat.Client
{
    public partial class BlueCat : Form
    {

        #region 变量
        string _dbip = string.Empty;
        string _dbuser = string.Empty;
        string _dbpass = string.Empty;
        string _dbpot = string.Empty;
        string _dbName = string.Empty;

        /// <summary>
        /// 数据刷新代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private delegate void messgaeEventDelegate(object sender, ConfigManageEventArgs args);

        /// <summary>
        /// 异步任务回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private delegate void CallBackDelegate();

        private LocalParam userParam = new LocalParam();

        /// <summary>
        /// 用户界面参数
        /// </summary>
        string configPath = Path.Combine(Environment.CurrentDirectory, "Resource", "Config", "LocalUserParam.xml");
        #endregion

        public BlueCat()
        {
            InitializeComponent();
            ConfigManage.MesageEvent += MessageEventHandler;
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MessageEventHandler(object sender, ConfigManageEventArgs args)
        {
            if (this.rtb_Monitor.InvokeRequired)
            {
                messgaeEventDelegate meDelegate = new messgaeEventDelegate(MessageEventHandler);
                this.Invoke(meDelegate, sender, args);
                return;
            }
            if (!string.IsNullOrWhiteSpace(args.Message))
            {
                rtb_Monitor.AppendText(string.Format("【{0}】 {1}", DateTime.Now.ToString("HH:MM:ss-fff"), args.Message));
                rtb_Monitor.AppendText(Environment.NewLine);
            }
            else
            {
                rtb_Monitor.AppendText(Environment.NewLine);
            }
            if (args.ProgressIndex >= 0)
            {
                toolbarStatus.Value = args.ProgressIndex;
            }
        }

        /// <summary>
        /// 下单完成事件
        /// </summary>
        private void CallbackFunc()
        {
            if (this.InvokeRequired)
            {
                CallBackDelegate callback = new CallBackDelegate(CallbackFunc);
                this.Invoke(callback);
                return;
            }
            btn_Modify.Enabled = true;
        }

        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            userParam = FileConvertor.XmlDeserializeObjectFromFile<LocalParam>(configPath);
            txt_dbip.Text = userParam.DbServerIP;
            txt_dbpot.Text = userParam.DbServerPot;
            txt_dbuser.Text = userParam.DbServerUser;
            txt_dbpass.Text = userParam.DbServerPss;
            txt_dbName.Text = userParam.DbName;
            txt_operate_no.Text = userParam.LastOperateNo;
            txt_sys_version_no.Text = userParam.LastSysVersionNo;
            txt_sys_ver_no_new.Text = userParam.NextSysVersionNo;
            txt_client_config_type.Text = userParam.LastClientConfigType;
            txt_cfg.Text = userParam.ConfigPath;

            if (string.IsNullOrEmpty(txt_operate_no.Text))
            {
                txt_operate_no.Text = "多个操作员用，分隔";
            }
        }

        /// <summary>
        /// 数据库连接信息设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DBSetting_Click(object sender, EventArgs e)
        {
            _dbip = txt_dbip.Text;
            _dbuser = txt_dbuser.Text;
            _dbpot = txt_dbpot.Text;
            _dbpass = txt_dbpass.Text;
            _dbName = txt_dbName.Text;
            txt_dbip.ReadOnly = true;
            txt_dbuser.ReadOnly = true;
            txt_dbpass.ReadOnly = true;
            txt_dbpot.ReadOnly = true;
            txt_dbName.ReadOnly = true;
            tabDataBase.SelectedTab = tabPage2;
        }

        /// <summary>
        /// 数据库连接信息重新设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DBReset_Click(object sender, EventArgs e)
        {
            txt_dbip.ReadOnly = false;
            txt_dbuser.ReadOnly = false;
            txt_dbpass.ReadOnly = false;
            txt_dbpot.ReadOnly = false;
            txt_dbName.ReadOnly = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cfgFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "c:\\";
            dialog.Filter = "Json文件(*.json)|*.JSON";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txt_cfg.Text = dialog.FileName; ;
            }
        }

        /// <summary>
        /// 修改配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Modify_Click(object sender, EventArgs e)
        {
            string dbConn = string.Format("server={0};user id={1};password={2};port={3};persistsecurityinfo=True;database={4}", _dbip, _dbuser, _dbpass, _dbpot, _dbName);
            string client_cfig_type = txt_client_config_type.Text;
            string sys_serversion_curr = txt_sys_version_no.Text;
            string sys_ver_no_next = txt_sys_ver_no_new.Text;
            string cfg = txt_cfg.Text;
            List<int> opts = GetOperateNos(txt_operate_no.Text.Trim());
            btn_Modify.Enabled = false;
            new Thread(() => 
            {
                try
                {
                    foreach (int opt in opts)
                    {
                        ConfigManage.ModifyServerConfig(dbConn, opt, client_cfig_type, sys_serversion_curr, sys_ver_no_next, cfg);
                    }
                }
                finally
                {
                    CallbackFunc();
                }
            }).Start();
        }

        /// <summary>
        /// 获取操作员编号
        /// </summary>
        /// <param name="operates"></param>
        /// <returns></returns>
        private List<int> GetOperateNos(string operates)
        {
            List<int> numOpts = new List<int> ();
            string[] opts = operates.Split(new char[]{ ',', '，'});
                int operateNo;
            foreach (string opt in opts)
            {
                if (!int.TryParse(opt, out operateNo))
                {
                    MessageBox.Show("获取操作员编号失败");
                    return new List<int>();
                }
                numOpts.Add(operateNo);
            }
            return numOpts;
        }

        /// <summary>
        /// 关闭菜单时，保存用户参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlueCat_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserDataToParam();
        }

        /// <summary>
        /// 保存用户参数
        /// </summary>
        private void SaveUserDataToParam()
        {
            userParam.DbServerIP = txt_dbip.Text;
            userParam.DbServerPot = txt_dbpot.Text;
            userParam.DbServerUser = txt_dbuser.Text;
            userParam.DbServerPss = txt_dbpass.Text;
            userParam.DbName = txt_dbName.Text;
            userParam.LastOperateNo = txt_operate_no.Text;
            userParam.LastSysVersionNo = txt_sys_version_no.Text;
            userParam.NextSysVersionNo = txt_sys_ver_no_new.Text;
            userParam.LastClientConfigType = txt_client_config_type.Text;
            userParam.ConfigPath = txt_cfg.Text;
            FileConvertor.ObjectSerializeXmlFile(userParam, configPath);
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            txt_operate_no.Text = string.Empty;
            txt_sys_version_no.Text = string.Empty;
            txt_sys_ver_no_new.Text = string.Empty;
            //txt_client_config_type.Text = string.Empty;
            txt_cfg.Text = string.Empty;
        }

        /// <summary>
        /// 清空运行信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            rtb_Monitor.Text = string.Empty;
        }

        /// <summary>
        /// 保存运行信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveLog_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Resource", "Log", string.Format("BlueCatLog.{0}.log", DateTime.Now.ToString("yyyyMMddHHMMssfff")));
            FileConvertor.WriteFile(path, rtb_Monitor.Text);
        }

        private void rtb_Monitor_TextChanged(object sender, EventArgs e)
        {
            rtb_Monitor.SelectionStart = rtb_Monitor.Text.Length; //Set the current caret position at the end
            rtb_Monitor.ScrollToCaret(); //Now scroll it automatically
        }
    }
}
