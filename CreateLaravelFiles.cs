﻿using System;
using System.Collections.Generic;
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
        @"<?php namespace App\Controllers\Admin;

        use App\Services\Validators\{0}Validator;
        use Input, Notification, Sentry, Redirect, Str, View;

        class {0}Controller extends \BaseController {{

	
	        public function edit()
	        {{
		        return View::make('admin.{0}.edit')->with('idiomas', \Idioma::All())
													         ->with('{1}', \{0}::first())
													         ->with('navegador_active', {2});
	        }}

	        public function update()
	        {{
		        $validation = new {0}Validator;

		        if ($validation->passes())
		        {{   
			        {3}
			
			        Notification::success('Los cambios fueron guardados correctamente.');
			        return Redirect::route('admin.{1}.edit');
		        }}
		        return Redirect::back()->withInput()->withErrors($validation->errors);
	        }}
        }}
        ?>";


        public static void WriteFiles(XmlModel model)
        {
            int i = 1;
            foreach (var elemento in model.Elementos)
            {
                WriteControllerFile(model, elemento, i);
                i++;
            }
        }

        public static void WriteControllerFile(XmlModel model, Elemento elemento, int posicion)
        {
            string programacionCampo = "";
                
                List<Campo> listaText = new List<Campo>();
                List<Campo> listaFile = new List<Campo>();
                List<Campo> listaData = new List<Campo>();
                foreach (var campo in elemento.Campos)
                {
                    switch (campo.Tipo)
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
							break;", text.Nombre.ToLower(), model.Nombre.ToLower());
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
                Write(model.Nombre + rutaControllerAdmin + elemento.Nombre + "Controller.php", String.Format(plantillaController, elemento.Nombre, elemento.Nombre.ToLower(), posicion, programacionCampo));
        }


        private static void Write(string path,string text)
        {
            System.IO.File.WriteAllText(path, text);
        }
    }
}
