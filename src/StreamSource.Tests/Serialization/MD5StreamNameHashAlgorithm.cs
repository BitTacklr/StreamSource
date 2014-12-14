using System;
using System.Security.Cryptography;
using System.Text;
using StreamSource.Naming;
using StreamSource.Storage;

namespace StreamSource.Serialization
{
    public class MD5StreamNameHashAlgorithm : IStreamNameHashAlgorithm 
    {
        public byte[] ComputeHash(StreamName name)
        {
            if (name == null) throw new ArgumentNullException("name");
            using (var algorithm = MD5.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(name.ToString()));    
            }
        }
    }
}
