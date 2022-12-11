using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApp03.Models
{
    public partial class Exam: BaseEntity<int>
    {
        // public int Id { get; set; }

        public int EmpId { get; set; }

        public int SubjectId { get; set; }

        public int Score { get; set; }

        [JsonIgnore]
        public virtual Emp Emp { get; set; }

        [JsonIgnore]
        public virtual Subject Subject { get; set; }
    }
}