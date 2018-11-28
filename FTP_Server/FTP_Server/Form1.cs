using mooftpserv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
namespace FTP_Server
{
    public partial class Form1 : Form
    {
        public static Form1 Form1this;
        private bool verbose = false;
        private bool anyPeer = false;
        private int port = -1;
        private int buffer = -1;
        public string IP = "";
        public int PORT = 0;
        private string FILE = "/C/";
        private Server srv = new Server();
        private _Socket sck_server = new _Socket();
        private bool _Server_Start = false;
        public Form1()
        {
            InitializeComponent();
            Form1this = this;
        }
        public bool Server_Start{
            get { return _Server_Start; }
            set { _Server_Start = value; }
        }
        private void button1_Click(object sender, EventArgs e){
            srv.LogHandler = new DefaultLogHandler(verbose);
            srv.AuthHandler = new DefaultAuthHandler(anyPeer);

            if (port != -1)
                srv.LocalPort = port;

            if (buffer != -1)
                srv.BufferSize = buffer * 1024; // in KB

            //setting
            string str = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            List<string> setting = new List<string>();
            StreamReader sr = new StreamReader(str + @"\setting.txt");
            while (!sr.EndOfStream) setting.Add(sr.ReadLine());
            sr.Close();
            //srv.LocalEndPoint = new IPEndPoint(IPAddress.Parse("192.168.12.44"), 21);
            IP = setting_parse(setting[0]);
            PORT = int.Parse(setting_parse(setting[1]));
            FILE = setting_parse(setting[2]);
            srv.LocalEndPoint = new IPEndPoint(IPAddress.Parse(IP), PORT);
            sck_server.LocalIP = IP;
            mooftpserv.DefaultFileSystemHandler file = new DefaultFileSystemHandler(FILE);
            srv.FileSystemHandler = file;
            //
            
            try{
                Thread server = new Thread(srv.Run);
                server.Start();
                Thread SockerServer = new Thread(sck_server.Listen);
                SockerServer.Start();
            }
            catch (Exception ex){
                MessageBox.Show(ex.Message);
            }
            //等待連線
            Thread.Sleep(20);
            if(Server_Start == true){
                lb_status.Text = "Server start in " + srv.LocalEndPoint;
                btn_Close.Enabled = true;
                btn_Start.Enabled = false;
            }
        }

        /// <summary>
        /// split setting text
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        private static string setting_parse(string set)
        {
            string[] split_set = set.Split('=');
            return split_set[1];
        }

        delegate void MyDelegate(IPEndPoint peer, string format, params object[] args);
        public void Write_Log(IPEndPoint peer, string format, params object[] args){
            string now = DateTime.Now.ToString("HH:mm:ss.fff");
            //以委派進行不同執行序存取
            if (this.InvokeRequired){
                MyDelegate MD = new MyDelegate(Write_Log);
                this.Invoke(MD, peer, format, args);
            }
            else{
                list_log.Items.Add(now + " , " + peer + " : " + String.Format(format, args));
                list_log.SelectedIndex = list_log.Items.Count - 1;
            }
        }

        delegate void CloseDelegate(object sender, EventArgs e);
        public void btn_Close_Click(object sender, EventArgs e){
            //以委派進行不同執行序存取
            if (this.InvokeRequired){
                CloseDelegate CD = new CloseDelegate(btn_Close_Click);
                this.Invoke(CD, sender, e);
            }
            else{
                srv.Stop();
                sck_server.Stop();
                lb_status.Text = "Server close";
                btn_Close.Enabled = false;
                btn_Start.Enabled = true;
            }
        }
    }
}
