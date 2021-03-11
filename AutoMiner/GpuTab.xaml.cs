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
    /// Logique d'interaction pour GpuTab.xaml
    /// </summary>
    public partial class GpuTab : UserControl
    {
        GPU gpu;
        public GpuTab(GPU gpu)
        {
            this.gpu = gpu;
            InitializeComponent();
            LblTitle.Content = gpu.name;
            TxtCoreClockMining.Text = (gpu.ocProfile.miningCoreClockBoost / 1000).ToString();
            TxtCoreClockNotMining.Text = (gpu.ocProfile.notMiningMemoryClockBoost / 1000).ToString();
            TxtMemoryClockMining.Text = (gpu.ocProfile.miningMemoryClockBoost / 1000).ToString();
            TxtMemoryClockNotMining.Text = (gpu.ocProfile.notMiningMemoryClockBoost / 1000).ToString();
            TxtPowerLimitMining.Text = gpu.ocProfile.miningPowerLimit.ToString();
            TxtPowerLimitNotMining.Text = gpu.ocProfile.notMiningPowerLimit.ToString();
        }

        private Boolean ValidateInputs()
        {
            string errorMessages = "";
            if (!isInteger(TxtCoreClockMining.Text))
            {
                errorMessages += "Le champ core clock mining n'est pas un entier";
            } else if (Int32.Parse(TxtCoreClockMining.Text) * 1000 > gpu.gpuInfo.coreClockBoost.max || Int32.Parse(TxtCoreClockMining.Text) * 1000 < gpu.gpuInfo.coreClockBoost.min)
            {
                errorMessages += "Le champ core clock mining doit être compris entre " + gpu.gpuInfo.coreClockBoost.max / 1000 + " et " + gpu.gpuInfo.coreClockBoost.min / 1000 + "\n";
            }
            if (!isInteger(TxtCoreClockNotMining.Text))
            {
                errorMessages += "Le champ core clock not mining n'est pas un entier";
            }
            else if (Int32.Parse(TxtCoreClockNotMining.Text) * 1000 > gpu.gpuInfo.coreClockBoost.max || Int32.Parse(TxtCoreClockNotMining.Text) * 1000 < gpu.gpuInfo.coreClockBoost.min)
            {
                errorMessages += "Le champ core clock not mining doit être compris entre " + gpu.gpuInfo.coreClockBoost.max / 1000 + " et " + gpu.gpuInfo.coreClockBoost.min / 1000 + "\n";
            }
            if (!isInteger(TxtMemoryClockMining.Text))
            {
                errorMessages += "Le champ memory clock mining n'est pas un entier";
            }
            else if (Int32.Parse(TxtMemoryClockMining.Text) * 1000 > gpu.gpuInfo.memoryClockBoost.max || Int32.Parse(TxtMemoryClockMining.Text) * 1000 < gpu.gpuInfo.memoryClockBoost.min)
            {
                errorMessages += "Le champ memory clock mining doit être compris entre " + gpu.gpuInfo.memoryClockBoost.max / 1000 + " et " + gpu.gpuInfo.memoryClockBoost.min / 1000 + "\n";
            }
            if (!isInteger(TxtMemoryClockNotMining.Text))
            {
                errorMessages += "Le champ memory clock not mining n'est pas un entier";
            }
            else if (Int32.Parse(TxtMemoryClockNotMining.Text) * 10000 > gpu.gpuInfo.memoryClockBoost.max || Int32.Parse(TxtMemoryClockNotMining.Text) * 1000 < gpu.gpuInfo.memoryClockBoost.min)
            {
                errorMessages += "Le champ memory clock not mining doit être compris entre " + gpu.gpuInfo.memoryClockBoost.max / 1000 + " et " + gpu.gpuInfo.memoryClockBoost.min / 1000 + "\n";
            }
            if (!isInteger(TxtPowerLimitMining.Text))
            {
                errorMessages += "Le champ power limit mining n'est pas un entier";
            }
            else if (Int32.Parse(TxtPowerLimitMining.Text) > gpu.gpuInfo.powerLimit.max || Int32.Parse(TxtPowerLimitMining.Text) < gpu.gpuInfo.powerLimit.min)
            {
                errorMessages += "Le champ power limit mining doit être compris entre " + gpu.gpuInfo.powerLimit.max + " et " + gpu.gpuInfo.powerLimit.min + "\n";
            }
            if (!isInteger(TxtPowerLimitNotMining.Text))
            {
                errorMessages += "Le champ power limit mining n'est pas un entier";
            }
            else if (Int32.Parse(TxtPowerLimitNotMining.Text) > gpu.gpuInfo.powerLimit.max || Int32.Parse(TxtPowerLimitNotMining.Text) < gpu.gpuInfo.powerLimit.min)
            {
                errorMessages += "Le champ power limit not mining doit être compris entre " + gpu.gpuInfo.powerLimit.max + " et " + gpu.gpuInfo.powerLimit.min + "\n";
            }
            if (errorMessages.Length > 0)
            {
                MessageBox.Show(errorMessages, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            } else
            {
                return true;
            }
        }

        private Boolean isInteger(string s)
        {
            try
            {
                Int32.Parse(s);
                return true;
            } catch (Exception e)
            {
                return false;
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (!this.ValidateInputs())
            {
                
                return;
            }
            try
            {
                this.gpu.ocProfile.miningCoreClockBoost = Int32.Parse(TxtCoreClockMining.Text) * 1000;
                this.gpu.ocProfile.notMiningCoreClockBoost = Int32.Parse(TxtCoreClockNotMining.Text) * 1000;
                this.gpu.ocProfile.miningMemoryClockBoost = Int32.Parse(TxtMemoryClockMining.Text) * 1000;
                this.gpu.ocProfile.notMiningMemoryClockBoost = Int32.Parse(TxtMemoryClockNotMining.Text) * 1000;
                this.gpu.ocProfile.miningPowerLimit = Int32.Parse(TxtPowerLimitMining.Text);
                this.gpu.ocProfile.notMiningPowerLimit = Int32.Parse(TxtPowerLimitNotMining.Text);
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
