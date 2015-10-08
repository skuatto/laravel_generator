using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LARAVEL_WEB_GENERATOR;
namespace LARAVEL_WEB_GENERATOR
{
    public class XmlReader
    {
        public XmlModel Model {get; set;}

        public static XmlModel LeerXML(string ruta)
        {
            XmlModel model = null;
            try
            {
                XDocument documentoXML = XDocument.Load(ruta);

                model = new XmlModel
                {
                    Ruta = documentoXML.Descendants("RUTA").First().Value,
                    Nombre = documentoXML.Descendants("NOMBRE").First().Value,
                    Descripcion = documentoXML.Descendants("DESCRIPCION").First().Value,,
                    Idiomas = documentoXML.Descendants("IDIOMAS").Elements().Select(x => new Idioma { Nombre = x.Name.ToString() }).ToList(),
                    Elementos = documentoXML.Descendants("ELEMENTOS").Elements().Select
                    (
                        x => new Elemento
                        {
                            Nombre = x.Name.ToString(),
                            Descripcion = x.Attribute("descripcion").Value,
                            Singular = (int.Parse(x.Attribute("singular").Value) == 1) ? true : false,
                            Campos = x.Elements().Select(
                                e => new Campo { Nombre = e.Name.ToString(), Descripcion = e.Value, Tipo = e.Attribute("type").Value.ToString(), Editable = (int.Parse(e.Attribute("editable").Value) == 1) ? true : false,  MultiIdioma = (int.Parse(e.Attribute("multidioma").Value) == 1) ? true : false }
                            ).ToList()
                        }
                    ).ToList(),

                    Menus = documentoXML.Descendants("MENU").Elements().Select
                    (
                        x => new Menu 
                        { 
                            Nombre = x.Name.ToString(), 
                            Descripcion = x.Attribute("descripcion").Value,
                            Submenu = x.Elements().Select
                            (
                                e => new SubMenu { Nombre = e.Name.ToString(), Descripcion = e.Value }
                            ).ToList()
                        }
                    ).ToList()
                };

            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }

            return model;
        }
    }
}
