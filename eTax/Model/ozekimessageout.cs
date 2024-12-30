using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTax.Model
{
    public class ozekimessageout
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Msg { get; set; }
        public string Senttime { get; set; }
        public string Receivedtime { get; set; }
        public string Status { get; set; }
        public string Operator { get; set; }
        public string Msgtype { get; set; }
        public string Errormsg { get; set; }
        public string Reference { get; set; }
    }
}