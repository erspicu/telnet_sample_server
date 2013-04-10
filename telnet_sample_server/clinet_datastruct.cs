using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace telnet_sample_server
{
    public enum SpecialKeyType
    {
        NULL = -1,
        Up = 0 ,
        Down = 1,
        Left = 2,
        Right = 3 ,
        PageUp = 4 ,
        PageDown = 5
    }
    public class clinet_keyinput
    {
        public SpecialKeyType SKeyTyep = SpecialKeyType.NULL;
       // public List<byte> raw_bytes = new List<byte>();
        public char key = (char)0;
        public int count = 0;
    }
    public class clinet_datastruct
    {
        public string id = "";
        public string passwd = "";
        public string nickman = "";
        public string mail = "";
    }
}
