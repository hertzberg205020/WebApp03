using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebApp03.Models
{
    public partial class Emp: BaseEntity<int>
    {
        public Emp()
        {
            Exams = new HashSet<Exam>();
        }
        // [Key]
        // public int Id { get; set; }

        // [Required]
        public string Name { get; set; }

        // [Required]
        public string EmpNo { get; set; }

        [JsonIgnore]
        public virtual ICollection<Exam> Exams { get; set; }
        
    }
}