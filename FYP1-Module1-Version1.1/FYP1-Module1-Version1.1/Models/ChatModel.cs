using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP1_Module1_Version1._1.Models
{
    public class ChatModel
    {
        public IEnumerable<conversation> chat { get; set; }
        public conversation Text { get; set; }
    }
}