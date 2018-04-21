using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace ServerBackup
{
    public partial class FrmServerBackup : Form
    {
        
        string publicIP = "";//公用IP地址
        string ab = "A";
        public FrmServerBackup()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            //读取当前是主机还是备机
            try
            {
                StreamReader srd = new StreamReader(Application.StartupPath + "\\AB.txt");
                ab = srd.ReadToEnd().Trim();
                srd.Close();
                lblST.Text = "当前服务器:"+ab;
                StreamReader srd1 = new StreamReader(Application.StartupPath + "\\public ip.txt");
                publicIP = srd1.ReadToEnd();
                srd1.Close();
                lblIP.Text = "公用IP地址："+publicIP;
                cmd("setip1.bat");//复原IP地址
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件读取错误:"+ex.Message);
            }
            this.Text = "服务器"+ab+" OK";
        }

        private void cmd(string fileName)
        {
            txtInfo.Text += "执行"+ fileName + "\r\n";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.FileName = Application.StartupPath + "\\" + fileName;
            process.Start();
        }
        int errcount = 0;
        private void checkIP()
        {
            PingDev p = new PingDev();
            List<string> ips = new List<string>();
            ips.Clear();
            ips.Add(publicIP);
            ips = p.CheckIPList(ips);
            if (ips.Count != 0)
            {
                //判断是否ping通
                if (ips[0].Contains("=N"))
                {
                    ips[0]=ips[0].Replace("=N", "");
                    //ping 不通就再ping一次
                    txtInfo.Text += "第一次ping不通\r\n";
                    lblIP.ForeColor = Color.Red;
                    Thread.Sleep(1000);
                    ips = p.CheckIPList(ips);
                    if (ips.Count != 0)
                    {
                        if (ips[0].Contains("=N"))
                        {
                            //第二次不通，继续ping第三次
                            ips[0] = ips[0].Replace("=N", "");
                            txtInfo.Text += "第二次ping不通\r\n";
                            lblIP.ForeColor = Color.Red;
                            Thread.Sleep(1000);
                            ips = p.CheckIPList(ips);
                            if (ips.Count != 0)
                            {
                                if (ips[0].Contains("=N"))
                                {
                                    txtInfo.Text += "第三次ping不通\r\n";
                                    lblIP.ForeColor = Color.Red;
                                    //第三次都ping不通，就切换自己的IP为公用IP
                                    cmd("setip3.bat");
                                    Thread.Sleep(5000);
                                    //重启MYSQL服务,重启MSS服务
                                    cmd("restServer.bat");

                                    //如果网线拔掉，就提示网线拔掉，
                                    //循环一直等到网卡连接正常,但是下面的代码不能监测指定网卡的状态
                                    //Network.LocalConnectionStatus();
                                    if (errcount >= 2)
                                    {
                                        // 如果连续执行重复了2次。
                                        //复原IP地址一次,防止因为IP地址冲突导致公用IP永远不能ping通
                                        cmd("setip1.bat");//复原IP地址，复原为非公用IP
                                        errcount = 0;//复原计数器
                                        Thread.Sleep(40000);

                                    }
                                    else
                                    {
                                        errcount++;//计数器+1，防止程序反复执行CMD
                                        Thread.Sleep(5000);
                                    } 
                                   
                                }
                            }
                        }
                    }
                }
                else
                {
                    //能ping通，不执行任何操作，
                    lblIP.ForeColor = Color.Green;
                }
            }
        }
        Thread t1 = null;
        Thread t2 = null;
        Thread t3 = null;
        private void FrmServerBackup_Load(object sender, EventArgs e)
        {

            t1 = new Thread(new ThreadStart(poll));
            t1.Start();//网络检查

            t2 = new Thread(new ThreadStart(checkMss));
            t2.Start();//MSS进程是否存在检查

            t3 = new Thread(new ThreadStart(checkMssMem));
            t3.Start();//MSS内存检查
        }

        private void poll()
        {
            if (ab == "B")
            {
                txtInfo.Text += "当前是B机，延迟15秒启动轮询。\r\n";
                Thread.Sleep(15000);
            }
            Thread.Sleep(8000);//修改了IP地址以后，一定要等几秒，等网络系统反应过来
            while (true)
            {
                try
                {
                    if (ab == "B")
                    {
                        //txtInfo.Text += "当前是B机，再延迟10秒启动下一次轮询。\r\n";
                        Thread.Sleep(6000);
                    }
                    lblLight.ForeColor =Color.Green;
                    Thread.Sleep(2000);//修改了IP地址以后，
                    checkIP();
                    lblLight.ForeColor = Color.Red;
                    Thread.Sleep(1000);
                    if (txtInfo.TextLength > 10000) txtInfo.Text = "";
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void FrmServerBackup_FormClosing(object sender, FormClosingEventArgs e)
        {
            t1.Abort();
            t2.Abort();
            t3.Abort();
            Application.Exit();
        }
        /// <summary>
        /// 只检查进程是否存在
        /// </summary>
        private void checkMss()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(50000);
                    lblProcess.Text = "50秒检查MSS进程一次";
                    int exists = 0;

                    System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process process in processList)
                    {
                        if (process.ProcessName.ToUpper().Replace(".EXE", "") == "MSS")
                        {
                            //MSS工作正常
                            exists = 1;
                        }
                    }
                    if (exists == 0)
                    {
                        //MSS进程不存在
                        //重启MYSQL服务,重启MSS服务
                        cmd("restServer.bat");
                    }
                    lblProcess.Text = "MSS进程";
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                    continue;
                } 
            }
        }

     
        /// <summary>
        /// 检查进程的内存使用率是不是长时间一样
        /// 如果进程在几分钟内，进程的内存使用率一样，那么就重启MSS
        /// </summary>
        private void checkMssMem()
        {
            while (true)
            {
                try
                {
                    lblMem.Text = "50秒检查一次MSS内存";
                    int count = 0;
                    long lastMem = 0;
                    System.Diagnostics.Process[] processList = System.Diagnostics.Process.GetProcesses();
                    foreach (System.Diagnostics.Process process in processList)
                    {
                        if (process.ProcessName.ToUpper().Replace(".EXE", "") == "MSS")
                        {
                            //获取内存使用率
                            long currMem = process.WorkingSet64 / 1024;
                            if (currMem == lastMem) count++;// 如果当前值=上一次的值，那么count++
                            if (currMem != lastMem) count = 0;//如果当前值不等于上一次的值，那么count=0
                            lastMem = currMem;
                        }
                    }
                    if (count >= 30)
                    {
                        //MSS进程假死，也有可能是MQ故障
                        //重启MYSQL服务,重启MSS服务
                        cmd("restServer.bat");
                        count = 0;//复原，以免反复重启
                    }
                    Thread.Sleep(50000);
                    lblMem.Text = "MSS内存";
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                    continue;
                }
            }
        }
    }
}
