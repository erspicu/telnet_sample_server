using System;
namespace telnet_sample_server
{
    interface Iclinet_thread
    {
        void get_system_info();
        void registration();
        void start();
        void system_main_menu();
    }
}
