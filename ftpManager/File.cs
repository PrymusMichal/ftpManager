using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ftpManager
{
    class File
    {
        public string fileName { get; set; }
        public string fileLocation { get; set; }

        public File(string fileName, string fileLocation)
        {
            this.fileName = fileName;
            this.fileLocation = fileLocation;
        }
    }
}
