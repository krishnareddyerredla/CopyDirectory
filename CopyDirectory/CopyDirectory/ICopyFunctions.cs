using System;
using System.Threading.Tasks;

namespace CopyDirectory
{  
    public interface ICopyFunctions
    {
        Task<CopyResult> CopyDirectory(string source, string destination, Action<string> progress);
    }
}
