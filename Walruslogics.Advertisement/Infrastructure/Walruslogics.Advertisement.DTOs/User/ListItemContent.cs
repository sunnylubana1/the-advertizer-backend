using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Walruslogics.Advertisement.DTOs.User
{
    public class ListItemContent
    {
        public ListItemContent() { }
        
        public long Value { get;set; }
        public string Text { get; set; }
        public bool selected { get; set; }
    }
}
