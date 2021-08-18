using Ofakim.Contracts;
using Ofakim.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ofakim.Jobs
{
    public class UpdateXmlFileTask
    {
        public IWriteToFile _writeToFile;
        public UpdateXmlFileTask(IWriteToFile writeToFile)
        {
            _writeToFile = writeToFile;
        }

        public void UpdateXmlFile()
        {
            lock (IndexModel.xmlFileLock)
            {

                var date = DateTime.Now;

                try
                {
                    _writeToFile.WriteToFile();
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }
    }
}
