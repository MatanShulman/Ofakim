using Ofakim.Contracts;
using Ofakim.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace Ofakim.Services
{
    public class ReadFileService : IReadFromFile
    {
        private string xmlPath = Path.Combine(Directory.GetCurrentDirectory(), "Settings.xml");


        public List<BlockModel> ReadFromFile()
        {

            if (!File.Exists(xmlPath))
            {
                return new List<BlockModel>();
            }
            try
            {
                //parse and reutrn list
                using (var stream = File.OpenText(xmlPath))
                {
                    XmlSerializer s = new XmlSerializer(typeof(List<BlockModel>));

                    return (List<BlockModel>)s.Deserialize(stream);
                }

            }
            catch
            {
                throw;
            }
        }
    }
}

