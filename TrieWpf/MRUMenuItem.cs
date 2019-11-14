using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrieWpf
{
    public class MRUMenuItem
    {
        public ICommand OpenCommand { get; set; }
        public string ShortName { get; set; }
        public string FileName { get; set; }
        public string OpenParameter { get; set; }
        public bool IsSeparator
        {
            get; set;
        }
    }
}
