using Ejab.BAL.ModelViews;
using Ejab.BAL.Reository;
using Ejab.DAl;
using Ejab.DAl.Common;
using Ejab.DAl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejab.BAL.Services
{
    public interface ISysLog: IGenericRepository<SysLog>
    {
        void AddNewLog(ActionData actiontype, string desc, int createdby);
        List<LogDTO> GetLogData(int pageIndex, int pageSize, string searchText = null);
        int Count(string searchText = null);
    }
}
