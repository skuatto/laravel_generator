using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LARAVEL_WEB_GENERATOR
{
    public static class MyExtensions
    {
        public static bool HasAttributeBool(this XElement e, string XName)
        {
            bool retorno = false;
            if (e.Attribute(XName) != null)
            {
                if(int.Parse(e.Attribute(XName).Value) == 1)
                    retorno = true;
            }
            return retorno;
        }

        public static string HasAttributeString(this XElement e, string XName)
        {
            string retorno = "";
            if (e.Attribute(XName) != null)
            {
                retorno = e.Attribute(XName).Value;
            }
            return retorno;
        }
    }
}
