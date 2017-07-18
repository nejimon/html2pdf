using MyRealm.Utilities.Common.Interfaces;
using MyRealm.Utilities.HtmlTools;
using MyRealm.Utilities.PdfTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnListDemo_Click(object sender, EventArgs e)
        {
            var model = new Dictionary<string, object> {
                {
                    "doc", new {CompanyName = "MyRealm Corporation", ReportTitle = "Employee Report" }
                },
                {
                    "employees", new List<dynamic>{
                        new { Name = "Nejimon CR", Designation = "Software Architect"  },
                        new { Name = "John Doe", Designation = "Project Manager"  },
                        new { Name = "Jane Doe", Designation = "Software Engineer"  }
                    }
                }
            };

            IHtmlRenderer htmlRenderer = new PhantomHtmlRenderer();
            var boundTemplate = htmlRenderer.BindTemplate(Application.StartupPath + @"\Templates\sampleTemplate.html", model);

            IPdfRenderer pdfRenderer = new WkHtml2PdfRenderer();
            var pdfBytes = pdfRenderer.ConvertHtmlToPdf(boundTemplate.TemplateHtml);

            writeFileAndOpen(pdfBytes);

        }

        private void writeFileAndOpen(byte[] bytes)
        {
            string newName = Guid.NewGuid().ToString() + ".pdf";
            File.WriteAllBytes(newName, bytes);
            Process.Start(newName);
        }
    }
}
