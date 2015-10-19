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
                    Descripcion = documentoXML.Descendants("DESCRIPCION").First().Value,
                    Idiomas = documentoXML.Descendants("IDIOMAS").Elements().Select(x => new Idioma { Nombre = x.Name.ToString(), Descripcion = x.Value }).ToList(),

                    Elementos = documentoXML.Descendants("ELEMENTOS").Elements().Select
                    (
                        x => new Elemento
                        {
                            Nombre = x.Name.ToString(),
                            Descripcion = x.Attribute("descripcion").Value,
                            Singular = x.HasAttributeBool("singular"),
                            Campos = x.Elements().Select(
                                e => new Campo
                                {
                                    Nombre = e.Name.ToString(),
                                    Descripcion = e.Value,
                                    Tipo = e.HasAttributeString("type"),
                                    Editable = e.HasAttributeBool("editable"),
                                    MultiIdioma = e.HasAttributeBool("multidioma"),
                                    RelacionCampo = e.HasAttributeString("relacion_campo"),
                                    RelacionHijo = e.HasAttributeString("relacion_hijo")
                                }
                            ).Where(e => !e.Nombre.Equals("HIJOS")).ToList(),
                            Elementos = x.Elements().Select(y => new { Name = y.Name.ToString(), Elements = y.Elements() }).Where(y => y.Name.Equals("HIJOS")).Select
                                (
                                    r => r.Elements.Select
                                    (
                                      f => new Elemento
                                      {
                                          Nombre = f.Name.ToString(),
                                          Descripcion = f.Attribute("descripcion").Value,
                                          Singular = f.HasAttributeBool("singular"),
                                          Campos = f.Elements().Select
                                            (
                                                z => new Campo
                                                {
                                                    Nombre = z.Name.ToString(),
                                                    Descripcion = z.Value,
                                                    Tipo = z.Attribute("type").Value.ToString(),
                                                    Editable = z.HasAttributeBool("editable"),
                                                    MultiIdioma = z.HasAttributeBool("multidioma"),
                                                    RelacionCampo = z.HasAttributeString("relacion_campo"),
                                                    RelacionHijo = z.HasAttributeString("relacion_hijo")
                                                }
                                            ).ToList()
                                      }
                                    ).ToList()
                                ).ToList().FirstOrDefault()
                        }).ToList(),
                    
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
