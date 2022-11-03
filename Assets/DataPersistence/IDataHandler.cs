using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.DataPersistence
{
    public interface IDataHandler<T>
    {
        public T Load();

        public void Save(T data);

        public bool DoesFileExist(string fileName);
    }
}
