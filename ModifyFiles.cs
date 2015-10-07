using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{
    public class ModifyFiles
    {
        private static string rutaDatabase = @"app\config\database.php";
        private static string rutaApp = @"\app\config\app.php";
        private static string rutaRoutes = @"\app\routes.php";


        public static void ModifyDatabase(string ruta, string database)
        {
            ModifyLine(ruta + '\\' + rutaDatabase, String.Format(@"            'database'  => '{0}',", database), 58);
        }
        private static void ModifyLine(string path, string text, int line)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            lines[line-1] = text;
            System.IO.File.WriteAllLines(path, lines);
        }

        private static void AppendLines(string path, string text, int line)
        {
            string[] lines = System.IO.File.ReadAllLines(path);

            var allLines = lines.Take(line - 1).ToList();
            allLines.Add(text);
            allLines.AddRange(lines.Skip(line - 1));

            System.IO.File.WriteAllLines(path, allLines);
        }
    }
}
