using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnalisisWebsite.Models
{
    public class TableValue
    {
        //public ValuesTables(int Id, float f1, float f2, float f3, float f4, float f5)
        //{
        //    this.Id = Id;
        //    this.F1 = f1;
        //    this.F2 = f2;
        //    this.F3 = f3;
        //    this.F4 = f4;
        //    this.F5 = f5;
        //}
        //public ValuesTables()
        //{

        //}
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните все ячейки!")]
        public double F1 {get;set;}
        [Required(ErrorMessage = "Заполните все ячейки!")]
        public double F2 { get; set; }
        [Required(ErrorMessage = "Заполните все ячейки!")]
        public double F3 { get; set; }
        [Required(ErrorMessage = "Заполните все ячейки!")]
        public double F4 { get; set; }
        [Required(ErrorMessage = "Заполните все ячейки!")]
        public double F5 { get; set; }

        //public List<TableValue> valuesTables { get; set; }

    }

    public class Tables
    {
        public List<TableValue> valuesTables { get; set; }

    }

}
