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
    public class clinet_thread : telnet_sample_server.Iclinet_thread
    {
        public TcpClient tcc;
        public clinet_tools tools;
        public clinet_datastruct clinet_profile;

        public clinet_thread(TcpClient tc)
        {
            tcc = tc;
            tools = new clinet_tools(tcc);
        }
        public void get_system_info()
        {

            tools.clearscreen();
            tools.setForegroundColor(ConsoleColor.Green);

            string netvm = "";
            Type t = Type.GetType("Mono.Runtime");
            if (t != null)
                netvm = "mono";
            else
                netvm = "Microsoft .NET Framework";

            tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'2', (byte)'2', (byte)';', (byte)'1', (byte)'H' }, 0, 7);
            string netframeworkinfo = netvm + " " + System.Environment.Version.ToString();
            tcc.GetStream().Write(Encoding.GetEncoding("Big5").GetBytes(netframeworkinfo), 0, Encoding.GetEncoding("Big5").GetBytes(netframeworkinfo).Length);

            string cpu_arch = "";
            if (System.Environment.Is64BitOperatingSystem == true)
                cpu_arch = "x64";
            else
                cpu_arch = "x86";
            string os_name = System.Environment.OSVersion.ToString() + " " + cpu_arch;
            tcc.GetStream().Write(new byte[] { 0x1b, (byte)'[', (byte)'2', (byte)'3', (byte)';', (byte)'1', (byte)'H' }, 0, 7);
            tcc.GetStream().Write(Encoding.GetEncoding("Big5").GetBytes(os_name), 0, Encoding.GetEncoding("Big5").GetBytes(os_name).Length);
        }
        public void Server_Exit()
        {
            tools.clearscreen();
            tools.printfile(@"/etc/goodbye.ansi");
            tcc.GetStream().ReadByte();
            tcc.GetStream().Close();
            tcc.Close();
        }

        int main_menu_ch = 1;
        public void system_main_menu()
        {

        begian:
            tools.clearscreen();
            get_system_info();
            tools.set_cursor_loc_org();
            tools.printfile(@"/etc/title.ansi");
            tools.printfile(@"/etc/info.ansi");
            tools.printfile(@"/etc/menu.ansi");
            tools.setColor(ConsoleColor.White, ConsoleColor.Yellow);
            tools.print("我是 [" + clinet_profile.id + "]", 0, 0);
            tools.reset_color();

            tools.print("★", 16 + main_menu_ch, 19); ;

            while (true)
            {

                clinet_keyinput ck = tools.get_input_op();

                if ((ck.key == '\r' || ck.SKeyTyep == SpecialKeyType.Right) && main_menu_ch == 0)
                {
                    Server_ListFavoriteBoards();
                    break;
                }
                else if ((ck.key == '\r' || ck.SKeyTyep == SpecialKeyType.Right) && main_menu_ch == 1)
                {
                    Server_ListBoards();
                    break;
                }
                else if ((ck.key == '\r' || ck.SKeyTyep == SpecialKeyType.Right) && main_menu_ch == 2)
                {
                    Server_MailMenu();
                    break;
                }
                else if ((ck.key == '\r' || ck.SKeyTyep == SpecialKeyType.Right) && main_menu_ch == 3)
                {
                    Server_SetupMenu();
                    break;
                }
                else if ((ck.key == '\r' || ck.SKeyTyep == SpecialKeyType.Right) && main_menu_ch == 4)
                {
                    Server_Exit();
                    break;
                }

                if (ck.key == 'f')
                {
                    main_menu_ch = 0;
                    tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                    tools.print("★", 16, 19);
                }
                if (ck.key == 'b')
                {
                    main_menu_ch = 1;
                    tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                    tools.print("★", 17, 19);
                }
                if (ck.key == 'm')
                {
                    main_menu_ch = 2;
                    tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                    tools.print("★", 18, 19);
                }
                if (ck.key == 's')
                {
                    main_menu_ch = 3;
                    tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                    tools.print("★", 19, 19);
                }
                if (ck.key == 'e')
                {
                    main_menu_ch = 4;
                    tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                    tools.print("★", 20, 19);
                }
                if (ck.count == 3)
                {
                    if (ck.SKeyTyep == SpecialKeyType.Up)
                    {
                        if (main_menu_ch <= 4 && main_menu_ch > 0)
                        {
                            if (main_menu_ch == 4)
                            {
                                main_menu_ch = 3;
                                tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                                tools.print("★", 19, 19);
                            }
                            else if (main_menu_ch == 3)
                            {
                                main_menu_ch = 2;
                                tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                                tools.print("★", 18, 19);
                            }
                            else if (main_menu_ch == 2)
                            {
                                main_menu_ch = 1;
                                tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                                tools.print("★", 17, 19);
                            }
                            else if (main_menu_ch == 1)
                            {
                                main_menu_ch = 0;
                                tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                                tools.print("★", 16, 19);
                            }
                        }
                    }
                    if (ck.SKeyTyep == SpecialKeyType.Down)
                    {
                        if (main_menu_ch == 0)
                        {
                            main_menu_ch = 1;
                            tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                            tools.print("★", 17, 19);
                        }
                        else if (main_menu_ch == 1)
                        {
                            main_menu_ch = 2;
                            tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                            tools.print("★", 18, 19);
                        }
                        else if (main_menu_ch == 2)
                        {
                            main_menu_ch = 3;
                            tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                            tools.print("★", 19, 19);
                        }
                        else if (main_menu_ch == 3)
                        {
                            main_menu_ch = 4;
                            tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                            tools.print("★", 20, 19);
                        }
                    }
                }
            }
            goto begian;
        }
        public void registration()
        {
            tools.clearscreen();
            bool check_ok = false;
            string newid = "";
            while (check_ok == false)
            {
                tools.print("請輸入註冊帳號 : ", 2, 5);

                tools.print_inputbox(10, ConsoleColor.Red, ConsoleColor.White);
                newid = tools.get_input_line(12);

                //判斷是否已被註冊過
                if (Directory.Exists(Directory.GetCurrentDirectory() + "/UserID/" + newid + "/"))
                {
                    tools.reset_color();
                    tools.print("本ID已被註冊過,請改換註冊ID", 3, 5);
                    check_ok = false;
                }
                else
                {
                    tools.set_cursor_loc(3, 5);
                    tools.clear_cursorLine();
                    check_ok = true;
                    tools.reset_color();
                    tools.print("OK!", 2, 0);
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/UserID/" + newid + "/");
                }
            }
            //

            tools.print("請輸入註冊密碼 : ", 4, 5);
            tools.print_inputbox(10, ConsoleColor.Red, ConsoleColor.White);
            string passwd = tools.get_input_password(10, '*');
            tools.reset_color();
            tools.print("OK!", 4, 0);

            tools.print("請輸入暱稱 : ", 6, 5);
            tools.print_inputbox(10, ConsoleColor.Red, ConsoleColor.White);
            string nickname = tools.get_input_line(10);

            tools.reset_color();
            tools.print("OK!", 6, 0);

            tools.print("請輸電子信箱 : ", 8, 5);
            tools.print_inputbox(22, ConsoleColor.Red, ConsoleColor.White);
            string mailurl = tools.get_input_line(20);

            tools.reset_color();
            tools.print("OK!", 8, 0);

            clinet_profile = new clinet_datastruct();

            clinet_profile.id = newid;
            clinet_profile.nickman = nickname;
            clinet_profile.passwd = passwd;
            clinet_profile.mail = mailurl;

            //建立帳號
            File.WriteAllText(Directory.GetCurrentDirectory() + "/UserID/" + newid + "/" + ".PassWord", tools.get_sha1(passwd));

            tools.print("恭喜!註冊完畢.按任意鍵繼續..", 10, 5);
            tools.pause();
        }
        public bool login_check(string id)
        {

            //檢查此帳號是否存在
            if (!File.Exists(Directory.GetCurrentDirectory() + "/UserID/" + id + "/.PassWord"))
            {
                tools.print("此帳號不存在,按任意鍵繼續..", 20, 0);
                tools.pause();
                tools.set_cursor_loc(20, 0);
                tools.clear_cursor2LineEnd();
                return false;
            }

            //輸入密碼
            tools.print("請輸入密碼 : ", 18, 0);
            tools.print_inputbox(12, ConsoleColor.DarkBlue, ConsoleColor.White);
            string passwd = tools.get_input_password(10, '*');
            tools.reset_color();

            string sha1 = File.OpenText(Directory.GetCurrentDirectory() + "/UserID/" + id + "/.PassWord").ReadToEnd();



            while (sha1 != tools.get_sha1(passwd))
            {
                tools.print("密碼錯誤,按任意鍵繼續...", 20, 0);
                tools.pause();
                tools.set_cursor_loc(20, 0);
                tools.clear_cursor2LineEnd();

                tools.print("請輸入密碼 : ", 18, 0);
                tools.print_inputbox(12, ConsoleColor.DarkBlue, ConsoleColor.White);
                passwd = tools.get_input_password(10, '*');
                tools.reset_color();
            }

            tools.print("帳號密碼確認無誤,按任意鍵繼續...", 20, 0);
            tools.pause();

            clinet_profile = new clinet_datastruct();
            clinet_profile.id = id;

            return true;
        }
        public void Server_MailMenu()
        {

            tools.clearscreen();
            tools.set_cursor_loc_org();

            while (true)
            {
                tools.print("郵件功能選單曲,功能尚未完成,按任意鍵繼續", 1, 1);
                tools.pause();
                break;
            }
        }
        public void Server_SetupMenu()
        {

            tools.clearscreen();
            tools.set_cursor_loc_org();

            while (true)
            {
                tools.print("功能設定區,功能尚未完成,按任意鍵繼續", 1, 1);
                tools.pause();
                break;
            }
        }
        public void Server_ListFavoriteBoards()
        {

            tools.clearscreen();
            tools.set_cursor_loc_org();

            while (true)
            {
                tools.print("我的最愛看板,功能尚未完成,按任意鍵繼續", 1, 1);
                tools.pause();
                break;
            }
        }
        public void Server_ListBoards()
        {

            int index = 4;

        begin:
            tools.clearscreen();
            tools.set_cursor_loc_org();

            string basedir = Directory.GetCurrentDirectory() + "/Boards/";
            tools.setColor(ConsoleColor.White, ConsoleColor.Blue);
            tools.print("[看板列表]                                                                    ", 1, 1);
            tools.reset_color();

            tools.setColor(ConsoleColor.White, ConsoleColor.Yellow);
            tools.print("[←,e]主選單 [→,r,enter]閱讀 [↑,j ↓,k]選擇 [PgUp][PgDn]翻頁 [/]搜尋 [h]求助", 2, 1);

            tools.setColor(ConsoleColor.Blue, ConsoleColor.White);
            tools.print("  編號   看板         類別   中文敘述                        板主             ", 3, 1);
            tools.reset_color();

            //

            tools.setForegroundColor(ConsoleColor.White);
            List<string> dirs = Directory.GetDirectories(basedir).ToList();

            int th = 4;
            foreach (string i in dirs)
            {

                string dir = i.Remove(0, basedir.Length);
                string comment = tools.get_file_text(i + "/comment.inf");
                tools.print((th - 3).ToString().PadLeft(4, ' '), th, 3);
                tools.print(dir, th, 10);
                tools.print(comment, th, 30);
                th++;
            }

            tools.setForegroundColor(ConsoleColor.Green);
            tools.print("◇", index, 1);

            //
            while (true)
            {
                clinet_keyinput ck = tools.get_input_op();
                if (ck.SKeyTyep == SpecialKeyType.Left || ck.key == 'e')
                    break;
                else if (ck.SKeyTyep == SpecialKeyType.Right || ck.key == 'r' || ck.key == 0x0d)
                {
                    Server_ListArticles();
                    goto begin;
                }
                else if (ck.SKeyTyep == SpecialKeyType.Down || ck.key == 'k')
                {
                    if (index < 8)
                    {
                        index++;
                        tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                        tools.setForegroundColor(ConsoleColor.Green);
                        tools.print("◇", index, 1);
                    }
                }
                else if (ck.SKeyTyep == SpecialKeyType.Up || ck.key == 'j')
                {
                    if (index > 4)
                    {
                        index--;
                        tcc.GetStream().Write(new byte[] { (byte)'\b', (byte)'\b', (byte)' ', (byte)' ' }, 0, 4);
                        tools.setForegroundColor(ConsoleColor.Green);
                        tools.print("◇", index, 1);
                    }
                }
                else if (ck.key == 'h')
                {
                    tools.print("尚未完成的功能,按任意鍵繼續..", 24, 1);
                    tools.pause();
                    tools.set_cursor_loc(24, 1);
                    tools.clear_cursor2LineEnd();
                    tools.set_cursor_loc(index, 1);
                }
                else if (ck.key == '/')
                {
                    tools.print("尚未完成的功能,按任意鍵繼續..", 24, 1);
                    tools.pause();
                    tools.set_cursor_loc(24, 1);
                    tools.clear_cursor2LineEnd();
                    tools.set_cursor_loc(index, 1);
                }
                else if (ck.SKeyTyep == SpecialKeyType.PageDown)
                {
                    tools.print("尚未完成的功能,按任意鍵繼續..", 24, 1);
                    tools.pause();
                    tools.set_cursor_loc(24, 1);
                    tools.clear_cursor2LineEnd();
                    tools.set_cursor_loc(index, 1);
                }
                else if (ck.SKeyTyep == SpecialKeyType.PageUp)
                {
                    tools.print("尚未完成的功能,按任意鍵繼續..", 24, 1);
                    tools.pause();
                    tools.set_cursor_loc(24, 1);
                    tools.clear_cursor2LineEnd();
                    tools.set_cursor_loc(index, 1);
                }

            }
        }
        public void Server_ListArticles()
        {
            tools.clearscreen();
            tools.set_cursor_loc_org();

            tools.setColor(ConsoleColor.White, ConsoleColor.Blue);
            tools.print("文章列表                                                                       ", 1, 1);
            tools.setColor(ConsoleColor.White, ConsoleColor.Black );
            tools.print("[←]離開 [→]閱讀 [Ctrl-P]發表文章 [d]刪除 [z]精華區 [i]看板資訊/設定 [h]說明  ", 2, 1);
            tools.setColor(ConsoleColor.White, ConsoleColor.Green );
            tools.print("   編號    日 期 作  者       文  章  標  題                         人氣:331  ", 3, 1);
            tools.reset_color();



            tools.print("功能編輯中..按任意鍵離開...", 24, 1);
            tools.pause();
        }
        public void start()
        {
            try
            {
                ///////
                tools.ask_no_clinet_echo();
                tools.reset_color();
                tools.set_cursor_loc_org();
                tools.clearscreen();

                bool check_ok = false;
                while (tcc.Connected)
                {
                    tools.set_cursor_loc_org();
                    tools.printfile(@"/etc/wellcome.ansi");
                    tools.print("歡迎來到 erspicu_brox 的 bbs ", 14, 0);
                    tools.print("(或以guest參觀,new註冊帳號)", 17, 0);
                    tools.print("請輸入登入帳號 : ", 16, 0);

                    tools.print_inputbox(12, ConsoleColor.Red, ConsoleColor.White);

                    string loginid = tools.get_input_line(10);

                    tools.reset_color();

                    if (loginid == "new")
                    {
                        registration();
                        check_ok = true;
                    }
                    else if (loginid == "guest")
                    {
                        clinet_profile = new clinet_datastruct();
                        clinet_profile.id = "guest";
                        clinet_profile.passwd = "";
                        clinet_profile.nickman = "我是訪客";
                        clinet_profile.mail = "";
                        check_ok = true;
                    }
                    else
                    {
                        check_ok = login_check(loginid);
                    }

                    //最後進入主選單
                    if (check_ok == true)
                    {
                        Console.WriteLine(clinet_profile.id + " (" + tcc.GetHashCode().ToString() + ") " + "登入");
                        system_main_menu();
                    }
                }
                Console.WriteLine("Clinet已登出.");
            }
            catch
            {
                Console.WriteLine("Clinet(" + tcc.GetHashCode().ToString() + ") 已關閉連結");
            }

        }
    }
}
