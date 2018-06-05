using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCat.ConfigTools
{
    public enum FixedStyle
    {
        None,
        Left,
        Right
    }

    public enum ColumnSortOrder
    {
        None,
        Ascending,
        Descending
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperateType
    {
        Insert,
        Delete,
        Modify,
        Query
    }

    /// <summary>
    /// 任务配置文件类型
    /// </summary>
    public enum TaskConfigFileType
    {
        EXCEL,
        JSON
    }

    /// <summary>
    /// 默认价格类型
    /// </summary>
    public enum DefaultPriceType : int
    {
        指定价 = 1,
        盘口价,
        最新价,
        挂单价,
        市价
    }

    /// <summary>
    /// 下单面板功能
    /// </summary>
    public enum OrderPanelFunc : int
    {
        撤单 = 1,
        撤最后一笔和最后一批,
        条件撤单,
        全部撤单,
        改单,
        盘口价改单,
        挂单价改单
    }

    /// <summary>
    /// 埋单面板功能
    /// </summary>
    public enum AdvPanelFunc : int
    {
        立即发出 = 1,
        暂停,
        激活,
        修改,
        撤销,
        查看委托
    }

    /// <summary>
    /// 自动填单后动作
    /// </summary>
    public enum AfterAutoFillOrderDo
    {
        发出委托_需要确认 = 1,
        光标到合约代码,
        光标到买卖,
        光标到开平,
        光标到委托数量,
        光标到委托价格,
        光标到下单按钮,
        光标跳转到卖出单选框,
        光标跳转到买入单选框,
        光标跳转到开仓单选框,
        光标跳转到平仓单选框
    }

    /// <summary>
    /// 套利单参数：下单数量出现小数时的处理方式，目前只支持四舍五入
    /// </summary>
    public enum ArbitrageOrderNumFloat : int
    {
        四舍五入
    }

    /// <summary>
    /// 下单后清空下单版选项
    /// </summary>
    public enum ClearEntrustPanelAfterOrder : int
    {
        不清空 = 1,
        全部清空,
        清空价格和手数,
        只清空手数
    }

    public enum EntrustNumFloat : int
    {
        四舍五入,
        截位,
        进位
    }

    /// <summary>
    /// 下单要素
    /// </summary>
    public enum EntrustElement : int
    {
        合约 = 1,
        买卖,
        开平,
        数量,
        价格
    }

    /// <summary>
    /// 下单后光标位置
    /// </summary>
    public enum FocusLocationAfterOrder : int
    {
        合约代码 = 1,
        价格,
        买卖,
        开平,
        数量
    }

    /// <summary>
    /// 默认手数设置维度
    /// </summary>
    public enum DefaultHandNumSetDimension : int
    {
        品种 = 1,
        合约代码
    }

    /// <summary>
    /// 默认价格分类
    /// </summary>
    public enum DefaultPriceCategory : int
    {
        买入 = 1,
        卖出,
        快捷平仓,
        反手平仓,
        反手开仓,
        一键清仓,
        锁仓
    }

    /// <summary>
    /// 价格类型
    /// </summary>
    public enum PriceType
    {
        指定价 = 1,
        盘口价,
        最新价,
        挂单价,
        市价
    }

    /// <summary>
    /// 价差计算方式
    /// </summary>
    public enum PriceIntervalType
    {
        按配比,
        按一比一
    }

    /// <summary>
    /// 投资类型
    /// </summary>
    public enum FutureInvestType
    {
        //40351:投资类型-a:投机
        //40351:投资类型-b:套保
        //40351:投资类型-c:套利
        投机 = 1,
        套保,
        套利,
        未知类型,
        混合
    }

    /// <summary>
    /// 板块
    /// </summary>
    public enum Plate : int
    {
        盘口行情 = 1,
        资金栏,
        委托面板,
        埋单,
        委托,
        持仓,
        成交,
        资金,
        自定义行情组
    }

    /// <summary>
    /// 键盘数字键
    /// </summary>
    public enum NumShortKey_KeyBoard : int
    {
        [Description("1")]
        D1 = 1,
        [Description("2")]
        D2 = 2,
        [Description("3")]
        D3 = 3,
        [Description("4")]
        D4 = 4,
        [Description("5")]
        D5 = 5,
        [Description("6")]
        D6 = 6,
        [Description("7")]
        D7 = 7,
        [Description("8")]
        D8 = 8,
        [Description("9")]
        D9 = 9
    }

    /// <summary>
    /// 键盘数字键盘数字键
    /// </summary>
    public enum NumShortcut_NumBoard : int
    {
        [Description("1")]
        NumPad1 = 1,
        [Description("2")]
        NumPad2 = 2,
        [Description("3")]
        NumPad3 = 3,
        [Description("4")]
        NumPad4 = 4,
        [Description("5")]
        NumPad5 = 5,
        [Description("6")]
        NumPad6 = 6,
        [Description("7")]
        NumPad7 = 7,
        [Description("8")]
        NumPad8 = 8,
        [Description("9")]
        NumPad9 = 9
    }

    /// <summary>
    /// 查询面板功能
    /// </summary>
    public enum QueryPanelFuntion
    {
        未知,
        锁仓,
        快捷平仓,
        反手,
        撤单,
        全部撤单,
        条件撤单,
        撤最后一笔和最后一批,
        改单,
        盘口价改单,
        挂单价改单,
        立即发出,
        暂停,
        激活,
        修改,
        撤销,
        查看委托
    }

    /// <summary>
    /// 普通埋单类型
    /// </summary>
    public enum CommonAdvType
    {
        手工单 = 1,
        自动单,
        价格单,
        时间单
    }

    /// <summary>
    /// 交易类型
    /// </summary>
    public enum OrderType
    {
        普通委托 = 1,
        止盈止损,
        批量委托,
        本地套利
    }

    /// <summary>
    /// 持仓面板功能
    /// </summary>
    public enum PositionPanelFunc : int
    {
        平仓 = 1,
        反手,
        锁仓
    }

    /// <summary>
    /// 比较方向
    /// </summary>
    public enum CompareDirection
    {
        /// <summary>
        /// 大于
        /// </summary>
        [Description(">")]
        大于 = 1,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("<")]
        小于,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("≥")]
        大于等于,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("≤")]
        小于等于
    }

    public enum Keys
    {
        A = 0x41,
        Add = 0x6b,
        Alt = 0x40000,
        Apps = 0x5d,
        Attn = 0xf6,
        B = 0x42,
        Back = 8,
        BrowserBack = 0xa6,
        BrowserFavorites = 0xab,
        BrowserForward = 0xa7,
        BrowserHome = 0xac,
        BrowserRefresh = 0xa8,
        BrowserSearch = 170,
        BrowserStop = 0xa9,
        C = 0x43,
        Cancel = 3,
        Capital = 20,
        CapsLock = 20,
        Clear = 12,
        Control = 0x20000,
        ControlKey = 0x11,
        Crsel = 0xf7,
        D = 0x44,
        D0 = 0x30,
        D1 = 0x31,
        D2 = 50,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,
        Decimal = 110,
        Delete = 0x2e,
        Divide = 0x6f,
        Down = 40,
        E = 0x45,
        End = 0x23,
        Enter = 13,
        EraseEof = 0xf9,
        Escape = 0x1b,
        Execute = 0x2b,
        Exsel = 0xf8,
        F = 70,
        F1 = 0x70,
        F10 = 0x79,
        F11 = 0x7a,
        F12 = 0x7b,
        F13 = 0x7c,
        F14 = 0x7d,
        F15 = 0x7e,
        F16 = 0x7f,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 130,
        F2 = 0x71,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 120,
        FinalMode = 0x18,
        G = 0x47,
        H = 0x48,
        HanguelMode = 0x15,
        HangulMode = 0x15,
        HanjaMode = 0x19,
        Help = 0x2f,
        Home = 0x24,
        I = 0x49,
        IMEAccept = 30,
        IMEAceept = 30,
        IMEConvert = 0x1c,
        IMEModeChange = 0x1f,
        IMENonconvert = 0x1d,
        Insert = 0x2d,
        J = 0x4a,
        JunjaMode = 0x17,
        K = 0x4b,
        KanaMode = 0x15,
        KanjiMode = 0x19,
        KeyCode = 0xffff,
        L = 0x4c,
        LaunchApplication1 = 0xb6,
        LaunchApplication2 = 0xb7,
        LaunchMail = 180,
        LButton = 1,
        LControlKey = 0xa2,
        Left = 0x25,
        LineFeed = 10,
        LMenu = 0xa4,
        LShiftKey = 160,
        LWin = 0x5b,
        M = 0x4d,
        MButton = 4,
        MediaNextTrack = 0xb0,
        MediaPlayPause = 0xb3,
        MediaPreviousTrack = 0xb1,
        MediaStop = 0xb2,
        Menu = 0x12,
        Modifiers = -65536,
        Multiply = 0x6a,
        N = 0x4e,
        Next = 0x22,
        NoName = 0xfc,
        None = 0,
        NumLock = 0x90,
        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 100,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        O = 0x4f,
        Oem1 = 0xba,
        Oem102 = 0xe2,
        Oem2 = 0xbf,
        Oem3 = 0xc0,
        Oem4 = 0xdb,
        Oem5 = 220,
        Oem6 = 0xdd,
        Oem7 = 0xde,
        Oem8 = 0xdf,
        OemBackslash = 0xe2,
        OemClear = 0xfe,
        OemCloseBrackets = 0xdd,
        Oemcomma = 0xbc,
        OemMinus = 0xbd,
        OemOpenBrackets = 0xdb,
        OemPeriod = 190,
        OemPipe = 220,
        Oemplus = 0xbb,
        OemQuestion = 0xbf,
        OemQuotes = 0xde,
        OemSemicolon = 0xba,
        Oemtilde = 0xc0,
        P = 80,
        Pa1 = 0xfd,
        Packet = 0xe7,
        PageDown = 0x22,
        PageUp = 0x21,
        Pause = 0x13,
        Play = 250,
        Print = 0x2a,
        PrintScreen = 0x2c,
        Prior = 0x21,
        ProcessKey = 0xe5,
        Q = 0x51,
        R = 0x52,
        RButton = 2,
        RControlKey = 0xa3,
        Return = 13,
        Right = 0x27,
        RMenu = 0xa5,
        RShiftKey = 0xa1,
        RWin = 0x5c,
        S = 0x53,
        Scroll = 0x91,
        Select = 0x29,
        SelectMedia = 0xb5,
        Separator = 0x6c,
        Shift = 0x10000,
        ShiftKey = 0x10,
        Sleep = 0x5f,
        Snapshot = 0x2c,
        Space = 0x20,
        Subtract = 0x6d,
        T = 0x54,
        Tab = 9,
        U = 0x55,
        Up = 0x26,
        V = 0x56,
        VolumeDown = 0xae,
        VolumeMute = 0xad,
        VolumeUp = 0xaf,
        W = 0x57,
        X = 0x58,
        XButton1 = 5,
        XButton2 = 6,
        Y = 0x59,
        Z = 90,
        Zoom = 0xfb
    }
}
