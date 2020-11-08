using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cw2
{
    interface IFileWriter
    {
        public void write(Uczelnia uczelnia, string outputdata);
    }
}
