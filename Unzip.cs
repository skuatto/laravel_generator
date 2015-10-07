using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{
    public class Unzip
    {
        static string fileName = "lvl4_base_multi_idoma";

        public static void Extract(string nombeArchivo, string rutaExtraer)
        {
            string zipToUnpack = nombeArchivo;
            string unpackDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(unpackDirectory + rutaExtraer))
            {
                using (ZipFile zip = ZipFile.Read(zipToUnpack))
                {
                    // here, we extract every entry, but we could extract conditionally
                    // based on entry name, size, date, checkbox status, etc.  
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                Directory.Move(unpackDirectory + fileName + @"\", unpackDirectory + rutaExtraer);

            }
        }
    }
}
