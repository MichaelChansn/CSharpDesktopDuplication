using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ControlServer1._0.DataPacket
{
    class ENUMS
    {
        #region
        /*****************************************************************************************
              鼠标控制信息：
            MOUSE+LEFT_CLICK+OK+1         单击左键
            MOUSE+RIGHT_CLICK+OK+2        单击右键
            MOUSE+LEFT_DOUBLECLICK+OK+3   双击左键
            MOUSE+RIGHT_DOUBLECLICK+OK+4  双击右键
            MOUSE+LEFT_DOWN+OK+5          左键按下
            MOUSE+LEFT_UP+OK+6            左键抬起
            MOUSE+RIGHT_DOWN+OK+7         右键按下
            MOUSE+RIGHT_UP+OK+8           右键抬起
            MOUSE+MOVE+X+Y                鼠标移动XY是相对移动的坐标长度可正可负
            MOUSE+WHEEL+DISTANCE+9        鼠标滚轮滚动DISTANCE是滚动距离正负表示前后滚动

            文字输入信息：
            KEY+VALUE+OK+DONE             得到键盘按键值VALUE，用于输入

            特殊键信息：
            SPECIAL+ENTER_DOWN+OK+DONE          Enter
            SPECIAL+ENTER_UP+OK+DONE

            SPECIAL+BACKSPACE_DOWN+OK+DONE      backspace
            SPECIAL+BACKSPACE_UP+OK+DONE

            SPECIAL+SPACE_DOWN+OK+DONE          SPACE
            SPECIAL+SPACE_UP+OK+DONE

            SPECIAL+ESC_DOWN+OK+DONE            ESC
            SPECIAL+ESC_UP+OK+DONE

            SPECIAL+SHIFT_DOWN+OK+DONE          Shift
            SPECIAL+SHIFT_UP+OK+DONE

            SPECIAL+CTRL_DOWN+OK+DONE           Ctrl
            SPECIAL+CTRL_UP+OK+DONE

            SPECIAL+ALT_DOWN+OK+DONE            Alt
            SPECIAL+ALT_UP+OK+DONE 

            SPECIAL+TAB_DOWN+OK+DONE            Tab
            SPECIAL+TAB_UP+OK+DONE

            SPECIAL+WIN_DOWN+OK+DONE            windows
            SPECIAL+WIN_UP+OK+DONE 

            SPECIAL+F1_DOWN+OK+DONE             F1
            SPECIAL+F1_UP+OK+DONE
            ...到
            SPECIAL+F12_DOWN+OK+DONE            F12
            SPECIAL+F12_UP+OK+DONE

            SPECIAL+END_DOWN+OK+DONE            End
            SPECIAL+END_UP+OK+DONE 

            SPECIAL+HOME_DOWN+OK+DONE           home
            SPECIAL+HOME_UP+OK+DONE 

            SPECIAL+DELETE_DOWN+OK+DONE         delete
            SPECIAL+DELETE_UP+OK+DONE

            SPECIAL+PRTSC_DOWN+OK+DONE          PrtSc
            SPECIAL+PRTSC_UP+OK+DONE 

            SPECIAL+INTERT_DOWN+OK+DONE         intert
            SPECIAL+INTERT_UP+OK+DONE

            SPECIAL+NUMLOCK_DOWN+OK+DONE        numlock
            SPECIAL+NUMLOCK_UP+OK+DONE

            SPECIAL+PAGEUP_DOWN+OK+DONE         PageUp
            SPECIAL+PAGEUP_UP+OK+DONE 

            SPECIAL+PAGEDOWN_DOWN+OK+DONE       PageDown
            SPECIAL+PAGEDOWN_UP+OK+DONE

            SPECIAL+UPKEY_DOWN+OK+DONE          Up
            SPECIAL+UPKEY_UP+OK+DONE 

            SPECIAL+DOWNKEY_DOWN+OK+DONE        Down
            SPECIAL+DOWNKEY_UP+OK+DONE 

            SPECIAL+LEFTKEY_DOWN+OK+DONE        Left
            SPECIAL+LEFTKEY_UP+OK+DONE 

            SPECIAL+RIGHTKEY_DOWN+OK+DONE       right
            SPECIAL+RIGHTKEY_UP+OK+DONE

            SPECIAL+CAPSLOCK_DOWN+OK+DONE       CapsLock
            SPECIAL+CAPSLOCK_UP+OK+DONE 
 

            功能控制：
            FUN+SHUTDOWN+OK+1             关机
            FUN+RESTSRRT+OK+2             重启
            FUN+MANGER+OK+3               任务管理
            FUN+SLEEP+OK+4                待机
            FUN+LOGOUT+OK+5               注销
            FUN+LOCK+OK+6                 锁定计算机
            FUN+SHUTDOWNTIME+VALUE+OK      在VALUE秒后关机
            FUN+SHUTDOWNCANCEL+OK+7         取消关机


            游戏手柄控制信息：
            GAME+UP_DOWN+OK+1             上，按下
            GAME+UP_UP+OK+2               上，抬起
            GAME+DOWN_DOWN+OK+3           下，按下
            GAME+DOWN_UP+OK+4             下，抬起
            GAME+LEFT_DOWN+OK+5           左，按下
            GAME+LEFT_UP+OK+6             左，抬起
            GAME+RIGHT_DOWN+OK+7          右，按下
            GAME+RIGHT_UP+OK+8            右，抬起

            GAME+A_DOWN+OK+9              A，按下
            GAME+A_UP+OK+10               A，抬起
            GAME+B_DOWN+OK+11             B，按下
            GAME+B_UP+OK+12               B，抬起
            GAME+C_DOWN+OK+13             C，按下
            GAME+C_UP+OK+14               C，抬起
            GAME+D_DOWN+OK+15             D，按下
            GAME+D_UP+OK+16               D，抬起

            GAME+START+OK+17              开始键
            GAME+STOP+OK+18               停止键

            GAME+OTHER1+OK+19             其他功能键
            GAME+OTHER2+OK+20
            GAME+OTHER3+OK+21
            GAME+OTHER4+OK+22

        退出信息：
         EXIT+OFF+OK+OUT               退出连接
         图片传送：
         PICTURE+START+OK+DONE       开始
         PICTURE+STOP+OK+DONE       停止

        *****************************************************************************************/

        public enum  MESSAGETYPE : byte
        {
            /**connection infos*/
            HOST_NANME = (byte)0x00,//client host name
            CODE = (byte)0x01,// credentials
            EXIT = (byte)0x02,//exit connection

            /**screen image control*/
            START_PIC = (byte)0x10,//start send picture
            STOP_PIC = (byte)0x11,//stop send picture

            /**key press infos*/
            KEY_DOWN = (byte)0x20,
            KEY_UP = (byte)0x21,



            /**mouse control*/
            MOUSE_LEFT_DOWN = (byte)0x30,
            MOUSE_LEFT_UP = (byte)0x31,
            MOUSE_RIGHT_DOWN = (byte)0x32,
            MOUSE_RIGHT_UP = (byte)0x33,
            MOUSE_LEFT_CLICK = (byte)0x34,
            MOUSE_LEFT_DOUBLE_CLICK = (byte)0x35,
            MOUSE_RIGHT_CLICK = (byte)0x36,
            MOUSE_RIGHT_DOUBLE_CLICK = (byte)0x37,
            MOUSE_WHEEL = (byte)0x38,
            MOUSE_MOVE = (byte)0x39,

            /**game control*/
            GAME_UP_UP = (byte)0x40,
            GAME_UP_DOWN = (byte)0x41,
            GAME_DOWN_UP = (byte)0x42,
            GAME_DOWN_DOWN = (byte)0x43,
            GAME_LEFT_UP = (byte)0x44,
            GAME_LEFT_DOWN = (byte)0x45,
            GAME_RIGHT_UP = (byte)0x46,
            GAME_RIGHT_DOWN = (byte)0x47,

            GAME_A_UP = (byte)0x48,
            GAME_A_DOWN = (byte)0x49,
            GAME_B_UP = (byte)0x4A,
            GAME_B_DOWN = (byte)0x4B,
            GAME_C_UP = (byte)0x4C,
            GAME_C_DOWN = (byte)0x4D,
            GAME_D_UP = (byte)0x4E,
            GAME_D_DOWN = (byte)0x4F,

            GAME_START_UP = (byte)0x50,
            GAME_START_DOWN = (byte)0x51,
            GAME_STOP_UP = (byte)0x52,
            GAME_STOP_DOWN = (byte)0x53,

            GAME_OTHER1_UP = (byte)0x54,
            GAME_OTHER1_DOWN = (byte)0x55,
            GAME_OTHER2_UP = (byte)0x56,
            GAME_OTHER2_DOWN = (byte)0x57,
            GAME_OTHER3_UP = (byte)0x58,
            GAME_OTHER3_DOWN = (byte)0x59,
            GAME_OTHER4_UP = (byte)0x5A,
            GAME_OTHER4_DOWN = (byte)0x5B,

            /**control PC funs*/
            FUN_SHUTDOWN = (byte)0x60,
            FUN_RESTART = (byte)0x61,
            FUN_MANAGER = (byte)0x62,
            FUN_SLEEP = (byte)0x63,
            FUN_LOGOUT = (byte)0x64,
            FUN_LOCK = (byte)0x65,
            FUN_SHUTDOWN_TIME = (byte)0x66,
            FUN_SHUTDOWN_CANCEL = (byte)0x67,
            FUN_SHOW_DESKTOP = (byte)0x68,

            /**send text to PC*/
            TEXT = (byte)0x70,

        }

        /*special keys on keyboard*/
        public enum SPECIALKEYS : byte
        {
            ENTER = (byte)0x00,
            BACKSPACE = (byte)0x01,
            SPACE = (byte)0x02,
            ESC = (byte)0x03,
            SHIFT = (byte)0x04,
            CTRL = (byte)0x05,
            ALT = (byte)0x06,
            TAB = (byte)0x07,
            WIN = (byte)0x08,

            F1 = (byte)0x09,
            F2 = (byte)0x0A,
            F3 = (byte)0x0B,
            F4 = (byte)0x0C,
            F5 = (byte)0x0D,
            F6 = (byte)0x0E,
            F7 = (byte)0x0F,
            F8 = (byte)0x10,
            F9 = (byte)0x11,
            F10 = (byte)0x12,
            F11 = (byte)0x13,
            F12 = (byte)0x14,

            END = (byte)0x15,
            HOME = (byte)0x16,
            DEL = (byte)0x17,
            PRTSC = (byte)0x18,
            INSERT = (byte)0x19,
            NUMLOCK = (byte)0x1A,
            PAGEUP = (byte)0x1B,
            PAGEDOWN = (byte)0x1C,

            ARROW_UP = (byte)0x1D,
            ARROW_DOWN = (byte)0x1E,
            ARROW_LEFT = (byte)0x1F,
            ARROW_RIGHT = (byte)0x20,

            CAPSLOCK = (byte)0x21,
        }


        public const String UDPSCANMESSAGE = "在不在啊？请回答。。。";
        public const Char NETSEPARATOR = '+';
        #endregion
    }
}
