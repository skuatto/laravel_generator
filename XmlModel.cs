using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{
    public class XmlModel
    {
        public string Ruta { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public List<Idioma> Idiomas { get; set; }
        public List<Elemento> Elementos { get; set; }
        public List<Menu> Menus { get; set; }
    }

    public class Idioma
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class Elemento
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Singular { get; set; }
        public List<Campo> Campos { get; set; }
        public List<Elemento> Elementos  { get; set; }
    }

    public class Campo
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public string RelacionHijo { get; set; }
        public string RelacionCampo { get; set; }
        public bool Editable { get; set; }
        public bool MultiIdioma { get; set; }
        

   }

    public class Menu
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<SubMenu> Submenu { get; set; }
    }

    public class SubMenu
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

}
