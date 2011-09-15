using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNNLOmics.Utilities {

    public delegate void MessageEventHandler(object sender, MessageEventArgs e);
    
    public class MessageEventArgs : EventArgs
    {
        public readonly string Message;

        public MessageEventArgs(string strMessage) 
        {
            Message = strMessage;
        }
    }
}
