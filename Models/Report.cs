using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication4.Models
{ 
    [Table("Reports")]
    public class Report
    {


     
        public int ID { get; set; }

        public int UserID { get; set; }

        public string Username { get; set; }

        public DateTime SubmitDate { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        
        public bool IsSeen { get; set; }

        public int AdminSeenID { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

    }
}
