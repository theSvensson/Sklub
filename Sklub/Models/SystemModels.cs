using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sklub.Models
{
    public enum Status { Open, InProgress, Closed }

    public class Ticket
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public Status Status { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}