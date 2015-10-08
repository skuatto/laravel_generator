﻿using System;
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
        private static string rutaLayoutInside = @"\app\views\admin\_layouts\inside.blade.php";

        public static void ModifyDatabase(string ruta, string database)
        {
            ModifyLine(ruta + '\\' + rutaDatabase, String.Format(@"            'database'  => '{0}',", database), 58);
        }

        public static void ModifyTitulo(XmlModel model)
        {
            ModifyLine(model.Nombre + '\\' + rutaLayoutInside, String.Format(@"		<a href=""#"" id""brand"">{0}</a>", model.Descripcion.ToUpper()), 9);
        }
        
        //Cargar todo el menu
        public static void AppendMenu(XmlModel model)
        {
            string codeMenu = "";
            string textToAppend = "";
            int posicion = 1;
            foreach (var menu in model.Menus)
            {
                if (menu.Submenu.Count > 0)
                {
                    string codigoSubmenu = "";
                    foreach (SubMenu submenu in menu.Submenu)
                    {
                        codigoSubmenu += String.Format(@"       <li><a href=""{{{{ URL::route('admin.{0}.edit') }}}}"">{1}</a></li>", submenu.Nombre.ToLower(),submenu.Descripcion);
                    }
                    codeMenu = String.Format(@"
                    <a data-toggle=""dropdown"" class='dropdown-toggle' href=""#"">
				        <span>{0}</span>
				        <span class=""caret""></span>
				    </a>
				    <ul class=""dropdown-menu"">
					  {1}
				    </ul>
                    ", menu.Descripcion, codigoSubmenu);
                }
                else
                {
                    codeMenu = String.Format(@"<a href=""{{{{ URL::route('admin.{0}.edit') }}}}"">{1}</a>", menu.Nombre.ToLower(), menu.Descripcion);
                }

                textToAppend = String.Format(@"
	                <li @if($navegador_active == {0}) {1} @endif>
				        {2}
			        </li>
                    ", posicion, posicion == 1 ? "class='active'" : "", codeMenu);

                posicion++;
            }

            AppendLines(model.Nombre + '\\' + rutaLayoutInside, textToAppend, 13);
        }

        public static void ModifyApp(XmlModel model)
        {
            string idiomaText = "";
            
            if (model.Idiomas.Count() > 0)
            {
                foreach (Idioma idioma in model.Idiomas)
                {
                    idiomaText += "'" + idioma.Nombre + "', ";
                }

                ModifyLine(model.Nombre + '\\' + rutaApp, String.Format(@"	'languages' => array({0}),", idiomaText), 190);
            }
            
        }

        //Individual para cada elemento
        public static void ModifyRoute(XmlModel model, Elemento elemento)
        {
            string textToAppend = "";
            if (elemento.Singular)
            {
                textToAppend = String.Format(@"
            	    Route::get('{0}', array('as' => 'admin.{0}.edit', 'uses' => 'App\Controllers\Admin\{1}Controller@edit'));
	                Route::post('{0}', array('as' => 'admin.{0}.update', 'uses' => 'App\Controllers\Admin\{1}Controller@update'));

                ", elemento.Nombre.ToLower(), elemento.Nombre);
            }
            else
            {
                textToAppend = String.Format(@"
            	    Route::resource('{0}',       'App\Controllers\Admin\{1}Controller');
	                Route::get('{0}/publicar/{{id_{0}}}', array('as' => 'admin.{0}.publicar', 'uses' => 'App\Controllers\Admin\{1}Controller@publicar'));
	                Route::get('{0}/ordenarArriba/{{id_{0}}}', array('as' => 'admin.{0}.ordenarArriba', 'uses' => 'App\Controllers\Admin\{1}Controller@ordenarArriba'));
	                Route::get('{0}/ordenarAbajo/{{id_{0}}}', array('as' => 'admin.{0}.ordenarAbajo', 'uses' => 'App\Controllers\Admin\{1}Controller@ordenarAbajo'));

                ", elemento.Nombre.ToLower(), elemento.Nombre);
            }
            AppendLines(model.Nombre + '\\' + rutaRoutes, textToAppend, 36);
        }

        private static void ModifyLine(string path, string text, int line)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            lines[line-1] = text;
            System.IO.File.WriteAllLines(path, lines);
        }

        private static void ModifyLine(string path, LiniaTexto liniaTexto)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            lines[liniaTexto.Linia - 1] = liniaTexto.Texto;
            System.IO.File.WriteAllLines(path, lines);
        }

        private static void ModifyLine(string path, List<LiniaTexto> liniasTexto)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach ( var liniaTexto in liniasTexto)
            {
                lines[liniaTexto.Linia - 1] = liniaTexto.Texto;
            }
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

        public class LiniaTexto
        {
            public string Texto { get; set; }
            public int Linia { get; set; }
        }

    }
}