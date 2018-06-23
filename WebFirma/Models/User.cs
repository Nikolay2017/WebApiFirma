using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebFirma.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int OtdelId { get; set; }
        public Otdel Otdel { get; set; }
        public int KvalId { get; set; }
        public Kval Kval { get; set; }
        public int IspytId { get; set; }
        public Ispyt Ispyt { get; set; }
        public DateTime Date_Start { get; set; }
        [NotMapped]
        public string DateStartString => Date_Start.ToString("dd.MM.yyyy hh:mm");
        public bool fl { get; set; }
    }
}
