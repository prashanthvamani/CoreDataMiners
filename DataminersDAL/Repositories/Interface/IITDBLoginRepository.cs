using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataminersDAL.Repositories.Interface
{
    public interface IITDBLoginRepository
    {
        DataSet ITDBLogin(string ntLoginID, string password);
    }
}
