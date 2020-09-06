using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    [Table("Answers")]
    public class Answer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public int OwnerId { get; set; }


        public string Text { get; set; }


        public DateTime Date { get; set; }
    }
}
