using MyRealm.Utilities.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealm.Utilities.Common.Interfaces
{
    public interface IHtmlRenderer
    {
        /// <summary>
        /// Takes an HTML template and a model and outputs an HTML bound with the data.
        /// </summary>
        /// <param name="templatePath">Path to the HTML template</param>
        /// <param name="models">Optional dictionary of models</param>
        /// <param name="htmlBefore">Optional HTML fragment to insert before</param>
        /// <param name="htmlAfter">Optional HTML fragment to insert after</param>
        /// <param name="parentElementToExtract">Optional tag specification if a part of the template only needs to be rendered</param>
        /// <param name="cssFiles">Optional CSS files</param>
        /// <param name="jsFiles">Optional Javascript files</param>
        /// <returns>New HTML markup bound with the model data</returns>
        BoundHtmlTemplate BindTemplate(string templatePath, Dictionary<string, object> models = null,
            string htmlBefore = null, string htmlAfter = null, string parentElementToExtract = null,
            List<string> cssFiles = null, List<string> jsFiles = null);
    }
}
