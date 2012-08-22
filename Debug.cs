using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Xml;
using Core;
using System.Net.Mail;
using System.Net;


namespace NeoDebug
{
    /// <summary>
    /// In case of error it saves in the disk and/or emails the error report
    /// </summary>
    public static class Debug
    {
        static StreamWriter log;
        public static bool ReportSend = D.NeoDebug_ReportSend; //Sends by email
        public static bool ReportSave = D.NeoDebug_ReportSave; //Writes in the HD
        
        public static string LogFilePath = D.NeoDebug_LogFilePath;
        public static string LogFile = LogFilePath + D.NeoDebug_LogFile;
        public static string From = D.NeoDebug_From;
        public static string To = D.NeoDebug_To;
        public static string Subject = D.NeoDebug_Subject;
        public static string Body = D.NeoDebug_Body;
        public static string Host = D.NeoDebug_Host;
        public static int Port = D.NeoDebug_Port;
        public static string User = D.NeoDebug_User;
        public static string Password = D.NeoDebug_Password;

        private static void LogCreate()
        {
            try
            {
                File.Delete(LogFile);
            }
            catch { }
            log = new StreamWriter(LogFile);
            log.Close();
        }

        public static void ErrorRecord(string messageError)
        {
            string report;
            log = new StreamWriter(LogFile, true);
            report = DateTime.Now + " " + messageError;
            if(ReportSave)
                try
                {
                    LogWrite(report);
                }
                catch { }

            if (ReportSend)
            {
                try
                {
                    ReportEmailSend(report);
                }
                catch { }
            }
        }

        public static void ErrorRecord(Exception ex)
        {
            string report;
            report = ex.Source + " " + ex.Message + ex.StackTrace;
            if (ReportSave)
                try
                {
                    LogWrite(report);
                }
                catch { }

            if (ReportSend)
            {
                try
                {
                    ReportEmailSend(DateTime.Now + " " + report);
                }
                catch { }
            }
        }

        public static void LogWrite(string report)
        {
            try
            {
                log = new StreamWriter(LogFile, true);
                log.WriteLine("[" + DateTime.Now + "]" + " - " + report);
            }
            catch
            {
                log = new StreamWriter(D.ApplicationDirectory + "Log_thread_Roteiro.txt", true);
                log.WriteLine("[" + DateTime.Now + "]" + " - " + report);
            }finally{
                try{
                    log.Close();
                }catch{}
            }
        }


        private static bool ReportEmailSend(string report)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(User, Password);

                mail.To.Add(To);
                mail.Subject = D.ApplicationName + " Máquina " + Dns.GetHostName();

                mail.From = new System.Net.Mail.MailAddress(User);
                mail.IsBodyHtml = true;
                mail.Body = report;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = cred;
                smtp.Port = Port;
                smtp.Send(mail);
 
                return true;
            }
            catch (Exception ex)
            {
                report = DateTime.Now + " " + ex.Message;
                LogWrite(report);
                return false;
            }
        }

        /// <summary>
        /// Faz com que o log não ultrapasse a quantidade máxima de registros estipulada
        /// </summary>
        /// <param name="logRegisterNumber"> Número máximo de registros no log</param>
        public static void LogRegistersTrim(int logRegisterMaxNumber) {
            StreamReader logToUpdate = new StreamReader(LogFile, true);
            List<string> LogRegisters = new List<string>();
            string register = String.Empty;
            int registerCount = 0;
            
            while (!logToUpdate.EndOfStream)
            {
                string row = logToUpdate.ReadLine();
                if ((row.StartsWith("[")) && (row[11] == ' ') && (row[20] == ']'))
                {
                    registerCount++;
                    if (register != String.Empty)
                        LogRegisters.Add(register);
                    register = row + Environment.NewLine;
                }
                else
                    register += row + Environment.NewLine;

            }
            LogRegisters.Add(register);
            logToUpdate.Close();
            logToUpdate.Dispose();

            if (registerCount > logRegisterMaxNumber) {
                int registersToRemove = registerCount - logRegisterMaxNumber;
                LogRegisters.RemoveRange(0, registersToRemove);
                StringBuilder s = new StringBuilder();

                foreach (string logRegister in LogRegisters)
                {
                    s.Append(logRegister);
                }

                File.WriteAllText(LogFile, s.ToString());
            }
        }


    }
}
