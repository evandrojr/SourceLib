﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Core;

namespace NeoDebug
{
    //public static class Mail
    //{

    //    public static string From = D.NeoDebug_From;
    //    public static string To = D.NeoDebug_To;
    //    public static string Subject = D.NeoDebug_Subject;
    //    public static string Body = D.NeoDebug_Body;
    //    public static string Host = D.NeoDebug_Host;
    //    public static string Port = D.NeoDebug_Port;
    //    public static string User = D.NeoDebug_User;
    //    public static string Password = D.NeoDebug_Password;



    //    public static bool DebugEnviar(string msg){

    //        try
    //        {
    //            // TODO: Add error handling for invalid arguments

    //            // To
    //            MailMessage mailMsg = new MailMessage();
    //            mailMsg.To.Add(To);

    //            // From
    //            MailAddress mailAddress = new MailAddress(From);
    //            mailMsg.From = mailAddress;

    //            // Subject and Body
    //            mailMsg.Subject = Subject;
    //            mailMsg.Body = msg;

    //            // Init SmtpClient and send
    //            SmtpClient smtpClient = new SmtpClient(Host, Convert.ToInt32(Port));
    //            System.Net.NetworkCredential credentials = 
    //               new System.Net.NetworkCredential(User, Password);
    //            smtpClient.Credentials = credentials;

    //            smtpClient.Send(mailMsg);
    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }

    //}
}
