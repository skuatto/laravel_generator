﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{
    class Program
    {
        static void Main(string[] args)
        {

            
            /*
             var model = XmlReader.LeerXML("config.xml");
            Console.WriteLine(model.nombre);
            Console.WriteLine(model.ruta);
            
            model.idiomas.ToList().ForEach(x => { Console.WriteLine(" " + x.nombre); });
            model.elementos.ToList().ForEach(x => 
                {
                    Console.Write(x.nombre + " => ");
                    x.campos.ForEach(y => { Console.WriteLine("editable: " + y.editable.ToString() + " nombre: " + y.nombre + " tipo: " + y.tipo); });
                }
            );
            model.menus.ToList().ForEach(x =>
            {
                Console.Write(x.nombre + " => ");
                x.submenu.ForEach(y => { Console.WriteLine(" nombre: " + y.nombre ); });
            }
           );
             
           */
            
            var model = XmlReader.LeerXML("config.xml");
            Unzip.Extract("lvl4_base_multi_idoma.zip", model.Ruta, model.Nombre.ToLower() + "\\");
           
            CreateLaravelFiles.WriteFiles(model);
            ModifyFiles.ModifyAllFiles(model);
            //ModifyFiles.ModifyAuthController(model, model.Elementos.FirstOrDefault());
            
            
            ConsoleCommand.Execute(new List<String> { "cd " + model.Ruta + model.Nombre + "\\", @"composer dump-autoload" });
            
        }
    }
}
