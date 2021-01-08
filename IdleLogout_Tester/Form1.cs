using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdleLogout;

namespace IdleLogout_Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private IdleLogout.SessionHelper_RDP sesshelper;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.sesshelper = new IdleLogout.SessionHelper_RDP();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           

            String msg = "";
            ArrayList sessions = sesshelper.getDisconnectedSessions();
            foreach (object entry in sessions)
            {
                msg = msg + entry.ToString() + "\r\n\r\n";
            }

            if (!msg.Equals("")) { 
                MessageBox.Show(msg);
            }

            foreach (SessionInfo entry in sessions)
            {
                if (!this.sesshelper.endSession(entry))
                {
                    //Error while closing session
                    MessageBox.Show("Could not Close Session with ID: " + entry.SessionId + 
                        ". Do you have the permissions needed to logoff user-sessions? Try running again with elevated privs.");
                }
            }
        }
    }
}
