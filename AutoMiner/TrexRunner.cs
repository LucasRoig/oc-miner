using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace AutoMiner
{
    class TrexRunner
    {
        Process trexProcess;
        Thread outputReader;
        Thread errorReader;
        public void Start()
        {
            if (this.trexProcess == null)
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(ConfigService.appConfig.trexConfig.execPath, 
                    String.Format("-a ethash -o {0} -u {1} -w {2}", ConfigService.appConfig.trexConfig.pool, ConfigService.appConfig.trexConfig.wallet, ConfigService.appConfig.trexConfig.rigName));
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                trexProcess = Process.Start(processStartInfo);
                Trace.WriteLine("Process " + processStartInfo.FileName + " started, pid " + trexProcess.Id);
                

                Thread.Sleep(2000);
                outputReader = new Thread(() =>
                {
                    while(trexProcess != null && !trexProcess.HasExited)
                    {
                        String line = trexProcess.StandardOutput.ReadLine() + "\n";
                        LogService.addLine(line);
                        Trace.Write(line);
                    }
                });
                outputReader.Start();
                errorReader = new Thread(() =>
                {
                    while (trexProcess != null && !trexProcess.HasExited)
                    {
                        String line = trexProcess.StandardError.ReadLine() + "\n";
                        LogService.addLine(line);
                        Trace.Write(line);

                    }
                });
                errorReader.Start();
            }
        }

        public void Stop()
        {
            if (trexProcess != null)
            {
                trexProcess.Kill();
                trexProcess.WaitForExit();
                trexProcess = null;
            }
        }
    }
}
