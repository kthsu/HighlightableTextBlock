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
        private string filterText;
        private string fullText;

        public string FilterText
        {
            get { return filterText; }
            set { SetProperty(ref this.filterText, value); }
        }        

        public string FullText
        {
            get { return fullText; }
            set { SetProperty(ref this.fullText, value); }
        }

    }
}
