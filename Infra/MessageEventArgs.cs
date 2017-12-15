using System;

namespace MVVMApplication.Infra
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; }

        public MessageEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
