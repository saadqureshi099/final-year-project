using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP1_Module1_Version1._1.Models
{
    public class ViewModel
    {
        public IEnumerable<notification> notifications { get; set; }
        public IEnumerable<teacher> Teachers { get; set; }
        public IEnumerable<idea> Ideas { get; set; }
        public int SelectedTeacherId { get; set; }
       public IEnumerable<milestone> milestones {get; set;}
        public IEnumerable<registration> registration { get; set; }
        
    }
}
