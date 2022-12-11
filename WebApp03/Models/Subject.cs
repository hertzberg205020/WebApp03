using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WebApp03.Models
{
    /// <summary>
    /// https://blog.darkthread.net/blog/ef-core-notes-3/
    /// https://exceptionnotfound.net/entity-framework-for-beginners-code-first-model-using-fluentapi/
    /// </summary>
    public partial class Subject: BaseEntity<int>
    {
        public Subject()
        {
            Exams = new HashSet<Exam>();
        }

        // public int Id { get; set; }

        public string Name { get; set; }
        
        /// <summary>
        /// 避免導覽屬性循環引用
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Exam> Exams { get; set; }
    }
}