using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{

    public class CreateLaravelFiles
    {
        
        private static string plantillaController = @"<?php
        class {0}Controller extends Controller {{
	      
	        protected function setupLayout()
	        {{
		        if ( ! is_null($this->layout))
		        {{
			        $this->layout = View::make($this->layout);
		        }}
	        }}

        }}
?>";



        public static void writeControllerFile(string path, string controladorNombre)
        {
            write(path + controladorNombre + "Controller.php", String.Format(plantillaController, controladorNombre));
        }

        private static void write(string path,string text)
        {
            File.WriteAllText(path, text);
        }
    }
}
