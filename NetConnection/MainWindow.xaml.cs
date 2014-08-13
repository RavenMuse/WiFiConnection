
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WiFiConnection.Properties;
using WiFiConnection.Resources;
using WiFiHost;
using ShutDownTimer;


namespace WiFiConnection
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>    
    public partial class MainWindow : Window
    {

        WifiHost wifiHost;
        ShutdownTimer shutdownTimer;
        TimeSpan shutdownTimeSpan;
        DateTime shutdownTime;
        private List<SharingConnection> sharingConnections;

        //状态栏倒计时刷新线程
        private Thread stateRefreshThread;

        
        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        
        public MainWindow()
        {
            InitializeComponent();                   
        }


        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            Title.Text = AppResource.ApplicationName + AppResource.Version;
            Txt_UpdateInstruction.Text = AppResource.UpdateInstruction;
            Txt_UpdateInstruction.IsReadOnly = true;
            
            S_ShareState.Text = AppResource.Unshared;
            S_ShutdownTime.Text = AppResource.ShutdownTimerState;
            B_WifiSetup.Content = AppResource.ShareWifi;
            B_ShutdownSetup.Content = AppResource.SetShutdown;
            
            
            if (Settings.Default.FirstLaunch)
            {                
                Settings.Default.FirstLaunch = false;
                Settings.Default.Save();
            }

            //用户密码读取
            Txt_Ssid.Text = Settings.Default.SSID;
            PswBox.Password = Settings.Default.Password;


            shutdownTimer = new ShutdownTimer();
            Txt_ShutDownHours.Text = Settings.Default.ShutdownTimeSpan.Hours.ToString();
            Txt_ShutDownMinutes.Text = Settings.Default.ShutdownTimeSpan.Minutes.ToString();
            

            wifiHost = new WifiHost();
            sharingConnections = wifiHost.GetSharingConnections();

            RefreshShareConnection();

            foreach (SharingConnection connection in Cb_ShareConnections.Items)
            {
                if (connection.Name == Settings.Default.ShareConnection)
                {
                    
                    Cb_ShareConnections.SelectedItem = connection;
                    break;
                }
            }
            
            InitialTray();

            

            //软件开启时自动设定
            if (Settings.Default.AutoSet)
            {
                SetWifi();
                SetShutdownTimer();
                this.WindowState = WindowState.Minimized;
            }
        }
        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.SSID = Txt_Ssid.Text;
            Settings.Default.Password = PswBox.Password;
            Settings.Default.ShareConnection = Cb_ShareConnections.Text;
            Settings.Default.ShutdownTimeSpan = shutdownTimeSpan;
            Settings.Default.Save();

            wifiHost.Stop();
            shutdownTimer.Stop();
        }
        
        

        /// <summary>
        /// WiFihost设定与取消逻辑控制
        /// </summary>
        private void SetWifi()
        {
            if (wifiHost.isHost == false)
            {
                try
                {
                    B_WifiSetup.Content = AppResource.UnShareWifi;
                    Txt_Ssid.IsEnabled = false;
                    PswBox.IsEnabled = false;
                    Cb_ShareConnections.IsEnabled = false;
                    S_ShareState.Text = AppResource.Shared;
                    wifiHost.Init(Txt_Ssid.Text, PswBox.Password, 20, (SharingConnection)Cb_ShareConnections.SelectedItem);
                    wifiHost.Start();
                    notifyIcon.Icon = AppResource.ShareStarted;
                    notifyBalloonTip(AppResource.ShareWifiSuccessInfo);
                }
                catch (Exception ex)
                {
                    notifyBalloonTip(AppResource.ShareWififailedInfo);
                    notifyBalloonTip(ex.ToString());
                }
            }
            else
            {
                B_WifiSetup.Content = AppResource.ShareWifi;
                Txt_Ssid.IsEnabled = true;
                PswBox.IsEnabled = true;
                Cb_ShareConnections.IsEnabled = true;
                S_ShareState.Text = AppResource.Unshared;
                wifiHost.Stop();
                notifyIcon.Icon = AppResource.ShareStopped;
                notifyBalloonTip(AppResource.ShareWifiCancelInfo);

            }

        }
        /// <summary>
        /// ShutdownTimer设定与取消逻辑控制
        /// </summary>
        private void SetShutdownTimer()
        {
            if (shutdownTimer.enable == false)
            {
                try
                {
                    shutdownTimeSpan = ShutdownTimer.TimeParse(Txt_ShutDownHours.Text, Txt_ShutDownMinutes.Text);
                    shutdownTime = DateTime.Now.Add(shutdownTimeSpan);
                    shutdownTimer.Start(shutdownTimeSpan);
                    Txt_ShutDownHours.IsEnabled = false;
                    Txt_ShutDownMinutes.IsEnabled = false;
                    B_ShutdownSetup.Content = AppResource.CancelShutdown;
                    notifyBalloonTip(AppResource.ShutdownInfo + shutdownTime.ToString());

                    //状态栏的倒计时监控线程
                    stateRefreshThread = new Thread(new ThreadStart(StartRefresh));
                    stateRefreshThread.IsBackground = true;
                    srh = StartRefreshDelegate;
                    stateRefreshThread.Start();
                    
                    
                }
                catch (Exception ex)
                {
                    notifyBalloonTip(ex.ToString());
                }
            }
            else
            {   
                shutdownTimer.Stop();
                stateRefreshThread.Abort();
                Txt_ShutDownHours.IsEnabled = true;
                Txt_ShutDownMinutes.IsEnabled = true;
                B_ShutdownSetup.Content = AppResource.SetShutdown;
                S_ShutdownTime.Text = AppResource.ShutdownTimerState;
                notifyBalloonTip(AppResource.ShutdownCancelInfo);
                

            }
        }

        delegate void StateRefreshHandler();
        StateRefreshHandler srh;
        private void StartRefresh()
        {
            while (true)
            {
                Thread.Sleep(1000);
                S_ShutdownTime.Dispatcher.Invoke(srh);
            }
        }
        private void StartRefreshDelegate()
        {
            S_ShutdownTime.Text = shutdownTimer.shutdownTimeSpan.ToString();
        }


        /// <summary>
        /// Cb_ShareConnection更新
        /// </summary>
        private void RefreshShareConnection()
        {
            Cb_ShareConnections.Items.Clear();

            foreach (var connection in sharingConnections)
            {
                if (connection.IsConnected && connection.IsSupported)
                {
                    Cb_ShareConnections.Items.Add(connection);
                }
            }

            if (Cb_ShareConnections.Items.Count > 0)
                Cb_ShareConnections.SelectedIndex = 0;
        }

        private void B_ShareConnectionRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshShareConnection();
        }

        private void B_WifiSetup_Click(object sender, RoutedEventArgs e)
        {
            SetWifi();
        }
        private void B_ShutdownSetup_Click(object sender, RoutedEventArgs e)
        {
            SetShutdownTimer();        
        }
        
        #region Ssid和Password
        private void Txt_Ssid_TextChanged(object sender, TextChangedEventArgs e)
        {
            Txt_Ssid.Background = Brushes.LightBlue;
        }
        private void PswBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PswBox.Password.Length < 8)
            {
                PswBox.Background = Brushes.Red;
            }
            else
            {
                PswBox.Background = Brushes.LightBlue;
            }
        }
        #endregion


        #region notifyBalloonTip初始化
        private void InitialTray()
        {
            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.BalloonTipText = AppResource.ApplicationName;
            notifyIcon.ShowBalloonTip(1000);
            notifyIcon.Text = AppResource.ApplicationName;
            notifyIcon.Icon = AppResource.ShareStopped;
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);
            //设置菜单项
            //System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem("菜单项1");
            //System.Windows.Forms.MenuItem menu2 = new System.Windows.Forms.MenuItem("菜单项2");
            //System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("菜单", new System.Windows.Forms.MenuItem[] { menu1, menu2 });

            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);



            System.Windows.Forms.MenuItem AutoSet = new System.Windows.Forms.MenuItem("自动搭建");
            AutoSet.Click += AutoSet_Click;
            if (Settings.Default.AutoSet)
            {
                AutoSet.Checked = true;
            }
            else
            {
                AutoSet.Checked = false;
            }

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { exit, AutoSet };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }

        private void AutoSet_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem o = (System.Windows.Forms.MenuItem)sender;
            Settings.Default.AutoSet = !Settings.Default.AutoSet;
            o.Checked = !o.Checked;
        }

        private void notifyBalloonTip(string s)
        {
            notifyIcon.BalloonTipText = s;
            notifyIcon.ShowBalloonTip(1000);
        }
        #endregion



        #region 托盘exit事件
        private void exit_Click(object sender, EventArgs e)
        {
            MessageBoxResult Result = MessageBox.Show("确定要关闭吗?", "退出", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (Result == MessageBoxResult.Yes)
            {   
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region 鼠标单击托盘图标
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    this.Visibility = Visibility.Visible;                    
                    this.Activate();
                    this.Show();
                }
            }
        }
        #endregion

        #region X按钮UI控制
        private void Exit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            notifyBalloonTip(AppResource.BackgroundRuningInfo);            
            this.WindowState = WindowState.Minimized;

        }
        private void Exit_MouseMove(object sender, MouseEventArgs e)
        {
            Exit.Foreground = Brushes.LightBlue;
        }

        private void Exit_MouseLeave(object sender, MouseEventArgs e)
        {
            Exit.Foreground = Brushes.Red;
        }
        #endregion        
        
        #region 窗口最小化时隐藏窗口
        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Hidden;
            }
        }
        #endregion
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
