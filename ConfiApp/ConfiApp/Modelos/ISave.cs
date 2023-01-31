using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConfiApp.Modelos
{
  public   interface ISave
    {
        void Save(string filename, string contentType, MemoryStream stream);
        Task<byte[]> CaptureAsync();
    }
}
