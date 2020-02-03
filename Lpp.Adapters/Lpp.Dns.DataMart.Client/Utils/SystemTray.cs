using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Lpp.Dns.DataMart.Lib.Classes;
using log4net;
using Lpp;
using System.ComponentModel;

namespace Lpp.Dns.DataMart.Client.Utils
{
    public class SystemTray
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string NotifyIconText = "DataMart Client";
        private static NotifyIcon notifyIcon = null;

        public static event EventHandler NotifyIconPicked;

        static void EnsureIcon()
        {
            if ( notifyIcon != null ) return;
            notifyIcon = new NotifyIcon { Icon = Properties.Resources.NotifyIconIdle };
            notifyIcon.MouseDoubleClick += notifyIcon_EventHandler;
            notifyIcon.Click += notifyIcon_EventHandler;
        }

        /// <summary>
        /// Dislay system tray Icon
        /// </summary>
        internal static void DisplaySystemTrayIcon()
        {
            EnsureIcon();
            notifyIcon.Icon = Properties.Resources.NotifyIconIdle;
            notifyIcon.Visible = true;
            notifyIcon.Text = NotifyIconText;
        }

        static void notifyIcon_EventHandler(object sender, EventArgs e)
        {
            NotifyIconPicked.Raise( null );
        }

        /// <summary>
        /// Dislay system tray Icon
        /// </summary>
        internal static void HideSystemTrayIcon()
        {
            if (null != notifyIcon) notifyIcon.Visible = false;
        }

        internal static void DisposeSystemTrayIcon()
        {
            if (notifyIcon == null)
                return;

            notifyIcon.Dispose();
            notifyIcon = null;
        }

        /// <summary>
        /// display new query notification icon
        /// </summary>
        /// <param name="message">Notification tool tip message to be displayed</param>
        internal static void DisplayNewQueryNotificationToolTip(string message)
        {
            EnsureIcon();
            notifyIcon.Icon = Properties.Resources.NotifyIconNewQuery;
            notifyIcon.BalloonTipText = message;
            notifyIcon.ShowBalloonTip(1000);
        }

        /// <summary>
        /// Update system tray notification icon/ message
        /// </summary>
        /// <param name="iconType"></param>
        /// <param name="message"></param>
        internal static void UpdateNotificationIcon(IconType iconType, string message)
        {
            EnsureIcon();

            switch (iconType)
            {
                case IconType.IconBusy:
                    notifyIcon.Icon = Properties.Resources.NotifyIconBusy;
                    notifyIcon.Text = EnsureLength((string.IsNullOrEmpty(message)) ? string.Format("{0} - Processing Queries", NotifyIconText) : message);
                    break;

                default:
                    notifyIcon.Icon = Properties.Resources.NotifyIconIdle;
                    notifyIcon.Text = EnsureLength((string.IsNullOrEmpty( message )) ? NotifyIconText : message);
                    break;
            }
        }

        /// <summary>
        /// Prepare notification and show through system tray icon
        /// </summary>
        /// <param name="hubRequest"></param>
        internal static void GenerateNotification(HubRequest hubRequest, int networkId)
        {
            log.Info( String.Format( "BackgroundProcess: Generating notification for query {0} from {1} (NetworkId: {2})", hubRequest.Source.ID, hubRequest.DataMartName, networkId ) );

            string message = $"New query submitted in { hubRequest.ProjectName } Project: { hubRequest.Source.Name }";
            DisplayNewQueryNotificationToolTip(message);
        }

        /// <summary>
        /// Update the tooltip of the NotifyIcon according to the current status
        /// </summary>
        internal static void UpdateNotifyText(int queriesProcessedCount, string dataMartName, int networkId)
        {
            string text = string.Format("DataMart Client - Processed {0} queries", queriesProcessedCount);
            UpdateNotificationIcon(IconType.IconBusy, text);            
        }

        static string EnsureLength(string message)
        {
            if (string.IsNullOrEmpty(message) || message.Length <= 64)
                return message;

            return message.Substring(0, 64);
        }
    }

    public enum IconType
    {
        IconIdle = 0,
        IconBusy = 1,
        IconDefault = 2
    }
}