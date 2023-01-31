using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using ConfiApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ConfiApp.Modelos;
using System.Threading.Tasks;
using System.IO;
using Java.IO;
using Android.Graphics;
using Plugin.CurrentActivity;
using Android.Graphics.Drawables;
using Android.Provider;

[assembly: Dependency(typeof(SaveAndroid))]
namespace ConfiApp.Droid
{
    class SaveAndroid : ISave
    {
        public static Activity activity { get; set; }
        [Obsolete]
        public async System.Threading.Tasks.Task<byte[]> CaptureAsync()
        {
            var activity1 = Forms.Context as Activity;

            var view = activity1.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);

            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;




        }

        [Obsolete]
        public void Save(string fileName, String contentType, MemoryStream s)
        {
            string root = null;

            root = Xamarin.Essentials.FileSystem.AppDataDirectory;

            Java.IO.File myDir = new Java.IO.File(root + "/Syncfusion");
            myDir.Mkdir();

            Java.IO.File file = new Java.IO.File(myDir, fileName);

            if (file.Exists()) file.Delete();

            try
            {
                FileOutputStream outs = new FileOutputStream(file);
                outs.Write(s.ToArray());

                outs.Flush();
                outs.Close();

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);

            }
            App.Current.Properties["RutaArchivo"] = file.Path.ToString();
            //if (file.Exists())
            //{
            //    Android.Net.Uri path = Android.Net.Uri.FromFile(file);
            //    string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
            //    string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
            //    Intent intent = new Intent(Intent.ActionView);
            //    intent.SetDataAndType(path, mimeType);
            //    Forms.Context.StartActivity(Intent.CreateChooser(intent, "Choose App"));
            //}
        }

    }
}