using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealm.Utilities.Common.Interfaces
{
    public interface IPdfRenderer
    {
        /// <summary>
        /// Converts HTML markup to PDF and returns PDF as byte array
        /// </summary>
        /// <param name="sourceHtmlMarkup">Source HTML Markup</param>
        /// <param name="headerHtmlFile">Optional Header HTML file, which will be repeated on each page</param>
        /// <param name="footerHtmlFile">Optional Footer HTML file, which will be repeated on each page</param>
        /// <returns>byte array</returns>
        byte[] ConvertHtmlToPdf(string sourceHtmlMarkup, string headerHtmlFile = null, string footerHtmlFile = null);
    }
}
