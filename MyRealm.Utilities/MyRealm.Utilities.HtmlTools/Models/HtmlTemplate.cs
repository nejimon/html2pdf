using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealm.Utilities.HtmlTools.Models
{
    internal class HtmlTemplate
    {
        private readonly string _templatePath;

        internal HtmlTemplate(string templatePath)
        {
            _templatePath = templatePath;
        }

        public string TemplateContent { get; set; }
        public string TemplatePath { get { return _templatePath; } }
    }
}
