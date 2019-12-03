using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

    class HashModel
    {
        HashAlgorithm hashAlgorithm;
        public HashModel(HashAlgorithm hashAlgorithm)
        {
            this.hashAlgorithm = hashAlgorithm;
           
        }
        public HashAlgorithm Algorithm
        {
            set
            {
                hashAlgorithm = value;
            }
        }
        public byte[] GetHashCode(byte[] source) =>  hashAlgorithm.ComputeHash(source);
        

    }

