using System.Diagnostics;
using UnityEngine;

namespace BaseFramework
{
    public class CommandUtil
    {
        public class CommandResult
        {
            public int resultCode;
            public string output;
            public string error;

            public CommandResult(int resultCode, string output, string error)
            {
                this.resultCode = resultCode;
                this.output = output;
                this.error = error;
            }
        }

        public static CommandResult ExecuteCommand(string command)
        {
            command = command.Replace("\"", "\\\"");

            ProcessStartInfo processInfo = null;

            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                processInfo = new ProcessStartInfo("/bin/bash", $"-c \"{command}\"");
            }

            if (processInfo == null)
            {
                return null;
            }
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            Process process = Process.Start(processInfo);
            process.WaitForExit();

            CommandResult commandResult = new CommandResult(process.ExitCode,
                                                            process.StandardOutput.ReadToEnd(),
                                                            process.StandardError.ReadToEnd());

            process.Close();

            return commandResult;
        }
    }
}
