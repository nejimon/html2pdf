using MyRealm.Utilities.Common.Helpers;
using MyRealm.Utilities.HtmlTools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRealm.Utilities.HtmlTools.Extensions
{
    internal static class TemplateExtensions 
    {
        public static void BeginTemplate(this HtmlTemplate template, 
            string htmlBefore = null, string htmlAfter = null)
        {
            ensureTemplatePathIsSet(template);

            var doc = new StringBuilder();
            doc.Append(@"<!DOCTYPE html>");
            doc.Append(@"<html>");
            doc.Append(@"<head>");
            doc.Append(@"<meta charset=""utf-8""/>");
            doc.Append(@"</head>");
            doc.Append(@"<body>");
            doc.Append(@"<script src=""../angular.js""></script>");
            doc.Append(@"<div ng-app='MyApp'>"); //create a temporary ng-app.
            doc.Append(@"<div ng-controller='MyController'>"); //create a temporary ng-controller.
            doc.Append(htmlBefore);
            string templateContents = FsHelper.GetFileContents (template.TemplatePath);
            validateTemplateContents(templateContents);
            doc.Append(templateContents);
            doc.Append(htmlAfter);
            doc.Append(@"</div>"); //controller div close
            doc.Append(@"</div>"); //app div close
            template.TemplateContent = doc.ToString();
        }

        public static void AddModels(this HtmlTemplate template, Dictionary<string, object> models)
        {
            ensureTemplatePathIsSet(template);

            var doc = new StringBuilder(template.TemplateContent);
            doc.Append(@"<script>");
            doc.Append(@"var myApp = angular.module('MyApp', []);");
            doc.Append(@"myApp.controller('MyController', function ($scope) {");

            //add models to the controller.
            if (models != null)
            {
                foreach (var kvp in models)
                {
                    var jsonModel = Newtonsoft.Json.JsonConvert.SerializeObject(kvp.Value);

                    if (jsonModel.Trim().StartsWith(@"""function"))
                        jsonModel = jsonModel.Trim('"');

                    doc.Append("$scope." + kvp.Key + "=" + jsonModel + ";");
                }
            }

            doc.Append(@"});");
            doc.Append(@"</script>");
            template.TemplateContent = doc.ToString();
        }

        public static void AddStylesheets(this HtmlTemplate template, List<string> cssFilePaths)
        {
            ensureTemplatePathIsSet(template);

            var doc = new StringBuilder(template.TemplateContent);

            if (cssFilePaths != null)
            {
                doc.Append("<style>");
                foreach (var cssFile in cssFilePaths)
                {
                    string cssContents = FsHelper.GetFileContents(cssFile);
                    doc.Append(cssContents);
                }
                doc.Append("</style>");
            }

            template.TemplateContent = doc.ToString();
        }

        public static void AddScripts (this HtmlTemplate template, List<string> jsFilePaths)
        {
            ensureTemplatePathIsSet(template);

            var doc = new StringBuilder(template.TemplateContent);

            if (jsFilePaths != null)
            {
                doc.Append("<script>");
                foreach (var jsFile in jsFilePaths)
                {
                    string jsContents = FsHelper.GetFileContents(jsFile);
                    doc.Append(jsContents);
                }
                doc.Append("</script>");
            }

            template.TemplateContent = doc.ToString();
        }

        public static void EndTemplate(this HtmlTemplate template)
        {
            ensureTemplatePathIsSet(template);

            var doc = new StringBuilder(template.TemplateContent);
            doc.Append("</body>");
            doc.Append("</html>");
            template.TemplateContent = doc.ToString();
        }

        private static void ensureTemplatePathIsSet(HtmlTemplate template)
        {
            if (string.IsNullOrWhiteSpace(template.TemplatePath))
                throw new InvalidOperationException("Please invoke BeginTemplate method first!");
        }

        private static void validateTemplateContents(string templateContents)
        {
            var forbiddenTags = new List<string>
            {
                "<html>", "<head>", "<title>"
            };

            if (forbiddenTags.Any(tag => templateContents.ToLower().Contains(tag)))
                throw new NotSupportedException("The template must not contain these tags: " + string.Join(",", forbiddenTags) + ". They will automatically be added by the template builder library.");
        }
    }
}
