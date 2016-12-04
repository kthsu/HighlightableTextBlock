using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace HighlightSample
{
    public class ViewModel : BindableBase
    {
        private string filter;
        private string fullText;

        public string Filter
        {
            get { return filter; }
            set { SetProperty(ref this.filter, value); }
        }        

        public string FullText
        {
            get { return fullText; }
            set { SetProperty(ref this.fullText, value); }
        }

    }
}
