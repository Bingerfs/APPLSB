using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataPersistence
{
    public interface IFileDataHandler
    {
        public T Load<T>();

        public void Save<T>(T data);

        public bool DoesFileExist(string fileName);
    }
}
