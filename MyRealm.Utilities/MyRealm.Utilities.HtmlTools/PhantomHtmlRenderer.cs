using MyRealm.Utilities.Common.Helpers;
using MyRealm.Utilities.Common.Interfaces;
using MyRealm.Utilities.Common.Models;
using MyRealm.Utilities.HtmlTools.Extensions;
using MyRealm.Utilities.HtmlTools.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MyRealm.Utilities.HtmlTools
{
    public class PhantomHtmlRenderer : IHtmlRenderer
    {
        private readonly string _baseOpPath;
        private readonly string _phantomJSDirectory;
        private readonly string _phantomJSWorkSpaceDirectory;
        private readonly string _phantomJSTempDirectory;

        public PhantomHtmlRenderer()
        {
            _baseOpPath = FsHelper.BaseOperationPath;
            _phantomJSDirectory = Path.Combine(_baseOpPath, "Tools", "PhantomJs");
            _phantomJSWorkSpaceDirectory = Path.Combine(_phantomJSDirectory, "Workspace");
            _phantomJSTempDirectory = Path.Combine(_phantomJSWorkSpaceDirectory, "Temp");


            FsHelper.CreateDirectoryIfNotExists(_baseOpPath);
            FsHelper.CreateDirectoryIfNotExists(_phantomJSDirectory);
            FsHelper.CreateDirectoryIfNotExists(_phantomJSWorkSpaceDirectory);
            FsHelper.CreateDirectoryIfNotExists(_phantomJSTempDirectory);
            FsHelper.CreateFileIfNotExists(_phantomJSDirectory, "phantomjs.exe", Properties.Resources.phantomjs);
            FsHelper.CreateFileIfNotExists(_phantomJSWorkSpaceDirectory, "angular.js", Properties.Resources.angular);
            FsHelper.CreateFileIfNotExists(_phantomJSWorkSpaceDirectory, "rasterize.js", Properties.Resources.rasterize);
            FsHelper.CreateFileIfNotExists(_phantomJSWorkSpaceDirectory, "savepage.js", Properties.Resources.savepage);

        }

        
        public BoundHtmlTemplate BindTemplate(string templatePath, Dictionary<string, object> models = null,  
            string htmlBefore = null,  string htmlAfter = null, string parentElementToExtract = null,
            List<string> cssFiles = null, List<string> jsFiles = null)
        {
            var template = new HtmlTemplate(templatePath);

            template.BeginTemplate(htmlBefore, htmlAfter);
            template.AddModels(models);
            template.AddStylesheets(cssFiles);
            template.AddScripts(jsFiles);
            template.EndTemplate();

            string tempFileId = Guid.NewGuid().ToString();
            string tempFileName = tempFileId + ".html";
            string boundFileName = tempFileId + "-bound.html";

            string tempSrcFileFullPath = string.Format(@"{0}\{1}", _phantomJSTempDirectory, tempFileName);
            string tempSrcFileRelativePath = string.Format(@"Workspace/Temp/{0}", tempFileName);

            string tempDestFileFullPath = string.Format(@"{0}\{1}", _phantomJSTempDirectory, boundFileName);
            string tempDestBoundFileRelativePath = string.Format("Workspace/Temp/{0}", boundFileName);

            FsHelper.CreateFileIfNotExists(_phantomJSTempDirectory, tempFileName, template.TemplateContent);
            //now render the file to phantomJS.  Additionally save the rendered content back to the temp file (saving is defined in savepage.js).
            bindHtmlWithPhantom(parentElementToExtract, tempSrcFileRelativePath, tempDestBoundFileRelativePath);

            string boundTemplateContents = FsHelper.GetFileContents(tempDestFileFullPath);

            FsHelper.DeleteFileIfExists(tempSrcFileFullPath);
            FsHelper.DeleteFileIfExists(tempDestFileFullPath);

            return new BoundHtmlTemplate
            {
                TemplateHtml = boundTemplateContents
            };
        }

        private void bindHtmlWithPhantom(string parentElement, string tempSrcFileRelativePath, string tempSrcBoundFileRelativePath)
        {
            Process p = null;

            try
            {
                //now render the file to phantomJS.  Additionally save the rendered content back to the temp file (saving is defined in savepage.js).
                string drive = _phantomJSDirectory.Substring(0, 2);
                string command = string.Format(drive + " & " + @"cd {0} & phantomjs \Workspace\savepage.js {1} {2} {3} & exit", _phantomJSDirectory, tempSrcFileRelativePath, tempSrcBoundFileRelativePath, parentElement);
                var psi = new ProcessStartInfo("cmd.exe", "/K" + command);
                psi.CreateNoWindow = true;
                psi.ErrorDialog = false;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                psi.RedirectStandardError = true;
                psi.RedirectStandardOutput = true;

                p = Process.Start(psi);
                string stdOutput = p.StandardOutput.ReadToEnd();
                string stdError = p.StandardError.ReadToEnd();

                if (!string.IsNullOrWhiteSpace(stdError))
                {
                    if (!p.HasExited)
                        p.Kill();

                    throw new Exception("Phantom Exception: " + stdError);
                }

                p.WaitForExit(180000); //3 minutes

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
