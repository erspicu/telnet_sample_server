using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
namespace telnet_sample_server
{
    class telnet_server_sample
    {
        static void Main(string[] args)
        {

            TcpListener tcpListener = new TcpListener(IPAddress.Any, 24);
            tcpListener.Start();

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                clinet_deal_thread(tcpClient);
            }
        }


        public class clinet_thread
        {
            TcpClient tcc;
            public clinet_thread(TcpClient tc)
            {
                tcc = tc;
            }
            public void start()
            {

                Console.WriteLine("新的Clinet端連入");

                try
                {
                    ///////
                    while (tcc.Connected )
                    {


                        //歡迎畫面 & 登入檢查


                        //不要本機端的輸入顯示
                        tcc.GetStream().Write(new byte[] { 0xff,251,1 }, 0, 3);
                        tcc.GetStream().ReadByte(); tcc.GetStream().ReadByte(); tcc.GetStream().ReadByte();

                        string tmp = "歡迎登入 erspicu_brox 的 bbs (密碼:1234) \n";

                        tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)';', (byte)'3', (byte)'H' }, 0, 6);
                        tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)';', (byte)'5', (byte)'m' }, 0, 6);
                        tcc.GetStream().Write(Encoding.Default.GetBytes(tmp), 0, Encoding.Default.GetBytes(tmp).Length);
                        tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[',  (byte)'m' }, 0, 3);
                        tmp = "請輸入登入密碼 : ";
                        tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)';', (byte)'3', (byte)'H' }, 0, 6);
                        tcc.GetStream().Write(Encoding.Default.GetBytes(tmp), 0, Encoding.Default.GetBytes(tmp).Length);


                        int readclinet = 0;
                        List<byte> get_line = new List<byte>();
                        while (Encoding.Default.GetString(get_line.ToArray()).Replace("\r", "") != "1234")
                        {
                            get_line.Clear();
                            while (readclinet != '\r')
                            {
                                readclinet = tcc.GetStream().ReadByte();

                                if (readclinet != '\r')
                                    tcc.GetStream().Write(Encoding.Default.GetBytes("*"), 0, 1);
                                get_line.Add((byte)readclinet);

                            }
                            readclinet = 0;

                            if (Encoding.Default.GetString(get_line.ToArray()).Replace("\r", "") != "1234")
                            {
                                tmp = "密碼錯誤,請重新輸入 : ";
                                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)';', (byte)'3', (byte)'H' }, 0, 6);
                                tcc.GetStream().Write(Encoding.Default.GetBytes(tmp), 0, Encoding.Default.GetBytes(tmp).Length);
                                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'0', (byte)'K' }, 0, 4);
                            }

                        }

                        tmp = "登入成功.按任意鍵繼續...";
                        tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'5', (byte)';', (byte)'3', (byte)'H' }, 0, 6);
                        tcc.GetStream().Write(Encoding.Default.GetBytes(tmp), 0, Encoding.Default.GetBytes(tmp).Length);

                        tcc.GetStream().ReadByte();
                        tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'2', (byte)'J' }, 0, 4);

                        /*
                         * 登入後功能延伸擴展
                         * 
                         * .....................................................
                         * .....................................................
                         * .....................................................
                         * 
                         * 
                        */

                        //關閉
                        tcc.GetStream().Close();
                        tcc.Close();
                        

                    }
                    //////
                    Console.WriteLine("Clinet已登出.");
                }
                catch
                {
                    Console.WriteLine("Clinet已意外關閉連結");
                }


            }
        }


        static public void clinet_deal_thread(TcpClient tc)
        {

            clinet_thread clt = new clinet_thread(tc);
            Thread c = new Thread(clt.start);
            c.Start();
        }
    }
}
