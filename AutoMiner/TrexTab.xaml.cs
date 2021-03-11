using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace AutoMiner
{
    /// <summary>
    /// Logique d'interaction pour TrexTab.xaml
    /// </summary>
    public partial class TrexTab : UserControl
    {
        public TrexTab()
        {
            InitializeComponent();
            txtExecPath.Text = ConfigService.appConfig.trexConfig.execPath;
            txtPool.Text = ConfigService.appConfig.trexConfig.pool;
            txtRig.Text = ConfigService.appConfig.trexConfig.rigName;
            txtWallet.Text = ConfigService.appConfig.trexConfig.wallet;
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfigService.appConfig.trexConfig.execPath = txtExecPath.Text.Trim();
                ConfigService.appConfig.trexConfig.pool = txtPool.Text.Trim();
                ConfigService.appConfig.trexConfig.rigName = txtRig.Text.Trim();
                ConfigService.appConfig.trexConfig.wallet = txtWallet.Text.Trim();
                ConfigService.saveConfig();
                MessageBox.Show("Save done!", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception exception)
            {
                Trace.TraceError(exception.Message);
                MessageBox.Show("Error while saving config\n" + exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
