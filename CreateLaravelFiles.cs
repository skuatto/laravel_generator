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

        private static string plantillaViewAdminEdit =
          @"@extends('admin._layouts.inside')
            @section('js_ready')
               {2}

            @stop
            @section('titulo')
                {1}
            @stop
            @section('breadcumb')

            @stop

            @section('submain')

                {{{{ Notification::showAll() }}}}

                @if ($errors->any())
                <div class=""alert alert-error"">
                    {{{{ implode('<br>', $errors->all()) }}}}
                </div>
                @endif

                {3}

                 {{{{ Form::open(array('method' => 'post', 'files' => true)) }}}}

               {4}
                <div class=""contenidoEditable"">
                    <div class=""row-fluid"">
                        <div class=""span10"">
                             {5}
                            <div class=""form-actions"">
                                {{{{ Form::submit('Guardar', array('class' => 'btn btn-primary')) }}}}
                                <a href=""{{ URL::route('admin.{0}.edit') }}}}"" class=""btn"">Cancelar</a>
                            </div>
                        </div>
                    </div>
                </div>
                {{{{ Form::close() }}}}

            @stop
        ";

        private static string plantillaModel =
              @"<?php

                class {0} extends Eloquent {{
	                protected $table = '{1}';
                    {2}
                }}

        ?>";

        private static string plantillaValidator =
        @"<?php namespace App\Services\Validators;
 
            class {0}Validator extends Validator {{
 
                public static $rules = array(
        
                );
 
            }}
        ?>";
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
        private static string plantillaViewAdminShow =
        @"@extends('admin._layouts.default')

        @section('main')

        @stop
        ";
        private static string plantillaViewAdminIndex = @"
        @extends('admin._layouts.inside')
