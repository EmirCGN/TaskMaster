using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.model
{
    public class TodoTask
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
        public string Notes { get; set; }
        public bool Completed { get; set; }
    }
}
