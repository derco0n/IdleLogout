using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.Threading;
using System.Collections;

namespace IdleLogout
{
    //Installer wird zum (De)Registrieren des Service benötigt
    /*
     
     Installieren mit  : C:\Windows\Microsoft.NET\Framework64\v4.0.30319> .\InstallUtil.exe /LogToConsole=true "Pfad\Zum\Binary.exe"
     Deinstallieren mit: C:\Windows\Microsoft.NET\Framework64\v4.0.30319> .\InstallUtil.exe /u "Pfad\Zum\Binary.exe"
         
    */
    [RunInstaller(true)]
    public class IdleLogoutServiceInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;
        private String MyName = "TeminateIdleUsersessions";

        public IdleLogoutServiceInstaller()
        {
            processInstaller =
                new ServiceProcessInstaller();
            serviceInstaller =
                new ServiceInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem; //Läuft im Kontext SYSTEM (hat nur lokale Rechte)                                            
            serviceInstaller.StartType = ServiceStartMode.Automatic; //Starttyp  //anpassbar
            serviceInstaller.ServiceName = MyName; //Namen ggf. anpassen
            serviceInstaller.Description = "A service that periodically logs out disconnected, idle user-sessions. Written by D. Marx (derco0n, 2021). For documentation see: https://intranet.olplastik.de/it/wikiintern/Seiten/Administration/Automatisierte%20Jobs/Automatische%20Trennung%20inaktiver%20Benutzersitzungen.aspx";
            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }

    /// <summary>
    /// This service will logout idle-users after a specified timeout
    /// </summary>
    public partial class IdleLogout : ServiceBase
    {
      

        public IdleLogout()
        {

            this.CanStop = true;
            this.CanPauseAndContinue = false; //Nicht pausierbar
            this.AutoLog = true;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            this.readservicesettings();

            //Einen Timer instanziieren
            TimerCallback tick =
            new TimerCallback(CheckAndLogout); //Bei jedem Timertick die Methode "CheckAndLogout" aufrufen
            this.timer = new System.Threading.Timer(tick, null, 0, this._jobintervall);

            this.logger.LogInfo("TeminateIdleUsersessions-Service started with an intervall of " + (this._jobintervall/1000).ToString() + " seconds.", 10000);
        }

        private void CheckAndLogout(object state)
        {

            try
            {
                
                ArrayList sessions = sesshelper.getDisconnectedSessions();  // Get all disconnected User sessions     
                
                if (sessions.Count == 0)
                {
                    this.logger.LogInfo("Could not find any disconnected session.", 10003);
                    return;
                }
                else
                {
                    this.logger.LogInfo("Found " + sessions.Count.ToString() + " disconnected sessions...", 10003);
                }

                foreach (SessionInfo entry in sessions)
                {
                    this.logger.LogInfo("Trying to close user-session: \"" + entry.ToString() +"\"", 10001);

                    if (!this.sesshelper.endSession(entry)) //Try to close the session
                    {
                        //Error while closing session
                        this.logger.LogError("Could not close user-session: \"" + entry.ToString() + "\"" +
                            "\r\nDo you have the permissions needed to logoff user-sessions? Try running again with elevated privs.", 11001);                       
                    }
                    else
                    {
                        //Success
                        this.logger.LogInfo("Session \"" + entry.ToString() + "\" has been terminated.", 10002);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error while running CheckAndLogout() => " + ex.ToString(), 11000);
            }
        }

        protected override void OnStop()
        {
        }

        /*
        protected void checkUSers()
        {

        }
        */
    }
}
