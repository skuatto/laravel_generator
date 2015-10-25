using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{
    public static class LaravelCodeStrings
    {
        public static string PlantillaViewAdminEditCreate =
       @"@extends('admin._layouts.inside')
            @section('js_ready')
               {2}

            @stop
            @section('titulo')
                {1}
            @stop
            @section('breadcumb')
                {6}
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

        public static string PlantillaModel =
              @"<?php

                class {0} extends Eloquent {{
	                protected $table = '{1}';
                    {2}
                }}

        ?>";

        public static string PlantillaValidator =
        @"<?php namespace App\Services\Validators;
 
            class {0}Validator extends Validator {{
 
                public static $rules = array(
        
                );
 
            }}
        ?>";
        public static string PlantillaController =
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
        public static string PlantillaViewAdminShow =
        @"@extends('admin._layouts.default')

        @section('main')

        @stop
        ";
        public static string PlantillaViewAdminIndex =
            @"@extends('admin._layouts.inside')
            @section('js_ready')
            @stop
            @section('titulo')
                {3}
            @stop
            @section('breadcumb')
                <ul class=""breadcrumb"">
                    <li><a href = ""{{{{ \URL::route('admin.{0}.edit') }}}}"" > {1}</a> <span class=""divider"">/</span></li>
                    <li class=""active"">{3}</li>
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
		            <a href = ""{{{{ URL::route('admin.{2}.create') }}}}"" class=""btn btn-success""><i class=""icon-plus-sign""></i> Crear un nuevo {3}</a>
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
                                        @foreach(${2} as $e)
                                        < tr >
                                            < td >
                                                < a href=""{{{{ URL::route('admin.{2}.edit', $e->id) }}}}"">{{{{ $e->idiomas()->espanyol()->get()->first()->pivot->titulo }}}}</a>
								            </td>
								            <td>{{{{ \Helper::cortarCadena($e->idiomas()->catalan()->get()->first()->pivot->descripcion, 100,'...') }}}}</td>
								            <td >
									            <!--
                                                @if(!$e->isAlone())
                                                    @if($e->isFirst())
											            <a href = ""{{{{ URL::route('admin.{2}.ordenarAbajo', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-down""></i></a>
                                                    @elseif($e->isLast())
											            <a href = ""{{{{ URL::route('admin.{2}.ordenarArriba', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-up""></i></a>
										            @else
                                                        <a href=""{{{{ URL::route('admin.{2}.ordenarArriba', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-up""></i></a>
											            <a href = ""{{{{ URL::route('admin.{2}.ordenarAbajo', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-circle-arrow-down""></i></a>
										            @endif
                                                @endif
									            -->
									            <a href = ""{{{{ URL::route('admin.{2}.edit', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-edit""></i>&nbsp;Editar&nbsp;</a>
                                                @if($e->isActivo())
										                <a href = ""{{{{ URL::route('admin.{2}.publicar', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-eye-open""></i> &nbsp;Esconder&nbsp;</a>
									            @else
                                                        <a href=""{{{{ URL::route('admin.{2}.publicar', $e->id) }}}}"" class=""btn btn-success btn-mini pull-left""><i class=""icon-eye-close""></i> &nbsp;Publicar&nbsp;</a>
									            @endif
									            {{{{ Form::open(array('route' => array('admin.{2}.destroy', $e->id), 'method' => 'delete', 'data-confirm' => 'Estas seguro?')) }}}}
										            <button type = ""submit"" class=""btn btn-danger btn-mini""><i class=""icon-remove-sign""></i>&nbsp;Eliminar&nbsp;</button>
									            {{{{ Form::close() }}}}

								            </td>
							            </tr>
							            @endforeach
                                    </tbody>
					            </table>
					            {{{{ ${2}->links() }}}}
				            </div>
			            </div>

  
            @stop";

    }
}
