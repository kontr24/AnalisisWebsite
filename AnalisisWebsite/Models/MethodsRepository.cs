using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalisisWebsite.Models
{
    public class MethodsRepository 
    {
        //public List<TableValue> valuesTables;
        private ValuesTablesContext db;
        public MethodsRepository(ValuesTablesContext valuesTablesContext)
        {
            db = valuesTablesContext;
        }
        public void SaveTableValue(TableValue tableValue)
        {

            //TableValue dbEntry = db.TableValues.FirstOrDefault(x=>x.Id == id);
            //if (dbEntry != null)
            //{
            //    dbEntry.F1 = tableValue.F1;
            //    dbEntry.F2 = tableValue.F2;
            //    dbEntry.F3 = tableValue.F3;
            //    dbEntry.F4 = tableValue.F4;
            //    dbEntry.F5 = tableValue.F5;
            //}
            //db.SaveChanges();

        }

        //public List<TableValue> listDB(/*TableValue values,*/ int id)
        //{
        //    List<TableValue> tableValues = new List<TableValue>();
        //    tableValues.Add(new TableValue(1, -2, -3, 5, -2, 3));
        //    tableValues.Add(new ValuesTables(2, 470, 450, 558, 600, 550));
        //    tableValues.Add(new ValuesTables(3, 5, 4, 5, 6, 6));
        //    tableValues.Add(new ValuesTables(4, 110, 127, 107, 108, 192));
        //    tableValues.Add(new ValuesTables(5, 1288, 1441, 1458, 845, 1496));
        //    tableValues.Add(new ValuesTables(6, 675, 558, 687, 605, 720));
        //    tableValues.Add(new ValuesTables(7, 520, 429, 512, 475, 526));
        //    tableValues.Add(new ValuesTables(8, 2602, 2900, 5360, 2606, 2160));
        //    tableValues.Add(new ValuesTables(9, 1010, 930, 1070, 790, 900));
        //    tableValues.Add(new ValuesTables(10, 448, 432, 521, 467, 455));

        //    if (id != 0)
        //    {
        //        //valuesTables[1].NumberRow = 45;

        //        valuesTables[id - 1].F1 = values.F1;
        //        valuesTables[id - 1].F2 = values.F2;
        //        valuesTables[id - 1].F3 = values.F3;
        //        valuesTables[id - 1].F4 = values.F4;
        //        valuesTables[id - 1].F5 = values.F5;
        //    }


        //    return valuesTables;

        //}
    }
}
