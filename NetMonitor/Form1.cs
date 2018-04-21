using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
namespace NetMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool NICValid = true;
        string InfoString = "";
        private void Form1_Load(object sender, EventArgs e)
        {

            //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //ManagementObjectCollection moc = mc.GetInstances();

            //foreach (ManagementObject mo in moc) //查找网卡配置
            //{
            //    string s = ((string)mo["Caption"]);

            //    if (s == "NIC")
            //    {
            //        if ((bool)mo["DHCPEnabled"])  //此网卡未配置过IP
            //        {
            //            string[] ips = new string[1] { "LocalIP" };
            //            string[] masks = new string[1] { "StationIPmask" };
            //            Object[] objs = new object[2] { ips, masks };
            //            mo.InvokeMethod("EnableStatic", objs);
            //            NICValid = false;
            //            return;
            //        }

            //        string[] str = (string[])mo["IPAddress"]; //多个ip地址

            //        if (str.Length >= 3)
            //        {
            //            InfoString = "不能有多个IP地址绑定在此网卡上！";
            //            NICValid = false;
            //            return;
            //        }

            //        if (str.Length < 0)
            //        {
            //            InfoString = "请检查网卡设备！";
            //            NICValid = false;
            //            return;
            //        }

            //        if (str[0] == "0.0.0.0")
            //        {
            //            InfoString = "请检查网卡与交换机正确连接！";
            //            NICValid = false;
            //            return;
            //        }

            //    }
            //}
        }
    }
}
