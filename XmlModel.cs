using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LARAVEL_WEB_GENERATOR
{
    public class XmlModel
    {
        public string ruta { get; set; }
        public string nombre { get; set; }

        public List<Idioma> idiomas { get; set; }
        public List<Elemento> elementos { get; set; }
        public List<Menu> menus { get; set; }
    }

    public class Idioma
    {
        public string nombre { get; set; }

    }

    public class Elemento
    {
        public string nombre { get; set; }
        public List<Campo> campos { get; set; }
    }

    public class Campo
    {
        public string nombre { get; set; }
        public string tipo { get; set; }
        public bool editable { get; set; }
   }

    public class Menu
    {
        public string nombre { get; set; }
        public List<SubMenu> submenu { get; set; }
    }

    public class SubMenu
    {
        public string nombre { get; set; }
    }
}
