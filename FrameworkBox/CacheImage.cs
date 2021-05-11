using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrameworkBox
{
    public static class CacheImage
    {
        //hello

        private static readonly string appCacheFolder = $@"{ Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}" + @$"\{Application.CompanyName}" + @$"\{Application.ProductName}\";

        public static async Task<bool> DownloadImageList(IList<Tuple<Uri, string, int>> imagesList) // uri, id, groupname
        {
            try
            {
                for (int i = 0; i < imagesList.Count; i++)
                {
                    CreateFolder(imagesList[i].Item2);
                    await DownloadImage(imagesList[i].Item1, imagesList[i].Item2, imagesList[i].Item3);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }

        public static async Task<bool> DownloadOneImage(Tuple<Uri, string, int> specificImage) // uri, id, groupname
        {
            try
            {
                CreateFolder(specificImage.Item2);
                await DownloadImage(specificImage.Item1, specificImage.Item2, specificImage.Item3);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return false;
            }
        }

        private static async Task<Image> DownloadOneImageAndReturn(Tuple<Uri, string, int> specificImage) // uri, id, groupname
        {
            string localDir = $"{ appCacheFolder }" + $@"{specificImage.Item2}" + @"\";
            string urlWithoutSimbols = Regex.Replace(specificImage.Item1.ToString(), @"[\/:*?''<>|]", "");
            //
            string searchPath = localDir + specificImage.Item3 + "_" + urlWithoutSimbols;

            try
            {
                CreateFolder(specificImage.Item2);
                await DownloadImage(specificImage.Item1, specificImage.Item2, specificImage.Item3);
                return Image.FromFile(searchPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex}");
                return null;
            }
        }

        public async static Task<Image> SearchImage(Uri url, string groupName, int id)
        {
            try
            {
                string localDir = $"{ appCacheFolder }" + $@"{groupName}" + @"\";
                string urlWithoutSimbols = Regex.Replace(url.ToString(), @"[\/:*?''<>|]", "");
                //
                string searchPath = localDir + id + "_" + urlWithoutSimbols;

                if (File.Exists(Path.Combine(searchPath)))
                    return Image.FromFile(searchPath);
                else
                {
                    var imgToDownload = Tuple.Create(url, groupName, id);
                    return await DownloadOneImageAndReturn(imgToDownload);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on searching this image. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static bool DeleteGroup(string groupName)
        {
            try
            {
                string localDir = $"{ appCacheFolder }" + $@"{groupName}";
                var directory = new DirectoryInfo(localDir);
                // Deletar cache das imagens do restaurante

                if (directory.Exists)
                {
                    DirectoryInfo di = new(localDir);

                    foreach (FileInfo dirFile in di.GetFiles())
                    {
                        dirFile.IsReadOnly = false;
                        dirFile.Delete();
                    }

                    di.Attributes = FileAttributes.Normal;
                    directory.Delete(true);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred on deleting this group... Try Again. \n\nError Message: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool DeleteGroups(List<string> groupsName)
        {
            try
            {
                foreach (string dirName in groupsName)
                {
                    DeleteGroup(dirName);
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred on deleting this group... Try Again. \n\nError Message: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static async Task<bool> DownloadImage(Uri url, string groupName, int id)
        {
            string localDir = $"{ appCacheFolder }" + $@"{groupName}" + @"\";
            string urlWithoutSimbols = Regex.Replace(url.ToString(), @"[\/:*?''<>|]", "");
            //
            string saveToPath = localDir + id + "_" + urlWithoutSimbols;
            //

            // MessageBox.Show($"{localDir}");  // see the path where your image is being saved

            if (File.Exists(Path.Combine(saveToPath)))
            {
                // var img = Image.FromFile(saveToPath); instance of image to use <
                return true; // the image already exists in the directory
            }
            else
            {
                //if dont found img in folder... go to download!
                using WebClient wc = new();
                try
                {
                    await wc.DownloadFileTaskAsync(url, saveToPath);
                    ReadOnly(saveToPath);
                    return true;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show($"{ex}");
                    MessageBox.Show("The requested image could not be downloaded. Try again later or check if there is an active internet connection.", "Erro On Download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    wc.Dispose();
                }
            }
        }

        private static void CreateFolder(string GroupName)
        {
            string localFolder = $"{ appCacheFolder }" + $@"\{GroupName}";

            try
            {
                if (!Directory.Exists(localFolder))
                {
                    Directory.CreateDirectory(localFolder);
                    DirectoryInfo dirRest = new(localFolder);
                    dirRest.Attributes = FileAttributes.Hidden;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"{ex}");
                MessageBox.Show($"Error occurred on creating directory '{GroupName}' to store as images in cache. Check disk space or system access permissions.", "Cache Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ReadOnly(string FileName)
        {
            try
            {
                // Create a new FileInfo object.
                FileInfo fInfo = new(FileName);
                fInfo.Attributes = FileAttributes.Hidden;
                fInfo.IsReadOnly = true;
            }
            catch
            {
                MessageBox.Show("Error when trying to take the image as read-only. Check the permission of the system user and try again.", "Error Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }

}
