using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


    class HashController
    {
       HashModel hashModel;
        HashView hashView;
        public HashController(HashModel  hashModel,HashView hashView)
        {
            this.hashModel = hashModel;
            this.hashView = hashView;
        }
       public  HashAlgorithm Algorithm
        {
            set
            {
                hashModel.Algorithm = value;
            }
        }
        public void SetBasedOnString(string name)
        {
           
            switch (name)
            {
                case "MD5": Algorithm = MD5.Create(); break;
                case "SHA256":Algorithm = SHA256.Create(); break;
                case "SHA1Managed":Algorithm = SHA1Managed.Create(); break;


            }
            
        }
        public  void  UpdateView( byte[] source, Action<byte[]> toDo)
        {
         hashView.Update(hashModel.GetHashCode(source), toDo);
        }
     

    }

