using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHN.Export.Soln.Helpers.PdfGenerate
{
    public class SectionPageNumber
    {
        public string Section { get; set; }
        public string StartPageNumber { get; set; }
        public string EndPageNumber { get; set; }
    }

    public class PdfContent
    {
        public string Section { get; set; }
        public string SectionImage { get; set; }
        public List<SubSection> SubSestions { get; set; }
    }

    public class SubSection
    {
        public string Header { get; set; }
        public List<string> Contents { get; set; }
    }
}
