using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace telnet_sample_server
{
    class telnet_server_sample
    {
        static void Main(string[] args)
        {

            TcpListener tcpListener = new TcpListener(IPAddress.Any, 24);
            tcpListener.Start();

            Console.WriteLine("Telnet BBS Server 啟動.."); 

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                clinet_deal_thread(tcpClient);
                Console.WriteLine("新的Clinet端連入: " + tcpClient.GetHashCode().ToString() );
            }
        }

        static public void clinet_deal_thread(TcpClient tc)
        {
            clinet_thread clinet_thread_obj = new clinet_thread(tc);
            Thread clinet_conn = new Thread(clinet_thread_obj.start);
            clinet_conn.Start();
        }
    }
}
