﻿namespace BlueCat.Client
{
    partial class BlueCat
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.inputGroup = new System.Windows.Forms.GroupBox();
            this.tabDataBase = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_DBSetting = new System.Windows.Forms.Button();
            this.txt_dbpass = new System.Windows.Forms.TextBox();
            this.txt_dbuser = new System.Windows.Forms.TextBox();
            this.txt_dbpot = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_dbip = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_Modify = new System.Windows.Forms.Button();
            this.btn_cfgFile = new System.Windows.Forms.Button();
            this.txt_cfg = new System.Windows.Forms.TextBox();
            this.txt_sys_version_no = new System.Windows.Forms.TextBox();
            this.txt_client_config_type = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_operate_no = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_DBReset = new System.Windows.Forms.Button();
            this.txt_dbName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.rtb_Monitor = new System.Windows.Forms.RichTextBox();
            this.inputGroup.SuspendLayout();
            this.tabDataBase.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // inputGroup
            // 
            this.inputGroup.Controls.Add(this.tabDataBase);
            this.inputGroup.Location = new System.Drawing.Point(12, 12);
            this.inputGroup.Name = "inputGroup";
            this.inputGroup.Size = new System.Drawing.Size(221, 413);
            this.inputGroup.TabIndex = 1;
            this.inputGroup.TabStop = false;
            this.inputGroup.Text = "参数";
            // 
            // tabDataBase
            // 
            this.tabDataBase.Controls.Add(this.tabPage1);
            this.tabDataBase.Controls.Add(this.tabPage2);
            this.tabDataBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDataBase.Location = new System.Drawing.Point(3, 17);
            this.tabDataBase.Name = "tabDataBase";
            this.tabDataBase.SelectedIndex = 0;
            this.tabDataBase.Size = new System.Drawing.Size(215, 393);
            this.tabDataBase.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txt_dbName);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.btn_DBReset);
            this.tabPage1.Controls.Add(this.btn_DBSetting);
            this.tabPage1.Controls.Add(this.txt_dbpass);
            this.tabPage1.Controls.Add(this.txt_dbuser);
            this.tabPage1.Controls.Add(this.txt_dbpot);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txt_dbip);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(207, 367);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据库";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_DBSetting
            // 
            this.btn_DBSetting.Location = new System.Drawing.Point(110, 184);
            this.btn_DBSetting.Name = "btn_DBSetting";
            this.btn_DBSetting.Size = new System.Drawing.Size(75, 23);
            this.btn_DBSetting.TabIndex = 8;
            this.btn_DBSetting.Text = "确定";
            this.btn_DBSetting.UseVisualStyleBackColor = true;
            this.btn_DBSetting.Click += new System.EventHandler(this.btn_DBSetting_Click);
            // 
            // txt_dbpass
            // 
            this.txt_dbpass.Location = new System.Drawing.Point(68, 126);
            this.txt_dbpass.Name = "txt_dbpass";
            this.txt_dbpass.Size = new System.Drawing.Size(117, 21);
            this.txt_dbpass.TabIndex = 7;
            this.txt_dbpass.Text = "ziguan.123";
            // 
            // txt_dbuser
            // 
            this.txt_dbuser.Location = new System.Drawing.Point(68, 92);
            this.txt_dbuser.Name = "txt_dbuser";
            this.txt_dbuser.Size = new System.Drawing.Size(117, 21);
            this.txt_dbuser.TabIndex = 6;
            this.txt_dbuser.Text = "root";
            // 
            // txt_dbpot
            // 
            this.txt_dbpot.Location = new System.Drawing.Point(68, 55);
            this.txt_dbpot.Name = "txt_dbpot";
            this.txt_dbpot.Size = new System.Drawing.Size(117, 21);
            this.txt_dbpot.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "密码：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "用户名：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP：";
            // 
            // txt_dbip
            // 
            this.txt_dbip.Location = new System.Drawing.Point(68, 18);
            this.txt_dbip.Name = "txt_dbip";
            this.txt_dbip.Size = new System.Drawing.Size(117, 21);
            this.txt_dbip.TabIndex = 0;
            this.txt_dbip.Text = "10.20.31.42";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_Modify);
            this.tabPage2.Controls.Add(this.btn_cfgFile);
            this.tabPage2.Controls.Add(this.txt_cfg);
            this.tabPage2.Controls.Add(this.txt_sys_version_no);
            this.tabPage2.Controls.Add(this.txt_client_config_type);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.txt_operate_no);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(207, 367);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "配置修改";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_Modify
            // 
            this.btn_Modify.Location = new System.Drawing.Point(126, 322);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(75, 23);
            this.btn_Modify.TabIndex = 17;
            this.btn_Modify.Text = "修改";
            this.btn_Modify.UseVisualStyleBackColor = true;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // btn_cfgFile
            // 
            this.btn_cfgFile.Location = new System.Drawing.Point(167, 153);
            this.btn_cfgFile.Name = "btn_cfgFile";
            this.btn_cfgFile.Size = new System.Drawing.Size(34, 23);
            this.btn_cfgFile.TabIndex = 16;
            this.btn_cfgFile.Text = "***";
            this.btn_cfgFile.UseVisualStyleBackColor = true;
            this.btn_cfgFile.Click += new System.EventHandler(this.btn_cfgFile_Click);
            // 
            // txt_cfg
            // 
            this.txt_cfg.Location = new System.Drawing.Point(13, 154);
            this.txt_cfg.Name = "txt_cfg";
            this.txt_cfg.Size = new System.Drawing.Size(147, 21);
            this.txt_cfg.TabIndex = 15;
            this.txt_cfg.Text = "E:\\Work\\Hundsun\\workSource\\配置文件工具\\Ver0.01\\BlueCat\\Source\\BlueCat\\BlueCat.Tests\\bi" +
    "n\\Debug\\ConfigManageTempWork\\TaskConfig\\ConfigTasks.json";
            // 
            // txt_sys_version_no
            // 
            this.txt_sys_version_no.Location = new System.Drawing.Point(111, 90);
            this.txt_sys_version_no.Name = "txt_sys_version_no";
            this.txt_sys_version_no.Size = new System.Drawing.Size(90, 21);
            this.txt_sys_version_no.TabIndex = 14;
            this.txt_sys_version_no.Text = "OPLUS_20171130B";
            // 
            // txt_client_config_type
            // 
            this.txt_client_config_type.Location = new System.Drawing.Point(136, 53);
            this.txt_client_config_type.Name = "txt_client_config_type";
            this.txt_client_config_type.Size = new System.Drawing.Size(65, 21);
            this.txt_client_config_type.TabIndex = 13;
            this.txt_client_config_type.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "配置文件：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "sys_version_no：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "client_config_type：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "operate_no：";
            // 
            // txt_operate_no
            // 
            this.txt_operate_no.Location = new System.Drawing.Point(84, 16);
            this.txt_operate_no.Name = "txt_operate_no";
            this.txt_operate_no.Size = new System.Drawing.Size(117, 21);
            this.txt_operate_no.TabIndex = 8;
            this.txt_operate_no.Text = "10005";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rtb_Monitor);
            this.groupBox2.Location = new System.Drawing.Point(253, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(519, 413);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "运行信息";
            // 
            // btn_DBReset
            // 
            this.btn_DBReset.Location = new System.Drawing.Point(110, 213);
            this.btn_DBReset.Name = "btn_DBReset";
            this.btn_DBReset.Size = new System.Drawing.Size(75, 23);
            this.btn_DBReset.TabIndex = 9;
            this.btn_DBReset.Text = "重置";
            this.btn_DBReset.UseVisualStyleBackColor = true;
            this.btn_DBReset.Click += new System.EventHandler(this.btn_DBReset_Click);
            // 
            // txt_dbName
            // 
            this.txt_dbName.Location = new System.Drawing.Point(68, 157);
            this.txt_dbName.Name = "txt_dbName";
            this.txt_dbName.Size = new System.Drawing.Size(117, 21);
            this.txt_dbName.TabIndex = 11;
            this.txt_dbName.Text = "dbtrade";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 160);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "数据库：";
            // 
            // rtb_Monitor
            // 
            this.rtb_Monitor.Location = new System.Drawing.Point(6, 29);
            this.rtb_Monitor.Name = "rtb_Monitor";
            this.rtb_Monitor.Size = new System.Drawing.Size(507, 381);
            this.rtb_Monitor.TabIndex = 0;
            this.rtb_Monitor.Text = "";
            // 
            // BlueCat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.inputGroup);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "BlueCat";
            this.Text = "BlueCat";
            this.inputGroup.ResumeLayout(false);
            this.tabDataBase.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox inputGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabDataBase;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_dbip;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_dbpass;
        private System.Windows.Forms.TextBox txt_dbuser;
        private System.Windows.Forms.TextBox txt_dbpot;
        private System.Windows.Forms.Button btn_DBSetting;
        private System.Windows.Forms.TextBox txt_sys_version_no;
        private System.Windows.Forms.TextBox txt_client_config_type;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_operate_no;
        private System.Windows.Forms.Button btn_cfgFile;
        private System.Windows.Forms.TextBox txt_cfg;
        private System.Windows.Forms.Button btn_Modify;
        private System.Windows.Forms.Button btn_DBReset;
        private System.Windows.Forms.TextBox txt_dbName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RichTextBox rtb_Monitor;
    }
}

