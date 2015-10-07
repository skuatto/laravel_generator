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
        private static string rutaController = @"\app\Controllers\";
        private static string rutaControllerAdmin = @"\app\Controllers\admin\";

        private static string plantillaController = 
        @"<?php
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

        

        public static void writeControllerFile(XmlModel model)
        {
            foreach ( var elemento in model.elementos)
            {
                string programacionCampo = "";
                List<Campo> listaText = new List<Campo>();
                List<Campo> listaFile = new List<Campo>();
                List<Campo> listaData = new List<Campo>();
                foreach (var campo in elemento.campos)
                {
                    switch (campo.tipo)
                    {
                        case "text": listaText.Add(campo);
                            break;
                        case "file": listaFile.Add(campo);
                            break;
                        case "data": listaData.Add(campo);
                            break;
                    }
                }
                if (listaText.Count > 0)
                {
                  
                    string programacionCampoTexto = "";
                    foreach (Campo text in listaText)
                    {
                        programacionCampoTexto += String.Format(
                            @"case '{0}':
								${1} = \Idioma::find($idIdioma[1]);
								${1}->texto_que_hacemos = $input_valor;
								${1}->save();	
							break;", text.nombre.ToLower(), model.nombre.ToLower());
                    }

                    programacionCampo += String.Format(
                        @"foreach (Input::all() as $input => $input_valor ) {{
				            if(preg_match(""/^[^_]/"",$input)){{
					            //Si el texto no esta vacio le hacemos update
					            if(trim($input_valor) != ""){{
						            $idIdioma = explode('_', $input);
						            switch($idIdioma[0]){{
                                        {0}
                                    }}
                                }}
                            }}
                        }}", programacionCampoTexto);


                }
                /*
                
                            case "text":    programacionCampo + = String.Format(@"",);
                                            
                 */

                write(model.nombre + rutaControllerAdmin + elemento.nombre + "Controller.php", String.Format(plantillaController, elemento.nombre));
            }
        }

        private static void write(string path,string text)
        {
            File.WriteAllText(path, text);
        }
    }
}
