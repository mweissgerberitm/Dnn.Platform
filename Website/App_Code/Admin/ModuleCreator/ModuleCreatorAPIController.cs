#region Copyright

// 
// Copyright (c) 2013
// by DotNetNuke
// 

#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Web.Api;

#endregion

namespace DotNetNuke.ModuleCreator
{
    [AllowAnonymous]
    public class ModuleCreatorController : DnnApiController
    {
        [HttpPost]
        [HttpGet]
        public HttpResponseMessage MyResponse()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello World!");
        }

        [HttpPost]
        public HttpResponseMessage GetLanguages()
        {
            var folders = new List<FolderDto>();
            var folderList = Directory.GetDirectories(Globals.ApplicationMapPath + @"\DesktopModules\Admin\ModuleCreator\Templates");
            foreach (string folderPath in folderList)
            {
                var folderName = folderPath.Substring(folderPath.LastIndexOf(@"\") + 1);
                folders.Add(new FolderDto { Name = folderName });
            }
            return Request.CreateResponse(HttpStatusCode.OK, folders);
        }

        [HttpPost]
        public HttpResponseMessage GetTemplates(FolderDto folder)
        {
            var folders = new List<FolderDto>();
            var folderList = Directory.GetDirectories(Globals.ApplicationMapPath + @"\DesktopModules\Admin\ModuleCreator\Templates\" + folder.Name);
            foreach (string folderPath in folderList)
            {
                var folderName = folderPath.Substring(folderPath.LastIndexOf(@"\") + 1);
                folders.Add(new FolderDto { Name = folderName });
            }
            return Request.CreateResponse(HttpStatusCode.OK, folders);
        }

        [HttpPost]
        public HttpResponseMessage GetSnippets(FolderDto folder)
        {
            var snippets = new List<Snippet>();
            var snippetList = Directory.GetFiles(Globals.ApplicationMapPath + @"\DesktopModules\Admin\ModuleCreator\Templates\" + folder.Name + @"\Snippets");
            foreach (string snippetPath in snippetList)
            {
                var name = snippetPath.Substring(snippetPath.LastIndexOf(@"\") + 1);
                var readMe = Null.NullString;
                TextReader tr = new StreamReader(snippetPath);
                readMe = tr.ReadToEnd();
                tr.Close();

                snippets.Add(new Snippet { Name = name, Content = readMe });
            }
            return Request.CreateResponse(HttpStatusCode.OK, snippets);
        }

        [HttpPost]
        public HttpResponseMessage SaveSnippet(Snippet snippet)
        {
            var a = Globals.ApplicationMapPath + @"\DesktopModules\Admin\ModuleCreator\Templates\" + snippet.Name;
            using (StreamWriter sw = File.CreateText(Globals.ApplicationMapPath + @"\DesktopModules\Admin\ModuleCreator\Templates\" + snippet.Name))
            {
                sw.Write(snippet.Content);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage DeleteSnippet(Snippet snippet)
        {
            var snippetFile = Globals.ApplicationMapPath + @"\DesktopModules\Admin\ModuleCreator\Templates\" + snippet.Name;
            File.Delete(snippetFile);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
