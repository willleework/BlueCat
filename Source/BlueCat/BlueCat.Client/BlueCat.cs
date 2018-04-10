using BlueCat.ConfigTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        #endregion

        public BlueCat()
        {
            InitializeComponent();
            ConfigManage.MesageEvent += MessageEventHandler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MessageEventHandler(object sender, ConfigManageEventArgs args)
        {
            rtb_Monitor.AppendText(string.Format("【{0}】 {1}", DateTime.Now.ToString("HH:MM:ss-fff"), args.Message));
            rtb_Monitor.AppendText("\r\n");
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
            string dbConn = string.Format("server={0};user id={1};password={2};persistsecurityinfo=True;database={3}", _dbip, _dbuser, _dbpass, _dbName);
            int operateNo;
            if (!int.TryParse(txt_operate_no.Text, out operateNo))
            {
                MessageBox.Show("获取操作员编号失败");
            }
            ConfigManage.ModifyServerConfig(dbConn, operateNo, txt_client_config_type.Text, txt_sys_version_no.Text, txt_cfg.Text);
        }
    }
}
