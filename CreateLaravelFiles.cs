using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{

    public class CreateLaravelFiles
    {
        private static string rutaController = @"\app\Controllers\";
        private static string rutaControllerAdmin = @"\app\controllers\admin\";
        private static string rutaValidator = @"\app\Services\Validators\";
        private static string rutaModel = @"\app\models\";
        private static string rutaViewAdmin = @"\app\views\admin\";

     

        public static void WriteFiles(XmlModel model)
        {
            int i = 1;
            foreach( var elemento in model.Elementos )
            {
                WriteControllerFile(model, elemento, i);
                WriteValidatorFile(model, elemento);
                WriteModelFile(model, elemento);
                WriteEditViewFile( model, elemento);
                if( elemento.Elementos.Count > 0 )
                {
                    int e = 1;
                    foreach ( var elementoHijo in elemento.Elementos )
                    {
                        WriteControllerFile( model, elementoHijo, e, elemento );
                        WriteValidatorFile( model, elementoHijo );
                        WriteModelFile( model, elementoHijo );
                        WriteEditViewFile( model, elementoHijo, elemento );
                        WriteCreateViewFile( model, elementoHijo, elemento );
                    }
                    e++;
                }
                i++;
            }
        }

        public static void WriteControllerFile(XmlModel model, Elemento elemento, int posicion, Elemento elementoPadre = null)
        {
            string programacionCampo = "";
            string programacionCampoTexto = "";
            List<Campo> listaText = new List<Campo>();
            List<Campo> listaTextArea = new List<Campo>();
            List<Campo> listaFile = new List<Campo>();
            List<Campo> listaData = new List<Campo>();
            foreach (var campo in elemento.Campos)
            {
                switch (campo.Tipo)
                {
                    case "textarea": 
                    case "text": listaText.Add(campo);
                        break;
                    case "pdf":
                    case "image": listaFile.Add(campo);
                        break;
                    case "data": listaData.Add(campo);
                        break;
                }
            }
            if(elementoPadre != null)
            {
                programacionCampo += String.Format(
                @"${0} = new \{1};
				${0}->orden = \DB::table('{1}')->max('orden') + 1;
				${0}->activo = 1;
                $idiomas = array();  
                ", elemento.Nombre.ToLower(), elemento.Nombre);

            }
            if ( listaFile.Count > 0 )
            {

                foreach ( Campo campo in listaFile )
                {
                    programacionCampoTexto += String.Format(
                    @"if(Input::hasFile('_{0}'))
				    {{
		        	    ${1}->src = Image::upload(Input::file('_{0}'), ${1}->id, '{1}');
			            ${1}->save();
		    	    }}", campo.Nombre.ToLower(), elemento.Nombre.ToLower() );
                }
            }
            else
            {
                programacionCampoTexto += String.Format( @"${0}->save();", elemento.Nombre.ToLower() );
            }

            if ( listaText.Count > 0)
            {
               
                if(model.Idiomas.Count > 0)
                {
                    if ( elementoPadre == null )
                    {
                        foreach ( Campo text in listaText )
                        {
                            programacionCampoTexto += String.Format(
                                @"case '{0}':
								${1} = \Idioma::find($idIdioma[1]);
								${1}->{2} = $input_valor;
								${1}->save();	
							break;", text.Nombre.ToLower(), model.Nombre.ToLower(), text.Nombre.ToLower() + '_' + elemento.Nombre.ToLower() );
                        }

                        programacionCampo += String.Format(
                            @"foreach (Input::all() as $input => $input_valor ) {{
				            if(preg_match(""/^[^_]/"",$input)){{
					            //Si el texto no esta vacio le hacemos update
					            if(trim($input_valor) != """"){{
						            $idIdioma = explode('_', $input);
						            switch($idIdioma[0]){{
                                        {0}
                                    }}
                                }}
                            }}
                        }}", programacionCampoTexto );
                    }
                    else
                    {
                        string programacionCampoIdioma = "";
                        foreach ( Campo text in listaText )
                        {
                            programacionCampoTexto += String.Format(
                                @"case '{0}':
								${1} =  $idiomas[$idIdioma[1]]['{1}'] = $input_valor;
							    break;", text.Nombre.ToLower(), model.Nombre.ToLower(), text.Nombre.ToLower() + '_' + elemento.Nombre.ToLower() );
                            programacionCampoTexto += String.Format(
                                @"'{0}' => $idioma['{0}'],
							    ", text.Nombre.ToLower() );
                        }

                        programacionCampo += String.Format(
                            @"foreach (Input::all() as $input => $input_valor ) {{
				                if(preg_match(""/^[^_]/"",$input)){{
                                    $idIdioma = explode('_', $input);
                                    $idiomas[$idIdioma[1]]['id'] = $idIdioma[1];
						            switch($idIdioma[0]){{
                                        {1}
                                    }}
                                }}
                            }}
                            foreach($idiomas as $idioma){{
					            ${0}->idiomas()->attach(  $idioma['id'], array( {2} ));
				            }}
                            ", elemento.Nombre.ToLower(), programacionCampoTexto, programacionCampoIdioma );
                    }
                }

            }
      
            Write(model.Ruta + model.Nombre + '\\' + rutaControllerAdmin + elemento.Nombre + "Controller.php", String.Format( LaravelCodeStrings.PlantillaController, elemento.Nombre, elemento.Nombre.ToLower(), posicion, programacionCampo));
        }

        public static void WriteValidatorFile(XmlModel model, Elemento elemento )
        {
            Write(model.Ruta + model.Nombre + rutaValidator + elemento.Nombre + "Validator.php", String.Format( LaravelCodeStrings.PlantillaValidator, elemento.Nombre));
        }

        public static void WriteModelFile(XmlModel model, Elemento elemento)
        {
            string codigoModelo = "";

            if (model.Idiomas.Count() > 0)
            {
                codigoModelo += @"public function idiomas()
	                {{
		                 return $this->hasMany('Idioma');
	                }}
	                public function idioma()
                    {{
                        return $this->hasOne('Idioma')
                            ->codigo(Session::get('locale'));
                    }}";
            }
           
            if (elemento.Campos.Select(x => new { Tipo = x.Tipo }).Where(x => x.Tipo == "imagen").Count() > 0)
            {
                codigoModelo += @"
	                public function imagenes()
	                {{
		                 return $this->hasOne('Imagen');
	                }}
	              
                    public function imagen(){{
                         return $this->hasOne('Imagen','que_hacemos_id');
                    }}";
            }
            if (!elemento.Singular)
            {
                codigoModelo += @"public function scopeOrdenar($query, $orden = 'ASC')
                     {{
                         return $query->orderby('orden', $orden);
                     }}

                     public function scopeActivo($query)
                     {{
                         return $query->where('activo', '=' , 1);
                     }}

                     public function scopeWhereOrden($query,$orden)
                     {{
                         return $query->where('orden', '=' , $orden);
                     }}
    
                     public function scopeWhereOrdenMayor($query,$orden)
                     {{
                         return $query->where('orden', '>' , $orden);
                     }}
    
                     public function scopeWhereOrdenMenor($query,$orden)
                     {{
                         return $query->where('orden', '<' , $orden);
                     }}
                     //Funciones de comprobacion
                     public function isActivo()
                     {{
                         if($this->activo == 1)
                             return true;
                         else
                             return false;
                     }}

                     public function isFirst()
                     {{
                         if($this->orden == $this->min('orden'))
                             return true;
                         else
                             return false;
                     }}
                     public function isLast()
                     {{
                         if($this->orden == $this->max('orden'))
                             return true;
                         else
                             return false;
                     }}
                     public function isAlone()
                     {{
                         if($this->count() <= 1)
                             return true;
                         else
                             return false;

                     }}";
            }
            Write(model.Ruta + model.Nombre + rutaModel + elemento.Nombre + ".php", String.Format( LaravelCodeStrings.PlantillaModel, elemento.Nombre, elemento.Nombre.ToLower(), codigoModelo));
        }


        private static void WriteCreateEditViewFile( XmlModel model, Elemento elemento, bool crear, Elemento elementoPadre = null )
        {
            string textoEditarCrear = "Crear ";
            string nombreArchivo = "create.php";
            if ( !crear )
            {
                textoEditarCrear = "Editar ";
                nombreArchivo = "edit.php";
            }
            string scriptCode = "", tabCode = "", textCode = "";
            if ( model.Idiomas.Count > 0 )
            {
                scriptCode = @"
                    $("".tab"").each(function(i) {
                        idTextarea = $(this).find('.ckeditor_textarea').attr('id');
                        CKEDITOR.replace(idTextarea, {
                                forcePasteAsPlainText: true,
                                enterMode : CKEDITOR.ENTER_BR,
                                height: '400px',
                                 stylesSet: [
                                                { name: 'Subtitulo',  element: 'h2' }
                                            ],
                            } );
                        if(i!=0) $(this).hide();
                    });

                    ";
                tabCode = @"
                    <ul class=""nav nav-tabs"">
                        @for( $i=0; $i < sizeof($idiomas); $i++)
                        <li id=""nav_tab_{{ $i }}"" @if ($i==0) class=""active"" @endif>
                            <a href=""javascript:cambiarPestanya('{{ $i }}')"">{{$idiomas[$i]->nombre}}</a>
                        </li>
                        @endfor
                    </ul>
                    ";
            }

            List<Campo> listaCamposMultiIdioma = elemento.Campos.Select( x => new Campo { Descripcion = x.Descripcion, Nombre = x.Nombre, Editable = x.Editable, Tipo = x.Tipo, MultiIdioma = x.MultiIdioma } ).Where( x => x.MultiIdioma == true && x.Editable == true ).ToList();
            List<Campo> listaCamposNoMultiIdioma = elemento.Campos.Select( x => new Campo { Descripcion = x.Descripcion, Nombre = x.Nombre, Editable = x.Editable, Tipo = x.Tipo, MultiIdioma = x.MultiIdioma } ).Where( x => !x.MultiIdioma && x.Editable ).ToList();
            foreach ( var campo in listaCamposMultiIdioma )
            {
                switch ( campo.Tipo )
                {
                    case "textarea":
                        textCode = String.Format( @"
                                     @for( $i=0; $i < sizeof($idiomas); $i++)
                                        <div class=""tab control-group"" id=""tab_{{{{ $i }}}}"">
                                            {{{{ Form::label('{0}', '{1}: ') }}}}
                                            <div class=""controls"">
                                                {{{{ Form::textarea('{0}_'.$idiomas[$i]->id, $idiomas[$i]->{2}, array('class' => 'ckeditor_textarea', 'id' => '{0}_'.$idiomas[$i]->id) ) }}}}
                                            </div>
                                        </div>
                                    @endfor
                                    ", campo.Nombre.ToLower(), campo.Descripcion, campo.Nombre.ToLower() + '_' + elemento.Nombre.ToLower() );
                        break;
                    case "int":
                    case "text":
                        string valorInput = String.Format( @"$idiomas[$i]->{0}",campo.Nombre.ToLower() + '_' + elemento.Nombre.ToLower());

                        if ( elementoPadre != null )
                            valorInput = String.Format( @"${0}->idiomas()->id($idiomas[$i]->id )->get()->first()->pivot->{1}", elemento.Nombre.ToLower(), campo.Nombre.ToLower() );

                        if ( crear )
                            valorInput = @"''";

                        textCode = String.Format( @"
                                    @for( $i=0; $i < sizeof($idiomas); $i++)
                                    <div class=""tab control-group"" id=""tab_{{ $i }}}}"">
                                        {{{{ Form::label('{0}', '{1}: ') }}
                                        <div class=""controls"">
                                            {{{{ Form::text('{0}_'.$idiomas[$i]->id, {2}, array('class' => 'ckeditor_textarea', 'id' => '{0}_'.$idiomas[$i]->id) ) }}}}
                                        </div>
                                    </div>
                                @endfor
                                ", campo.Nombre.ToLower(), campo.Descripcion, valorInput );
                        
                        break;
                }

            }
            foreach ( var campo in listaCamposNoMultiIdioma )
            {
                switch ( campo.Tipo )
                {
                    case "int":
                    case "text":
                        textCode = String.Format( @"{{{{ Form::label('{0}', '{1}: ') }}}}
                            <div class=""controls"">
                                    {{{{ Form::text('_{0}',${2}->{0}, array( 'class' => 'input', 'id' => '{0}') ) }}}}
                            </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower() );
                        break;
                    //Falta acabar
                    case "imagen":
                        textCode = String.Format( @"
                                    <div class=""controls"">   
                                        {{{{ Form::label('{0}', '{1}: ') }}}}
                                        <div class=""fileupload2 fileupload2-new"" data-provides=""fileupload2"">
                                           <div class=""fileupload2-preview thumbnail"" style=""width: 200px; height: 150px;"">
                                                @if (count(${2}->imagen()->first())>0)
                                                    <a href=""{{{{ ${2}->imagen()->first()->src; }}}}""><img src=""{{{{ Image::resize(${2}->imagen()->first()->src, 200, 150); }}}}"" ></a>
                                                @else
                                                    <img src=""http://www.placehold.it/200x150/EFEFEF/AAAAAA&amp;text=sin+imagen"">
                                                @endif
                                            </div>
                                            <div>
                                                <span class=""btn btn-file""><span class=""fileupload2-new"">Selecciona una imagen</span><span class=""fileupload2-exists"">Cambiar</span>{{{{ Form::file('_{0}') }}}}</span>
                                                <a href=""#"" class=""btn fileupload2-exists"" data-dismiss=""fileupload2"">Quitar</a>
                                            </div>
                                        </div>
                                     </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower() );
                        break;
                    case "pdf":
                        string urlTexto = String.Format(@"<a href="">${0}->{1}</a>",elemento.Nombre.ToLower() ,campo.Nombre.ToLower());
                        if ( crear )
                            urlTexto = "";

                        textCode = String.Format( @"
                                    <div class=""controls"">   
                                        {{{{ Form::label('{0}', '{1}: ') }}}}
                                        <div class=""fileupload2 fileupload2-new"" data-provides=""fileupload2"">
                                            <div style=""width: 200px; height: 150px;"">
                                                {2}
                                            </div>
                                            <div>
                                                <span class=""btn btn-file""><span class=""fileupload2-new"">Selecciona un pdf</span><span class=""fileupload2-exists"">Cambiar</span>{{{{ Form::file('_{0}') }}}}</span>
                                                <a href=""#"" class=""btn fileupload2-exists"" data-dismiss=""fileupload2"">Quitar</a>
                                            </div>
                                        </div>
                                        </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower(), urlTexto );
                        break;
                    case "data":
                        textCode = String.Format( @"
                                {{{{ Form::label('{0}', '{1}: ') }}}}
                                <div class=""controls"">
                                   {{{{ Form::text('{0}',\Helper::mostrarFechaEditar(${2}->{0}), array( 'class' => 'input-medium datepick', 'id' => '{0}') ) }}}}
                                </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower() );
                        break;

                }
            }

            // Falta generar todas las migas de pan
            string rutaControlador = ( elemento.Singular ) ? "edit" : "index";
            string breadcumb = "";
            //El boton debe mostrar si este elemento contiene hijos
            if ( elementoPadre != null )
            {
                breadcumb = String.Format(
                @"<ul class=""breadcrumb"">
                    <li><a href = ""{{{{ \URL::route('admin.{0}.edit') }}}}"" > {1} </a><span class=""divider"">/</span></li>
                    <li class=""active"">{2}</li>
                </ul>", elementoPadre.Nombre.ToLower(), elementoPadre.Descripcion, elemento.Descripcion );
            }

            System.IO.Directory.CreateDirectory( model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() );
            Write( model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + nombreArchivo, String.Format( LaravelCodeStrings.PlantillaViewAdminEditCreate, elemento.Nombre.ToLower(), textoEditarCrear + elemento.Descripcion, scriptCode, breadcumb, tabCode, textCode, breadcumb ) );
        }

        public static void WriteEditViewFile( XmlModel model, Elemento elemento, Elemento elementoPadre = null )
        {
            WriteCreateEditViewFile( model, elemento, false, elementoPadre );
        }

        public static void WriteCreateViewFile( XmlModel model, Elemento elemento, Elemento elementoPadre = null )
        {
            WriteCreateEditViewFile( model, elemento, true, elementoPadre );
        }

        public static void WriteShowViewFile(XmlModel model, Elemento elemento)
        {
            System.IO.Directory.CreateDirectory(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower());
            Write(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + "/show.blade.php", String.Format( LaravelCodeStrings.PlantillaViewAdminShow, elemento.Nombre.ToLower(), elemento.Descripcion, ));
        }
        public static void WriteIndexViewFile(XmlModel model, Elemento elemento, Elemento elementoPadre )
        {
            System.IO.Directory.CreateDirectory(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower());
            Write(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + "/index.blade.php", String.Format( LaravelCodeStrings.PlantillaViewAdminIndex, elementoPadre.Nombre.ToLower(), elementoPadre.Descripcion, elemento.Nombre.ToLower(), elemento.Descripcion  ) );
        }

        private static void Write(string path,string text)
        {
            System.IO.File.WriteAllText(path, text);
        }
    }
}