using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFFinal.Models
{
    public interface ISaver<T>
    {
        public void Save(T obj, string path);
    }
}
