using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UkTote.Message
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class MetaAttribute : Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        public Enums.MessageType MessageType { get; set; }
        public Enums.ActionCode ActionCode { get; set; }

        // This is a positional argument
        public MetaAttribute(Enums.MessageType messageType, Enums.ActionCode actionCode)
        {
            MessageType = messageType;
            ActionCode = actionCode;
        }

        // This is a named argument
        public int NamedInt { get; set; }
    }
}
