using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace telnet_sample_server
{
    public class clinet_tools
    {
        public TcpClient tcc;



        public clinet_tools(TcpClient tcc_in)
        {
            tcc = tcc_in;
        }

        #region 一般程式通用工具
        public string get_sha1(string str)
        {

            byte[] buffer = Encoding.Default.GetBytes(str);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

        }
        #endregion


        #region 螢幕輸出相關

        //--畫面輸出
        public void ask_no_clinet_echo()//要求不要本機端的輸入顯示
        {
            tcc.GetStream().Write(new byte[] { 0xff, 251, 1 }, 0, 3);
            tcc.GetStream().ReadByte(); tcc.GetStream().ReadByte(); tcc.GetStream().ReadByte();
        }

        public void printfile(string path) //列印檔案內容
        {
            StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + path);
            byte[] buf = new byte[sr.BaseStream.Length];
            sr.BaseStream.Read(buf, 0, (int)sr.BaseStream.Length);
            tcc.GetStream().Write(buf, 0, (int)sr.BaseStream.Length);
            sr.Close();
        }

        public string get_file_text(string path)
        {
            StreamReader sr = new StreamReader(path);
            byte[] buf = new byte[sr.BaseStream.Length];
            sr.BaseStream.Read(buf, 0, (int)sr.BaseStream.Length);
            sr.Close();
            return Encoding.Default.GetString(buf);
        }

        public void print(string str, int x, int y)//列印字串
        {
            byte[] buf_str = Encoding.GetEncoding("Big5").GetBytes(str);
            int buf_len = buf_str.Length;
            List<byte> buf_loc = Encoding.GetEncoding("Big5").GetBytes("[" + x + ";" + y + "H").ToList();
            buf_loc.Insert(0, 0x1b);
            tcc.GetStream().Write(buf_loc.ToArray(), 0, buf_loc.Count);
            tcc.GetStream().Write(buf_str, 0, buf_len);
        }

        public void print_inputbox(int length, ConsoleColor fcolor, ConsoleColor bcolor)
        {
            setColor(fcolor, bcolor);

            int c = 0;

            while (c < length)
            {
                c++;
                tcc.GetStream().Write(new byte[] { 0x20 }, 0, 1);
            }

            while (length > 0)
            {
                length--;
                tcc.GetStream().Write(new byte[] { (byte)'\b' }, 0, 1);
            }

        }

        //--清除畫面
        public void clearscreen() //清除全螢幕
        {
            tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'2', (byte)'J' }, 0, 4);
        }

        public void backclear(int length) //列印 \b
        {
        }

        public void clear_beginLine2cursor()//從光標所在行開始清除到光標
        {

        }

        public void clear_cursor2LineEnd()//從光標位置清除到行末
        {
            tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'K' }, 0, 3);
        }

        public void clear_cursorLine() // 清除光標所在那一整行 ??
        {
        }

        //--光標
        public void set_cursor_loc(int x, int y)//設定光標位置
        {
            List<byte> buf_loc = Encoding.GetEncoding("Big5").GetBytes("[" + x + ";" + y + "H").ToList();
            buf_loc.Insert(0, 0x1b);
            tcc.GetStream().Write(buf_loc.ToArray(), 0, buf_loc.Count);
        }
        public void set_cursor_loc_org()//恢復到 1,1
        {
            tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'H' }, 0, 3);
        }

        //--色彩
        public void reset_color()//顏色重置
        {
            tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'m' }, 0, 3);
        }
        public void setForegroundColor(ConsoleColor fcolor) //設定前景色
        {
            if (fcolor == ConsoleColor.DarkGray)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'0', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Red)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'1', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Green)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'2', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Yellow)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'3', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Blue)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'4', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Magenta)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'5', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Cyan)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'6', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.White)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'7', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Black)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'0', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkRed)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'1', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkGreen)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'2', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkYellow)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'3', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkBlue)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'4', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkMagenta)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'5', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkCyan)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'6', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.Gray)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'7', (byte)'m' }, 0, 5);
        }
        public void setBackgroundColor(ConsoleColor bcolor)//設定背景色
        {
            if (bcolor == ConsoleColor.Black)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'0', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Red)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'1', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Green)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'2', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Yellow)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'3', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Blue)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'4', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Magenta)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'5', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Cyan)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'6', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.White)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'7', (byte)'m' }, 0, 5);
        }
        public void setColor(ConsoleColor fcolor, ConsoleColor bcolor)//設定前後景色
        {
            if (fcolor == ConsoleColor.DarkGray)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'0', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Red)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'1', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Green)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'2', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Yellow)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'3', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Blue)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'4', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Magenta)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'5', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Cyan)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'6', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.White)
            {
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'1', (byte)'m' }, 0, 4);
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'7', (byte)'m' }, 0, 5);
            }
            else if (fcolor == ConsoleColor.Black)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'0', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkRed)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'1', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkGreen)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'2', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkYellow)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'3', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkBlue)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'4', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkMagenta)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'5', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.DarkCyan)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'6', (byte)'m' }, 0, 5);
            else if (fcolor == ConsoleColor.Gray)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'3', (byte)'7', (byte)'m' }, 0, 5);

            if (bcolor == ConsoleColor.Black)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'0', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Red)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'1', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Green)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'2', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Yellow)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'3', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Blue)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'4', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Magenta)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'5', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.Cyan)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'6', (byte)'m' }, 0, 5);
            else if (bcolor == ConsoleColor.White)
                tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'4', (byte)'7', (byte)'m' }, 0, 5);
        }
        #endregion

        #region 鍵盤輸入相關

        //得取操作選單一般通用的操作key
        public clinet_keyinput get_input_op()
        {
            byte[] buf = new byte[4];
            clinet_keyinput ck = new clinet_keyinput();
            int i = tcc.GetStream().Read(buf, 0, 4);

            ck.count = i;

            if (i == 1)
            {
                ck.SKeyTyep = SpecialKeyType.NULL;
                ck.key = (char)buf[0];

                if (buf[0] == 0x7f || buf[0] == 0x08)
                    ck.key = '\b';
            }

            if (i == 4 || i == 3) //這地方很奇怪...
            {
                if (buf[0] == 0x1b && buf[1] == 0x4f && buf[2] == 0x41)//上
                    ck.SKeyTyep = SpecialKeyType.Up;
                else if (buf[0] == 0x1b && buf[1] == 0x4f && buf[2] == 0x42)//下
                    ck.SKeyTyep = SpecialKeyType.Down;
                else if (buf[0] == 0x1b && buf[1] == 0x4f && buf[2] == 0x44)//左
                    ck.SKeyTyep = SpecialKeyType.Left;
                else if (buf[0] == 0x1b && buf[1] == 0x4f && buf[2] == 0x43)
                    ck.SKeyTyep = SpecialKeyType.Right;
                else if (buf[0] == 0x1b && buf[1] == 0x5b && buf[2] == 0x35 && buf[3] == 0x7e)
                    ck.SKeyTyep = SpecialKeyType.PageUp;
                else if (buf[0] == 0x1b && buf[1] == 0x5b && buf[2] == 0x36 && buf[3] == 0x7e)
                    ck.SKeyTyep = SpecialKeyType.PageUp;

            }
            return ck;
        }

        public int pause()
        {

            return tcc.GetStream().ReadByte();
        }

        public string get_input_char() //取得輸入字元
        {
            return "";
        }

        public string get_input_char_with_enter()//從enter取得輸入字元
        {
            return "";
        }

        public string get_input_password(int max_length) //取得密碼,不輸出任何顯示
        {
            int readclinet = 0;
            List<byte> get_line = new List<byte>();
            while (readclinet != '\r')
            {
                readclinet = tcc.GetStream().ReadByte();
                if (readclinet != '\r')
                {
                    if (readclinet == 0x08 && get_line.Count != 0)
                        tcc.GetStream().Write(new byte[] { 0x08, 0x20, 08 }, 0, 3);

                    if (readclinet == 0x7f && get_line.Count != 0)
                        tcc.GetStream().Write(new byte[] { 0x08, 0x20, 08 }, 0, 3);
                }
                if (readclinet != 0x08 && readclinet != 0x7f && get_line.Count < max_length)
                    get_line.Add((byte)readclinet);
                if ((readclinet == 0x08 || readclinet == 0x7f) && get_line.Count != 0)
                    get_line.RemoveAt(get_line.Count - 1);
            }
            return Encoding.GetEncoding("Big5").GetString(get_line.ToArray()).Replace("\r", "");
        }

        public string get_input_password(int max_length, char password_char) //取得密碼,輸出隱藏碼
        {
            int readclinet = 0;
            List<byte> get_line = new List<byte>();
            while (readclinet != '\r')
            {
                readclinet = tcc.GetStream().ReadByte();
                if (readclinet != '\r')
                {
                    if (readclinet != 0x08 && readclinet != 0x7f && get_line.Count < max_length)
                        tcc.GetStream().WriteByte((byte)'*');
                    if (readclinet == 0x08 && get_line.Count != 0)
                        tcc.GetStream().Write(new byte[] { 0x08, 0x20, 08 }, 0, 3);

                    if (readclinet == 0x7f && get_line.Count != 0)
                        tcc.GetStream().Write(new byte[] { 0x08, 0x20, 08 }, 0, 3);
                }
                if (readclinet != 0x08 && readclinet != 0x7f && get_line.Count < max_length)
                    get_line.Add((byte)readclinet);
                if ((readclinet == 0x08 || readclinet == 0x7f) && get_line.Count != 0)
                    get_line.RemoveAt(get_line.Count - 1);
            }
            return Encoding.GetEncoding("Big5").GetString(get_line.ToArray()).Replace("\r", "");
        }

        public string get_input_line(int max_length)//取得字串
        {

            int readclinet = 0;
            List<byte> get_line = new List<byte>();

            while (readclinet != '\r')
            {
                readclinet = tcc.GetStream().ReadByte();

                if (readclinet != '\r')
                {

                    if (readclinet != 0x08 && readclinet != 0x7f && get_line.Count < max_length)
                        tcc.GetStream().WriteByte((byte)readclinet);

                    if (readclinet == 0x08 && get_line.Count != 0)
                    {
                        tcc.GetStream().Write(new byte[] { 0x08, 0x20, 08 }, 0, 3);
                    }

                    if (readclinet == 0x7f && get_line.Count != 0)
                    {
                        tcc.GetStream().Write(new byte[] { 0x08, 0x20, 08 }, 0, 3);
                    }
                }
                if (readclinet != 0x08 && readclinet != 0x7f && get_line.Count < max_length)
                    get_line.Add((byte)readclinet);
                if ((readclinet == 0x08 || readclinet == 0x7f) && get_line.Count != 0)
                    get_line.RemoveAt(get_line.Count - 1);
            }
            return Encoding.GetEncoding("Big5").GetString(get_line.ToArray()).Replace("\r", "");
        }
        #endregion
    }
}
