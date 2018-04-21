using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;


namespace ServerBackup
{
    public class PingDev
    {
        /// <summary>
        /// 批量检查IP地址是否在线
        /// </summary>
        /// <param name="ips">IP列表</param>
        /// <returns></returns>
        public List<string> CheckIPList(List<string> ipList)
        {
            rtIPS = new List<string>(ipList.Count);//给他赋值，防止溢出
            List<string> tmp = new List<string>();//临时保存报错的IP
            List<string> tmpx = new List<string>();//临时保存报错的IP
            int total = ipList.Count;//记录IP数量

            ThreadPool.QueueUserWorkItem(o =>
            {
                for (int i = ipList.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        Ping p1 = new Ping();
                        p1.PingCompleted += this.PingCompletedCallBack;//设置PingCompleted事件处理程序  
                        p1.SendAsync(ipList[i], 3000, null);
                    }
                    catch (Exception)
                    {
                        tmp.Add(ipList[i] + "=N");
                        tmpx.Add(ipList[i] + "=N");
                        ipList.Remove(ipList[i]);

                    }
                }
            });
            int x = 0;
            while (true)
            {
                if (rtIPS.Count >= ipList.Count)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(10);
                    x++;
                    if (x >= 1000) break;//防止进入死循环50秒没跳出来，就退出这个循环
                }
            }

            //为了兼容SERVER2008
            if (rtIPS.Count < total)
            {
                //把ipList.Remove(ipList[i]);的IP 添加到rtIPS
                rtIPS.AddRange(tmp);
                ipList.AddRange(tmpx);
            }
            //数量达标后，进行把=Y的都赋值，其余都是超时的
            //如果是Y的 都是ping通了的，而且返回IP正常，如过是N的就是没ping通的，但是win7下面返回IP可能是本机
            for (int i = 0; i < ipList.Count; i++)
            {
                bool isFind = false;
                for (int j = 0; j < rtIPS.Count; j++)
                {
                    //如果IP地址包含，并且含有Y，那么就说明这个是ping通了的
                    if ((rtIPS[j].Contains(ipList[i])) && (rtIPS[j].Contains("=Y")))
                    {
                        ipList[i] = ipList[i] + "=Y";
                        isFind = true;
                        break;
                    }
                }
                //循环完了检查是否找到一个正常的Y 且ping通的。否则就是ping不通的
                if (!isFind)
                {
                    ipList[i] = ipList[i] + "=N";
                }
            }
            return ipList;
        }
        List<string> rtIPS = null;//new List<string>();
        private void PingCompletedCallBack(object sender, PingCompletedEventArgs e)
        {

            PingReply reply = e.Reply;
            if (e.Cancelled)
            {
                //退出
                rtIPS.Add(reply.Address.ToString() + "=N");
                return;
            }
            if (e.Error != null)
            {
                //异常e.Error.Message;
                //rtIPS.Add(reply.Address.ToString() + "=N");
                return;
            }

            if (reply.Status == IPStatus.Success)
            {
                rtIPS.Add(reply.Address.ToString() + "=Y");
                return;
            }
            else
            {
                rtIPS.Add(reply.Address.ToString() + "=N");
                return;
            }
        }

    }
}