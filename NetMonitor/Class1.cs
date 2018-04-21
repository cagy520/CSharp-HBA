using System;
using System.Runtime.InteropServices;
using System.Threading;

public sealed class NetworkHelper
{
    #region 网络状态

    /// <summary>
    /// 监听计时器
    /// </summary>
    private Timer listenTimer;


    private static NetworkHelper instance;

    /// <summary>
    /// 监听间隔
    /// </summary>
    const int LISTEN_TIME_SPAN = 2000;

    //IsNetworkAlive Description
    const int NETWORK_ALIVE_LAN = 1;
    const int NETWORK_ALIVE_WAN = 2;
    const int NETWORK_ALIVE_AOL = 4;

    const int FLAG_ICC_FORCE_CONNECTION = 1;


    private NetworkHelper()
    {

    }

    static NetworkHelper()
    {
        instance = new NetworkHelper();
    }


    public static NetworkHelper GetNetworkHelperInstance()
    {
        return instance;
    }

    /// <summary>
    /// 检查网络是否连通，有延迟
    /// </summary>
    /// <param name="connectionDescription"></param>
    /// <param name="reservedValue"></param>
    [DllImport("wininet.dll")]
    private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lpszUrl">连接饿地址</param>
    /// <param name="dwFlags">FLAG_ICC_FORCE_CONNECTION</param>
    /// <param name="dwReserved">0</param>
    [DllImport("wininet.dll")]
    private extern static bool InternetCheckConnection(string lpszUrl, int dwFlags, int dwReserved);

    /// <summary>
    /// 检查网络是否连通,需要启动服务
    /// </summary>
    /// <param name="connectionDescription"></param>
    [DllImport("sensapi.dll")]
    private extern static bool IsNetworkAlive(out int connectionDescription);

    /// <summary>
    /// 检查是否能建立Internet连接，VISTA不可用
    /// </summary>
    /// <param name="dest"></param>
    /// <param name="ptr">0</param>
    [DllImport("sensapi.dll")]
    private extern static bool IsDestinationReachable(string dest, IntPtr ptr);


    /// <summary>
    /// 互联网是否可用
    /// </summary>
    /// <returns></returns>
    public bool IsInternetAlive()
    {
        int status;
        //检查网络是否可用
        if (IsNetworkAlive(out status))
        {
            //如果WAN可用，检查能否建立连接
            //if (status == NETWORK_ALIVE_WAN)
            //{
            if (InternetCheckConnection("http://www.baidu.com", FLAG_ICC_FORCE_CONNECTION, 0) ||
                InternetCheckConnection("http://www.sina.com.cn", FLAG_ICC_FORCE_CONNECTION, 0) ||
                InternetCheckConnection("http://www.163.com", FLAG_ICC_FORCE_CONNECTION, 0))
            {
                return true; //如果能建立连接返回TRUE
            }
            else
                return false;
            //}
            //else
            //    return false;
        }
        return false;
    }

    /// <summary>
    /// 为NetworkStatusChanged事件处理程序提供数据
    /// </summary>
    public class NetworkChangedEventArgs : EventArgs
    {
        public NetworkChangedEventArgs(bool status)
        {
            IsNetworkAlive = status;
        }

        public bool IsNetworkAlive
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// 表示NetworkStatusChanged事件的方法
    /// </summary>
    public delegate void NetworkChangedEventHandler(object sender, NetworkChangedEventArgs e);

    /// <summary>
    /// 网络状态变更时触发的事件
    /// </summary>
    public event NetworkChangedEventHandler NetworkStatusChanged;

    /// <summary>
    /// 网络状态变更时触发的事件
    /// </summary>
    private void OnNetworkStatusChanged(NetworkChangedEventArgs e)
    {
        if (NetworkStatusChanged != null)
            NetworkStatusChanged(this, e);
    }


    /// <summary>
    /// 监听网络状态
    /// </summary>
    public void ListenNetworkStatus(SynchronizationContext context)
    {
        //获得当前网络状态，并通知

        bool currentStatus = IsInternetAlive();
        OnNetworkStatusChanged(new NetworkChangedEventArgs(currentStatus));

        //启动监听网络状态，2秒钟检查一次，当状态变更时触发事件
        listenTimer = new Timer(delegate
        {
            bool tmpStatus = IsInternetAlive();
            if (currentStatus != tmpStatus)
            {
                currentStatus = tmpStatus;
                context.Post(delegate { OnNetworkStatusChanged(new NetworkChangedEventArgs(currentStatus)); }, null);

            }
        }
                                , null
                                , 0
                                , LISTEN_TIME_SPAN);

    }


    /// <summary>
    /// 停止监听网络状态
    /// </summary>
    public void CloseListenNetworkStatus()
    {
        listenTimer.Dispose();
    }
}
#endregion