@section('js_ready')
    $("".tab"").each(function(i) {{
        idTextarea = $(this).find('.ckeditor_textarea').attr('id');
        CKEDITOR.replace(idTextarea, {{
                height: '270px',
                 stylesSet: [
                                {{ name: 'Subtitulo', element: 'h2' }}
                            ],
            }} );
        if(i!=0) $(this).hide();
});
	$(""form[data-confirm]"").submit(function()
{{
        if (!confirm($(this).attr(""data-confirm"")))
        {
            return false;
        }
    }});
   
    
@stop
@section('titulo')
    Portfolios
@stop
@section('breadcumb')
    <ul class=""breadcrumb"">
        <li><a href = ""{{{{ \URL::route('admin.portfolio-general.edit') }}}}"" > Portfolio General</a> <span class=""divider"">/</span></li>
        <li class=""active"">Portfolios</li>
    </ul>
@stop
@section('submain')

{ { Notification::showAll() } }

    @if($errors->any())
    <div class=""alert alert-error"">
        {{{{ implode('<br>', $errors->all()) }}}}
    </div>
    @endif
    <div style=""margin-bottom: 20px;"">
		<a href = ""{{{{ URL::route('admin.portfolio.create') }}}}"" class=""btn btn-success""><i class=""icon-plus-sign""></i> Crear un nuevo portfolio</a>
     	</div>

			<div class=""box"">
				<div class=""box-content nopadding"">
					<table class=""table table-hover table-nomargin table-bordered"">
						<thead>
							<tr>
								<th>Nombre</th>
								<th>Texto</th>
								<th style = ""width:270px;"" > Acciones </ th >
                            </ tr >
                        </ thead >
                        < tbody >
                            @foreach($portfolios as $portfolio)
                            < tr >
                                < td >

                                    < a href=""{{{{ URL::route('admin.portfolio.edit', $portfolio->id) }}}}"">{{{{ $portfolio->idiomas()->catalan()->get()->first()->pivot->titulo }}}}</a>
									
								</td>
								<td>{{{{ \Helper::cortarCadena($portfolio->idiomas()->catalan()->get()->first()->pivot->descripcion_corta, 100,'...') }}}}</td>
								
								
								<td >
									<!--
                                    @if(!$portfolio->isAlone())
                                        @if($portfolio->isFirst())
											<a href = ""{{{{ URL::route('admin.portfolio.ordenarAbajo', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-down""></i></a>
                                        @elseif($portfolio->isLast())
											<a href = ""{{{{ URL::route('admin.portfolio.ordenarArriba', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-up""></i></a>
										@else
                                            <a href=""{{{{ URL::route('admin.portfolio.ordenarArriba', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-up""></i></a>
											<a href = ""{{{{ URL::route('admin.portfolio.ordenarAbajo', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-down""></i></a>
										@endif
                                    @endif
									-->
									<a href = ""{{{{ URL::route('admin.portfolio.edit', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-edit""></i>&nbsp;Editar&nbsp;</a>
                                    @if($portfolio->isActivo())
										 <a href = ""{{{{ URL::route('admin.portfolio.publicar', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-eye-open""></i> &nbsp;Esconder&nbsp;</a>
									@else
                                         <a href=""{{{{ URL::route('admin.portfolio.publicar', $portfolio->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-eye-close""></i> &nbsp;Publicar&nbsp;</a>
									@endif
									{{{{ Form::open(array('route' => array('admin.portfolio.destroy', $portfolio->id), 'method' => 'delete', 'data-confirm' => 'Estas seguro?')) }}}}
										<button type = ""submit"" class=""btn btn-danger btn-mini""><i class=""icon-remove-sign""></i>&nbsp;Eliminar&nbsp;</button>
									{{{{ Form::close() }}}}

								</td>
							</tr>
							@endforeach
                        </tbody>
					</table>
					{{{{ $portfolios->links() }}}}
				</div>
			</div>

  
@stop
";
 

        public static void WriteFiles(XmlModel model)
        {
            int i = 1;
            foreach (var elemento in model.Elementos)
            {
                WriteControllerFile(model, elemento, i);
                WriteValidatorFile(model, elemento);
                WriteModelFile(model, elemento);
                WriteEditViewFile(model, elemento);
                i++;
            }
        }

        public static void WriteControllerFile(XmlModel model, Elemento elemento, int posicion)
        {
            string programacionCampo = "";
                
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
                        case "file": listaFile.Add(campo);
                            break;
                        case "data": listaData.Add(campo);
                            break;
                    }
                }
                if (listaText.Count > 0)
                {

                    string programacionCampoTexto = "";
                    if(model.Idiomas.Count > 0)
                    {
                        foreach (Campo text in listaText)
                        {
                            programacionCampoTexto += String.Format(
                                @"case '{0}':
								    ${1} = \Idioma::find($idIdioma[1]);
								    ${1}->{2} = $input_valor;
								    ${1}->save();	
							    break;", text.Nombre.ToLower(), model.Nombre.ToLower(), text.Nombre.ToLower() + '_' + elemento.Nombre.ToLower());
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
                            }}", programacionCampoTexto);
                    }
                }

                Write(model.Ruta + model.Nombre + '\\' + rutaControllerAdmin + elemento.Nombre + "Controller.php", String.Format(plantillaController, elemento.Nombre, elemento.Nombre.ToLower(), posicion, programacionCampo));
        }

        public static void WriteValidatorFile(XmlModel model, Elemento elemento)
        {
            Write(model.Ruta + model.Nombre + rutaValidator + elemento.Nombre + "Validator.php", String.Format(plantillaValidator, elemento.Nombre));
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
            Write(model.Ruta + model.Nombre + rutaModel + elemento.Nombre + ".php", String.Format(plantillaModel, elemento.Nombre, elemento.Nombre.ToLower(), codigoModelo));
        }

        public static void WriteEditViewFile(XmlModel model, Elemento elemento)
        {
            string scriptCode = "", tabCode = "", textCode = "";
            if(model.Idiomas.Count > 0)
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
            
            List<Campo> listaCamposMultiIdioma = elemento.Campos.Select(x => new Campo{ Descripcion = x.Descripcion, Nombre = x.Nombre, Editable = x.Editable, Tipo = x.Tipo, MultiIdioma = x.MultiIdioma}).Where(x => x.MultiIdioma == true && x.Editable == true).ToList();
            List<Campo> listaCamposNoMultiIdioma = elemento.Campos.Select(x => new Campo { Descripcion = x.Descripcion, Nombre = x.Nombre, Editable = x.Editable, Tipo = x.Tipo, MultiIdioma = x.MultiIdioma }).Where(x => !x.MultiIdioma && x.Editable).ToList();
            foreach ( var campo in  listaCamposMultiIdioma )
            {
               switch (campo.Tipo)
               {
                   case "textarea": textCode = String.Format(@"
                                     @for( $i=0; $i < sizeof($idiomas); $i++)
                                        <div class=""tab control-group"" id=""tab_{{{{ $i }}}}"">
                                            {{{{ Form::label('{0}', '{1}: ') }}}}
                                            <div class=""controls"">
                                                {{{{ Form::textarea('{0}_'.$idiomas[$i]->id, $idiomas[$i]->{2}, array('class' => 'ckeditor_textarea', 'id' => '{0}_'.$idiomas[$i]->id) ) }}}}
                                            </div>
                                        </div>
                                    @endfor
                                    ", campo.Nombre.ToLower(), campo.Descripcion, campo.Nombre.ToLower() + '_' + elemento.Nombre.ToLower());
                                    break;
                   case "int":
                    case "text": textCode = String.Format(@"
                                    @for( $i=0; $i < sizeof($idiomas); $i++)
                                    <div class=""tab control-group"" id=""tab_{{ $i }}}}"">
                                        {{{{ Form::label('{0}', '{1}: ') }}
                                        <div class=""controls"">
                                            {{{{ Form::text('{0}_'.$idiomas[$i]->id, $idiomas[$i]->{2}, array('class' => 'ckeditor_textarea', 'id' => '{0}_'.$idiomas[$i]->id) ) }}}}
                                        </div>
                                    </div>
                                @endfor
                                ", campo.Nombre.ToLower(), campo.Descripcion, campo.Nombre.ToLower() + '_' + elemento.Nombre.ToLower());
                                break;
               }
                 
            }
            foreach (var campo in listaCamposNoMultiIdioma)
            {
                switch (campo.Tipo)
                {
                    case "int":
                    case "text": textCode = String.Format(@"{{{{ Form::label('{0}', '{1}: ') }}}}
                            <div class=""controls"">
                                    {{{{ Form::text('_{0}',${2}->{0}, array( 'class' => 'input', 'id' => '{0}') ) }}}}
                            </div>", campo.Nombre.ToLower(),campo.Descripcion, elemento.Nombre.ToLower());
                        break;
                    case "imagen": textCode = String.Format(@"
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
                                     </div>", campo.Nombre.ToLower(),campo.Descripcion, elemento.Nombre.ToLower());
                                break;
                    case "data": textCode = String.Format(@"
                                {{{{ Form::label('{0}', '{1}: ') }}}}
                                <div class=""controls"">
                                   {{{{ Form::text('{0}',\Helper::mostrarFechaEditar(${2}->{0}), array( 'class' => 'input-medium datepick', 'id' => '{0}') ) }}}}
                                </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower());
                                break;
                  
                }
            }

            // Falta generar todas las migas de pan
            string rutaControlador = (elemento.Singular) ? "edit" : "index";
            string breadcumb = "";
            //El boton debe mostrar si este elemento contiene hijos
            /*
            string breadcumb = String.Format(
                @"<div style=""margin-bottom: 20px;"">
                    <a href=""{{{{ URL::route('admin.{0}.{2}') }}}}"" class=""btn btn-success""><i class=""icon-zoom-in""></i>&nbsp;{1}</a>
                </div>", elemento.Nombre.ToLower(), elemento.Descripcion, rutaControlador);
            */

            System.IO.Directory.CreateDirectory(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower());
            Write(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + "/edit.blade.php", String.Format(plantillaViewAdminEdit, elemento.Nombre.ToLower(), elemento.Descripcion, scriptCode, breadcumb, tabCode, textCode));
        }

        public static void WriteCreateViewFile(XmlModel model, Elemento elemento)
        {
            string scriptCode = "", tabCode = "", textCode = "";
            if (model.Idiomas.Count > 0)
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

            List<Campo> listaCamposMultiIdioma = elemento.Campos.Select(x => new Campo { Descripcion = x.Descripcion, Nombre = x.Nombre, Editable = x.Editable, Tipo = x.Tipo, MultiIdioma = x.MultiIdioma }).Where(x => x.MultiIdioma == true && x.Editable == true).ToList();
            List<Campo> listaCamposNoMultiIdioma = elemento.Campos.Select(x => new Campo { Descripcion = x.Descripcion, Nombre = x.Nombre, Editable = x.Editable, Tipo = x.Tipo, MultiIdioma = x.MultiIdioma }).Where(x => !x.MultiIdioma && x.Editable).ToList();
            foreach (var campo in listaCamposMultiIdioma)
            {
                switch (campo.Tipo)
                {
                    case "textarea": textCode = String.Format(@"
                                     @for( $i=0; $i < sizeof($idiomas); $i++)
                                        <div class=""tab control-group"" id=""tab_{{{{ $i }}}}"">
                                            {{{{ Form::label('{0}', '{1}: ') }}}}
                                            <div class=""controls"">
                                                {{{{ Form::textarea('{0}_'.$idiomas[$i]->id, $idiomas[$i]->{2}, array('class' => 'ckeditor_textarea', 'id' => '{0}_'.$idiomas[$i]->id) ) }}}}
                                            </div>
                                        </div>
                                    @endfor
                                    ", campo.Nombre.ToLower(), campo.Descripcion, campo.Nombre.ToLower() + '_' + elemento.Nombre.ToLower());
                        break;
                    case "int":
                    case "text": textCode = String.Format(@"
                                    @for( $i=0; $i < sizeof($idiomas); $i++)
                                    <div class=""tab control-group"" id=""tab_{{ $i }}}}"">
                                        {{{{ Form::label('{0}', '{1}: ') }}
                                        <div class=""controls"">
                                            {{{{ Form::text('{0}_'.$idiomas[$i]->id, $idiomas[$i]->{2}, array('class' => 'ckeditor_textarea', 'id' => '{0}_'.$idiomas[$i]->id) ) }}}}
                                        </div>
                                    </div>
                                @endfor
                                ", campo.Nombre.ToLower(), campo.Descripcion, campo.Nombre.ToLower() + '_' + elemento.Nombre.ToLower());
                        break;
                }

            }
            foreach (var campo in listaCamposNoMultiIdioma)
            {
                switch (campo.Tipo)
                {
                    case "int":
                    case "text": textCode = String.Format(@"{{{{ Form::label('{0}', '{1}: ') }}}}
                            <div class=""controls"">
                                    {{{{ Form::text('_{0}',${2}->{0}, array( 'class' => 'input', 'id' => '{0}') ) }}}}
                            </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower());
                        break;
                    case "imagen": textCode = String.Format(@"
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
                                     </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower());
                        break;
                    case "data": textCode = String.Format(@"
                                {{{{ Form::label('{0}', '{1}: ') }}}}
                                <div class=""controls"">
                                   {{{{ Form::text('{0}',\Helper::mostrarFechaEditar(${2}->{0}), array( 'class' => 'input-medium datepick', 'id' => '{0}') ) }}}}
                                </div>", campo.Nombre.ToLower(), campo.Descripcion, elemento.Nombre.ToLower());
                        break;

                }
            }

            // Falta generar todas las migas de pan
            string rutaControlador = (elemento.Singular) ? "edit" : "index";
            string breadcumb = "";
            //El boton debe mostrar si este elemento contiene hijos
            /*
            string breadcumb = String.Format(
                @"<div style=""margin-bottom: 20px;"">
                    <a href=""{{{{ URL::route('admin.{0}.{2}') }}}}"" class=""btn btn-success""><i class=""icon-zoom-in""></i>&nbsp;{1}</a>
                </div>", elemento.Nombre.ToLower(), elemento.Descripcion, rutaControlador);
            */

            System.IO.Directory.CreateDirectory(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower());
            Write(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + "/edit.blade.php", String.Format(plantillaViewAdminEdit, elemento.Nombre.ToLower(), elemento.Descripcion, scriptCode, breadcumb, tabCode, textCode));
        }

        public static void WriteShowViewFile(XmlModel model, Elemento elemento)
        {
            System.IO.Directory.CreateDirectory(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower());
            Write(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + "/show.blade.php", String.Format(plantillaViewAdminEdit, elemento.Nombre.ToLower(), elemento.Descripcion, scriptCode, breadcumb, tabCode, textCode));
        }
        public static void WriteIndexViewFile(XmlModel model, Elemento elemento)
        {
            System.IO.Directory.CreateDirectory(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower());
            Write(model.Ruta + model.Nombre + rutaViewAdmin + elemento.Nombre.ToLower() + "/index.blade.php", String.Format(plantillaViewAdminEdit, elemento.Nombre.ToLower(), elemento.Descripcion, scriptCode, breadcumb, tabCode, textCode));
        }

        private static void Write(string path,string text)
        {
            System.IO.File.WriteAllText(path, text);
        }
    }
}