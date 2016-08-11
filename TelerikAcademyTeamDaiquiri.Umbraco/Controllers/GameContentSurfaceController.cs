namespace TelerikAcademyTeamDaiquiri.Umbraco.Controllers
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using TelerikAcademyTeamDaiquiri.Umbraco.Models.Macros;
    using TelerikAcademyTeamDaiquiri.Umbraco.Models;

    public class GameContentSurfaceController : SurfaceController
    {
        private const string FilePathRegex = @"^\/media\/(\d{4,})\/(.+)\.zip$";

        private const string StorylineFileName = "index.html";

        public ActionResult RenderGameContent(int contentZipArchive)
        {
            var storylineContentNode = Umbraco.TypedContent(contentZipArchive);
            if (storylineContentNode.DocumentTypeAlias != nameof(GameContent))
            {
                throw new ArgumentException($"Content with id {contentZipArchive} is not a valid {nameof(GameContent)} node", nameof(contentZipArchive));
            }

            var archivePath = storylineContentNode.GetPropertyValue<string>(nameof(GameContent.ContentZipArchive));
            var contentWidth = storylineContentNode.GetPropertyValue<int>(nameof(GameContent.Width));
            var contentHeight = storylineContentNode.GetPropertyValue<int>(nameof(GameContent.Height));

            string storylineContentPath = string.Empty;

            var regexResult = Regex.Match(archivePath, FilePathRegex);
            if (regexResult.Success)
            {
                var fileId = regexResult.Groups[1].ToString();
                var fileName = regexResult.Groups[2].ToString();
                var targetDirectory = $"~/GameContent/{fileName}-{fileId}/";
                var serverDirectory = Server.MapPath(targetDirectory);

                if (!Directory.Exists(serverDirectory))
                {
                    Directory.CreateDirectory(serverDirectory);
                    using (FileStream archiveStream = new FileStream(Server.MapPath("~" + archivePath), FileMode.Open))
                    {
                        using (ZipArchive archive = new ZipArchive(archiveStream, ZipArchiveMode.Read))
                        {
                            archive.ExtractToDirectory(serverDirectory);
                        }
                    }
                }

                if (System.IO.File.Exists(Path.Combine(serverDirectory, StorylineFileName)))
                {
                    storylineContentPath = Path.Combine(targetDirectory.Replace("~", ""), StorylineFileName);
                }
                else
                {
                    throw new ArgumentException($"Storyline content archive {archivePath} doesnt contain file with name {StorylineFileName}");
                }
            }
            else
            {
                throw new ArgumentException("The attached file is not it the proper format. Please attach a Zip archive!", nameof(contentZipArchive));
            }

            var gameContentViewModel = new GameContentViewModel
            {
                ContentFilePath = storylineContentPath,
                Width = contentWidth,
                Height = contentHeight
            };

            return View("~/Views/RenderGameContent.cshtml", gameContentViewModel);
        }
    }
}