using Co0nUtilZ;
using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace IdleLogout
{

    partial class IdleLogout
    {
        private string versioninfo;
        private int _jobintervall;
        private Co0nUtilZ.C_LoggingHelper logger;

        //Registry stuff, for options
        private C_RegistryHelper myRegHelper;
        private RegistryKey userRoot;
        private string subkey;  //Note: If the binary is compiled as x86 and run on x64-OS it will look in its "WOW6432Node"-Equivalent

        private SessionHelper_RDP sesshelper;  //Session-Helper to iterate and close user-sessions


        private System.Threading.Timer timer; //A timer to run thing periodically    

        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "TeminateIdleUsersessions";
            this.versioninfo = "v0.11 (08.01.2021)";
            this._jobintervall = 300000; //Default intervall for jobs to pe processed
            this.logger = new C_LoggingHelper(this.ServiceName, this.ServiceName + " " + this.versioninfo + ": ");


            //Registry stuff, for options        
            this.userRoot = Registry.LocalMachine;
            this.subkey = @"SOFTWARE\OL-IT\IdleLogoutService";  //Note: If the binary is compiled as x86 and run on x64-OS it will look in its "WOW6432Node"-Equivalent
            this.myRegHelper = new C_RegistryHelper(this.userRoot, this.subkey);

            this.sesshelper = new SessionHelper_RDP();
        }
    

        /// <summary>
        /// Read service-settings from the registry
        /// </summary>
        private void readservicesettings()
        {
            C_RegistryHelper reghelper = new C_RegistryHelper(this.userRoot, this.subkey);
            bool found_intervall = false;

            try {                 
                List<string> valuenames = reghelper.ListValues();

                foreach (string valnam in valuenames)
                {
                    switch (valnam)
                    {
                        case "jobintervall":
                            this._jobintervall = int.Parse(reghelper.ReadSettingFromRegistry("", valnam));
                            this.logger.LogInfo("Setting jobintervall to " + this._jobintervall.ToString() + " milliseconds.", 10003);
                            found_intervall = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error while reading settings from registry => " + ex.ToString(), 11004);
            }

            if (!found_intervall)
            {
                this.logger.LogWarn("Could not find Intervallsetting (\"jobintervall\") in registry. Will use default value.", 11005);
            }
        }
    }
    #endregion
}

