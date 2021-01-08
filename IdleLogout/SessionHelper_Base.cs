using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IdleLogout
{
    public class SessionInfo
    {
        public string UserName;
        public string Domain;
        public int SessionId;
        public string Client;
        public string Server;
        public WTS_CONNECTSTATE_CLASS ConnectionState;
        public WTSINFOA sessionInfo;

        public override string ToString()
        {
            return /* string myreturn = */ this.UserName +
                " (ID: " + this.SessionId.ToString() + ") " +
                this.ConnectionState.ToString();//+ 
                                                //" => Times: Now=" + this.sessionInfo.CurrentTime.ToString() + 
                                                //", Lastinput= " + this.sessionInfo.LastInputTime.ToString() + 
                                                //", IdleMinutes=" + this.sessionInfo.IdleTime.TotalMinutes;
                                                //return myreturn;
        }
    }

    public abstract class SessionHelper_Base
    {
        /// <summary>
        /// Gets existing user-sessions (internally used)
        /// </summary>        
        /// <returns></returns>
        protected abstract List<SessionInfo> FetchDisconnectedSessions(string server);

        /// <summary>
        /// List existing user sessions
        /// </summary>
        /// <returns>An Arraylist containing all active sessions</returns>
        public abstract ArrayList getDisconnectedSessions();

        /// <summary>
        /// Terminates an existing session
        /// </summary>
        /// <param name="sess">the session to be terminated</param>
        /// <returns>True on success, otherwise false</returns>
        public abstract bool endSession(KeyValuePair<int, String> sess, String ServerName);

        public abstract bool endSession(SessionInfo sessionInfo);
    }
}
