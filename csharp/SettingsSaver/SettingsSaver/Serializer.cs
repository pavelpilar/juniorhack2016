using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SettingsSaver
{
    public class Serializer<T>
    {
        public void Serialize(string filePath)
        {
            if(!File.Exists(filePath)) { throw new Exception(); }
            XmlSerializer serializer = new XmlSerializer(typeof(T));

        }
    }
}
