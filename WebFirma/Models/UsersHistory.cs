using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebFirma.Models
{
    public class UsersHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int OtdelId { get; set; }
        public int KvalId { get; set; }
        public int IspytId { get; set; }
        public DateTime Date_Start { get; set; }
        public bool fl { get; set; }
        public string ActionUsers { get; set; }
        public DateTime DateActionUsers { get; set; }
        [NotMapped]
        public string DateActionUsersString => DateActionUsers.ToString("dd.MM.yyyy hh:mm");
        [NotMapped]
        public List<properties_fields> usersHistory { get; set; }
    }
    public class properties_fields
    {
        public string Name { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
    public class prop_next
    {
        public int UserId { get; set; }
        public int CountProp { get; set; }
    }
}