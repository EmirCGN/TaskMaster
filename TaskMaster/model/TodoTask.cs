using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.model
{
    public class TodoTask
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public string Status { get; set; } = null!;
        public int Priority { get; set; } 
        public string Notes { get; set; } = null!;
        public bool Completed { get; set; }
    }
}
