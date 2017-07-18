using MyRealm.Utilities.Common.Helpers;
using MyRealm.Utilities.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealm.Utilities.PdfTools
{
    public class WkHtml2PdfRenderer : IPdfRenderer
    {
        private readonly string _wkhtmlToPdfPath;
        private readonly string _workSpaceDirectory;

        public WkHtml2PdfRenderer()
        {
            string baseOpPath = FsHelper.BaseOperationPath;
            string wkHtmlToPdfDirectory = Path.Combine(baseOpPath, "Tools", "WkHtml2Pdf");
            string binDir = Path.Combine(wkHtmlToPdfDirectory, "bin");
            string wkhtmltoxDir = Path.Combine(wkHtmlToPdfDirectory, "include", "wkhtmltox");
            string libDir = Path.Combine(wkHtmlToPdfDirectory, "lib");
            _workSpaceDirectory = Path.Combine(wkHtmlToPdfDirectory, "WorkSpace");

            FsHelper.CreateDirectoryIfNotExists(baseOpPath);
            FsHelper.CreateDirectoryIfNotExists(wkHtmlToPdfDirectory);
            FsHelper.CreateDirectoryIfNotExists(binDir);
            FsHelper.CreateDirectoryIfNotExists(wkhtmltoxDir);
            FsHelper.CreateDirectoryIfNotExists(libDir);
            FsHelper.CreateDirectoryIfNotExists(_workSpaceDirectory);

            FsHelper.CreateFileIfNotExists(binDir, "wkhtmltoimage.exe", Properties.Resources.wkhtmltoimage);
            FsHelper.CreateFileIfNotExists(binDir, "wkhtmltopdf.exe", Properties.Resources.wkhtmltopdf);
            FsHelper.CreateFileIfNotExists(binDir, "wkhtmltox.exe", Properties.Resources.wkhtmltox);

            FsHelper.CreateFileIfNotExists(wkhtmltoxDir, "dllbegin.inc", Properties.Resources.dllbegin);
            FsHelper.CreateFileIfNotExists(wkhtmltoxDir, "dllend.inc", Properties.Resources.dllend);
            FsHelper.CreateFileIfNotExists(wkhtmltoxDir, "image.h", Properties.Resources.image);
            FsHelper.CreateFileIfNotExists(wkhtmltoxDir, "pdf.h", Properties.Resources.pdf);

            FsHelper.CreateFileIfNotExists(libDir, "wkhtmltox.lib", Properties.Resources.wkhtmltoxlib);

            
            _wkhtmlToPdfPath = Path.Combine(binDir, "wkhtmltopdf.exe");
        }

         public byte[] ConvertHtmlToPdf(string sourceHtmlMarkup, string headerHtmlFile = null, string footerHtmlFile = null)
        {
            Process p = null;

            try
            {
                string uniqueFileId = Guid.NewGuid().ToString();
                string uniqueFileName = uniqueFileId + ".html";
                string sourceHtmlFile = Path.Combine(_workSpaceDirectory, uniqueFileName);

                FsHelper.CreateFileIfNotExists(_workSpaceDirectory, uniqueFileName, sourceHtmlMarkup);

                string targetPdfFileName = sourceHtmlFile.Replace(".html", ".pdf");
                string arguments = string.Format(@"""file:///{0}"" ""{1}""", sourceHtmlFile, targetPdfFileName);

                if (!string.IsNullOrWhiteSpace(footerHtmlFile))
                    arguments = string.Format(@"--footer-html ""file:///{0}"" {1}", footerHtmlFile, arguments);

                if (!string.IsNullOrWhiteSpace(footerHtmlFile))
                    arguments = string.Format(@"--header-html ""file:///{0}"" {1}", headerHtmlFile, arguments);

                // prepare process start info
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.UseShellExecute = false;
                psi.FileName = _wkhtmlToPdfPath;
                psi.CreateNoWindow = true;
                psi.Arguments = arguments; // @"-q -n """ + sourceHtmlFile + @""" """ + targetPdfFileName + @""""; // quiet, not run scripts

                using (p = Process.Start(psi))
                {
                    p.WaitForExit();
                }

                if (!File.Exists(targetPdfFileName))
                    throw new FileNotFoundException("Failed to create PDF file.");

                var fileBytes = FsHelper.GetFileContentsAsBytes(targetPdfFileName);

                FsHelper.DeleteFileIfExists(targetPdfFileName);
                FsHelper.DeleteFileIfExists(sourceHtmlFile);

                return fileBytes;
            }
            catch 
            {
                if (!p.HasExited)
                    p.Kill();

                throw;
            }
        }
    }
}
