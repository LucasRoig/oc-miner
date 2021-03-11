using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace AutoMiner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// see https://forums.guru3d.com/threads/msi-afterburner-net-class-library.339656/
    public partial class MainWindow : Window
    {
        bool isRunning = false;
        GpuWatcherThread gpuWatcher;
        TrexRunner trexRunner = new TrexRunner();
        public MainWindow()
        {
            InitializeComponent();
            foreach (GPU gpu in GpuManager.GetGPUs()) {
                TabItem item = new TabItem { Header = gpu.name, Name = "Name" };
                item.Content = new GpuTab(gpu);
                TabControl.Items.Add(item);
            }
            LogListenerImpl logListener = new LogListenerImpl(TxtLogs, this.Dispatcher);
            LogService.addListener(logListener);

        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.isRunning)
            {
                this.StopMining();
                //e.Cancel = true;
            }
            //MessageBox.Show("Stop Minning before closing" , "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        Boolean ValidateTrexConfig()
        {
            Boolean isValid = true;
            if (ConfigService.appConfig.trexConfig.execPath.Trim().Length == 0)
            {
                isValid = false;
            }
            if (ConfigService.appConfig.trexConfig.pool.Trim().Length == 0)
            {
                isValid = false;
            }
            if (ConfigService.appConfig.trexConfig.wallet.Trim().Length == 0)
            {
                isValid = false;
            }
            if (ConfigService.appConfig.trexConfig.rigName.Trim().Length == 0)
            {
                isValid = false;
            }
            if (!isValid)
            {
                MessageBox.Show("Trex conf is not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!File.Exists(ConfigService.appConfig.trexConfig.execPath))
            {
                MessageBox.Show("Trex exec does not exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return isValid;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (isRunning)
            {
                this.StopMining();
            } else
            {
                if (!ValidateTrexConfig())
                {
                    return;
                }
                GpuManager.applyMiningOcParams();
                Thread.Sleep(2000);
                gpuWatcher = new GpuWatcherThread();
                gpuWatcher.Start();
                trexRunner.Start();
                isRunning = true;
                BtStart.Content = "Stop";
            }
        }

        private void StopMining()
        {
            if (isRunning)
            {
                gpuWatcher.Stop();
                gpuWatcher = null;
                GpuManager.applyNotMiningOcParams();
                Thread.Sleep(2000);
                trexRunner.Stop();
                isRunning = false;
                BtStart.Content = "Start";
            }
        }
    }

    

    class LogListenerImpl : LogListener
    {
        TextBox textBox;
        Dispatcher dispatcher;
        public LogListenerImpl(TextBox textBox, Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            this.textBox = textBox;
        }
        public void addLine(string s)
        {
            try
            {
                this.dispatcher.Invoke(() =>
                {
                    textBox.AppendText(s);
                    textBox.ScrollToEnd();
                });
            } catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
        }

        public void removeLine()
        {
            try
            {
                this.dispatcher.Invoke(() =>
                {
                    int index = textBox.Text.IndexOf('\n');
                    if (index > 0)
                    {
                        textBox.Text = textBox.Text.Substring(index + 1);
                    }
                });
            } catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            
            
        }
    }
}
