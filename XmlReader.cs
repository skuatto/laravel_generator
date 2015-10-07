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
        public XmlModel model {get; set;}

        public static XmlModel LeerXML(string ruta)
        {
            XmlModel model = null;
            try
            {
                XDocument documentoXML = XDocument.Load(ruta);

                model = new XmlModel
                {
                    ruta = documentoXML.Descendants("RUTA").First().Value,
                    nombre = documentoXML.Descendants("NOMBRE").First().Value,
                    idiomas = documentoXML.Descendants("IDIOMAS").Elements().Select(x => new Idioma { nombre = x.Name.ToString() }).ToList(),
                    elementos = documentoXML.Descendants("ELEMENTOS").Elements().Select
                    (
                        x => new Elemento
                        {
                            nombre = x.Name.ToString(), campos = x.Elements().Select(
                                e => new Campo { nombre = e.Name.ToString(), tipo = e.Attribute("type").Value.ToString(), editable = (int.Parse(e.Attribute("editable").Value) == 1) ? true : false }
                            ).ToList()
                        }
                    ).ToList(),

                    menus = documentoXML.Descendants("MENU").Elements().Select
                    (
                        x => new Menu 
                        { 
                            nombre = x.Name.ToString(), submenu = x.Elements().Select
                            (
                                e => new SubMenu { nombre = e.Name.ToString() }
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
