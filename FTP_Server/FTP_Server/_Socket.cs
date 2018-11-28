﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FTP_Server
{
    class _Socket
    {
        Socket[] SckSs;   // 一般而言 Server 端都會設計成可以多人同時連線. 
        int SckCIndex;
        public string LocalIP = "";
        int SPort = 8181;
        int RDataLen = 15;
        // 聆聽
        public void Listen()
        {
            // 用 Resize 的方式動態增加 Socket 的數目
            Array.Resize(ref SckSs, 1);
            SckSs[0] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SckSs[0].Bind(new IPEndPoint(IPAddress.Parse(LocalIP), SPort));
            SckSs[0].Listen(10); // 進行聆聽; Listen( )為允許 Client 同時連線的最大數
            SckSWaitAccept();   // 另外寫一個函數用來分配 Client 端的 Socket
        }
        // 關閉
        public void Stop()
        {
            foreach (Socket sck in SckSs) if(sck != null)sck.Close();
        }
        // 等待Client連線
        private void SckSWaitAccept()
        {
            // 判斷目前是否有空的 Socket 可以提供給Client端連線
            bool FlagFinded = false;
            for (int i = 1; i < SckSs.Length; i++)
            {
                // SckSs[i] 若不為 null 表示已被實作過, 判斷是否有 Client 端連線
                if (SckSs[i] != null)
                {
                    // 如果目前第 i 個 Socket 若沒有人連線, 便可提供給下一個 Client 進行連線
                    if (SckSs[i].Connected == false)
                    {
                        SckCIndex = i;
                        FlagFinded = true;
                        break;
                    }
                }
            }
            // 如果 FlagFinded 為 false 表示目前並沒有多餘的 Socket 可供 Client 連線
            if (FlagFinded == false)
            {
                // 增加 Socket 的數目以供下一個 Client 端進行連線
                SckCIndex = SckSs.Length;
                Array.Resize(ref SckSs, SckCIndex + 1);
            }
            // 以下兩行為多執行緒的寫法, 因為接下來 Server 端的部份要使用 Accept() 讓 Cleint 進行連線;
            // 該執行緒有需要時再產生即可, 因此定義為區域性的 Thread. 命名為 SckSAcceptTd;
            // 在 new Thread( ) 裡為要多執行緒去執行的函數. 這裡命名為 SckSAcceptProc;
            Thread SckSAcceptTd = new Thread(SckSAcceptProc);
            SckSAcceptTd.Start();  // 開始執行 SckSAcceptTd 這個執行緒
            // 這裡要點出 SckSacceptTd 這個執行緒會在 Start( ) 之後開始執行 SckSAcceptProc 裡的程式碼, 同時主程式的執行緒也會繼續往下執行各做各的. 
            // 主程式不用等到 SckSAcceptProc 的程式碼執行完便會繼續往下執行.
        }// 接收來自Client的連線與Client傳來的資料
        private void SckSAcceptProc()
        {
            // 這裡加入 try 是因為 SckSs[0] 若被 Close 的話, SckSs[0].Accept() 會產生錯誤
            try
            {
                SckSs[SckCIndex] = SckSs[0].Accept();  // 等待Client 端連線
                // 為什麼 Accept 部份要用多執行緒, 因為 SckSs[0] 會停在這一行程式碼直到有 Client 端連上線, 並分配給 SckSs[SckCIndex] 給 Client 連線之後程式才會繼續往下, 若是將 Accept 寫在主執行緒裡, 在沒有Client連上來之前, 主程式將會被hand在這一行無法再做任何事了!!
                // 能來這表示有 Client 連上線. 記錄該 Client 對應的 SckCInde
                int Scki = SckCIndex;
                // 再產生另一個執行緒等待下一個 Client 連線
                SckSWaitAccept();
                long IntAcceptData;
                byte[] clientData = new byte[RDataLen];  // 其中RDataLen為每次要接受來自 Client 傳來的資料長度
                while (true)
                {
                    // 程式會被 hand 在此, 等待接收來自 Client 端傳來的資料
                    IntAcceptData = SckSs[Scki].Receive(clientData);
                    // 往下就自己寫接收到來自Client端的資料後要做什麼事唄~^^”
                    // 因為Client端傳ABCDE過來, 所以可以試著將Byte陣列轉成字串列印出來看看~
                    string S = Encoding.Default.GetString(clientData);
                    if (S.Equals("\0\0\0\0\0\0\0\0\0\0")) continue;
                    string[] tem_s = S.Split('\0');
                    if (tem_s[0].Equals("stop")) Form1.Form1this.btn_Close_Click(null, null);
                }
            }
            catch
            {
                // 這裡若出錯主要是來自 SckSs[Scki] 出問題, 可能是自己 Close, 也可能是 Client 斷線, 自己加判斷吧~
            }
        }
        // Server 傳送資料給所有Client
        private void SckSSend(string send)
        {
            for (int Scki = 1; Scki < SckSs.Length; Scki++)
            {
                if (null != SckSs[Scki] && SckSs[Scki].Connected == true)
                {
                    try
                    {
                        SckSs[Scki].Send(Encoding.ASCII.GetBytes(send));
                    }
                    catch
                    {
                        // 這裡出錯, 主要是出在 SckSs[Scki] 出問題, 自己加判斷吧~
                    }
                }
            }
        }
    }
}
