using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StromzählerContext;

namespace _65_WPF_Stromzähler
{
    public class LoadTable
    {
        private SzContext context = new();
        
        public List<CounterValue> loadTable()
        {

            return context.CounterValues.Include(x => x.Counter).Select(x => new CounterValue
            {
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
                Date = x.Date,
            }).OrderByDescending(x => x.Date).ToList();


        }
    }
}