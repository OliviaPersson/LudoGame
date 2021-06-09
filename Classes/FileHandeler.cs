using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace LudoGame.Classes
{
    /// <summary>
    /// A class for
    /// </summary>
    public class FileList
    {
        public string FileType { get; set; }
        public File[] Files { get; set; }
    }

    public class File
    {
        public string FileName { get; set; }
        public string NameID { get; set; }
    }

    internal static class FileHandeler
    {
        /// <summary>
        /// Handles the reading of image files from a json FileList
        /// </summary>
        /// <param name="sender">the canvas that the images are loaded to</param>
        /// <param name="assetLocation">The name of the folder within the assets folder</param>
        /// <returns>A dictionary containing the images with a nameID</returns>
        public static async Task<Dictionary<string, CanvasBitmap>> LoadImages(CanvasAnimatedControl sender, string assetLocation)
        {
            Dictionary<string, CanvasBitmap> result = new Dictionary<string, CanvasBitmap>();
            try
            {
                StorageFolder storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync($"Assets\\{assetLocation}");
                FileList fileList = await ParseLoadAssetsJson(storageFolder);
                // if the file doesn't state the correct FileType don't try to load any files
                if (fileList.FileType != "CanvasBitmap")
                {
                    return null;
                }
                foreach (File file in fileList.Files)
                {
                    result.Add(file.NameID, await CanvasBitmap.LoadAsync(sender, new Uri($"ms-appx:///Assets/{assetLocation}/{file.FileName}")));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Handles the reading of sound files from a json FileList
        /// </summary>
        /// <param name="assetLocation">The name of the folder within the assets folder</param>
        /// <returns>A dictionary containing the sounds with a nameID</returns>
        public static async Task<Dictionary<string, MediaPlayer>> LoadSounds(string assetLocation)
        {
            Dictionary<string, MediaPlayer> result = new Dictionary<string, MediaPlayer>();
            try
            {
                StorageFolder storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync($"Assets\\{assetLocation}");
                FileList fileList = await ParseLoadAssetsJson(storageFolder);
                // if the file doesn't state the correct FileType don't try to load any files
                if (fileList.FileType == "MediaSource")
                {
                    return null;
                }
                foreach (File file in fileList.Files)
                {
                    result.Add(file.NameID, new MediaPlayer() { Source = MediaSource.CreateFromStorageFile(await storageFolder.GetFileAsync(file.FileName)) });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Not yet implemented save game method
        /// </summary>
        public static void Save()
        {
        }

        /// <summary>
        /// Not yet implemented load game method
        /// </summary>
        public static void Load()
        {
        }

        private static async Task<FileList> ParseLoadAssetsJson(StorageFolder storageFolder)
        {
            StorageFile jsonFile = (StorageFile)await storageFolder.TryGetItemAsync("LoadAssets.json");

            string jsonString = await FileIO.ReadTextAsync(jsonFile);
            FileList fileList = JsonSerializer.Deserialize<FileList>(jsonString);
            return fileList;
        }
    }
}