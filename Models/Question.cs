using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    [Table("Questions")]
    public class Question
    {


        public int Id { get; set; }

        public int OwnerId { get; set; }

        public int AnswerId { get; set; }

        public int ResponserId { get; set; }

        public string Title { get; set; }

        public string Field { get; set; }

        public string Course { get; set; }

        public string Type { get; set; }

        public string Text { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string AttachFilePath { get; set; }

        public enum Statuses { InQueue, Answered, Seen };

    }
}
