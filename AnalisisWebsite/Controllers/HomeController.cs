using AnalisisWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AnalisisWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ValuesTablesContext db;
        public HomeController(ValuesTablesContext valuesTablesContext)
        {
            db = valuesTablesContext;
        }

        public async Task<IActionResult> Index(int? id)
        {

            List<TableValue> valuesTables = await db.TableValues.ToListAsync();
            //if (id != null)
            //{
            //    TableValue values = await db.TableValues.FirstOrDefaultAsync(p => p.Id == id);

            //}
            Tables plvm = new Tables
            {
                valuesTables = valuesTables.ToList()
            };
            return View(plvm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(TableValue tableValue, List<double> F1, List<double> F2, List<double> F3, List<double> F4, List<double> F5, int? id)
        {
            List<TableValue> valuesTables = await db.TableValues.ToListAsync();
            //редактирование таблицы

            //Работа с файлом
            //StreamWriter sW = new StreamWriter(@"Content/Notepad/res.txt");
            //for (int i = 0; i < 50; i++)
            //    //for (int j = 0; j < 10; j++)
            //        sW.WriteLine(F6.ToArray().ElementAt(i));
            //sW.Close();
            //Работа с файлом


            for (int i = 0; i <= 10; i++)
            {
                tableValue = await db.TableValues.FirstOrDefaultAsync(p => p.Id == i + 1);

                if (tableValue != null && F1.Count() == 10 && F2.Count() == 10 && F3.Count() == 10 && F4.Count() == 10 && F5.Count() == 10)
                {
                    tableValue.F1 = F1[i];
                    tableValue.F2 = F2[i];
                    tableValue.F3 = F3[i];
                    tableValue.F4 = F4[i];
                    tableValue.F5 = F5[i];
                    db.TableValues.Update(tableValue);
                    await db.SaveChangesAsync();
                }
                else if(i == 10 && (F1.Count() != 10 || F2.Count() != 10 || F3.Count() != 10 || F4.Count() != 10 || F5.Count() != 10) /*|| (F1[i] == 0 || F2[i] == 0 || F3[i] == 0 || F4[i] == 0 || F5[i] == 0)*/)
                {
                    ModelState.AddModelError("", "Значения ячеек таблицы не могут быть пустыми!");
                }

            }
            //редактирование таблицы

            //расчёты
            double[] sumFp = new double[6];
            double[] variance = new double[6];
            //расчёт среднего значения Fi, x ̅_j
            sumFp[1] = valuesTables.Sum(x => x.F1);
            sumFp[2] = valuesTables.Sum(x => x.F2);
            sumFp[3] = valuesTables.Sum(x => x.F3);
            sumFp[4] = valuesTables.Sum(x => x.F4);
            sumFp[5] = valuesTables.Sum(x => x.F5);

            List<double> averageX = new List<double>();

            for (int j = 1; j < sumFp.Length; j++)
            {
                averageX.Add(sumFp[j] / 10);

            }
            ViewBag.averageX = averageX;
            //расчёт среднего значения Fi, x ̅_j


            //расчёт S_j^2
            List<double> varianceS = new List<double>();
            for (int i = 0; i < valuesTables.ToArray().Count(); i++)
            {
                variance[1] += Math.Pow(valuesTables.ToArray().ElementAt(i).F1 - sumFp[1] / 10, 2);
                variance[2] += Math.Pow(valuesTables.ToArray().ElementAt(i).F2 - sumFp[1] / 10, 2);
                variance[3] += Math.Pow(valuesTables.ToArray().ElementAt(i).F3 - sumFp[1] / 10, 2);
                variance[4] += Math.Pow(valuesTables.ToArray().ElementAt(i).F4 - sumFp[1] / 10, 2);
                variance[5] += Math.Pow(valuesTables.ToArray().ElementAt(i).F5 - sumFp[1] / 10, 2);


            }
            for (int j = 1; j < variance.Length; j++)
            {
                varianceS.Add(variance[j]);
            }
            ViewBag.varianceS = varianceS;
            //расчёт S_j^2

            //расчёт s_j^2
            List<double> varianceS2 = new List<double>();
            for (int j = 0; j < variance.Length - 1; j++)
            {
                varianceS2.Add(varianceS[j] / (10 - 1));
            }
            ViewBag.varianceS2 = varianceS2;
            //расчёт s_j^2

            //Максимальное число в строке
            double[] columnData = new double[6];
            for (int j = 0; j < varianceS2.Count(); j++)
            {
                columnData[j] = varianceS2.ToArray().ElementAt(j);
            }
            //Максимальное число в строке
            ViewBag.observedG = columnData.Max();

            //расчёт G_набл
            double sjSumm = 0;
            double gObserved = 0;
            for (int i = 0; i < varianceS2.Count(); i++)
            {
                sjSumm += varianceS2.ToArray().ElementAt(i);
                gObserved = columnData.Max() / sjSumm;
            }
            //расчёт G_набл

            ViewBag.observedG = "G_набл = " + Math.Round(gObserved, 3);
            ViewBag.criticalG = "G_крит = " + 0.3029; //значение из таблицы Кохрена

            //сравнение G_набл и G_крит
            ViewBag.comparisonG = (gObserved > 0.3029) ? "–> G_набл > G_крит, продолжать проверку гипотезы Н_0 не имеет смысла" : "–> G_набл < G_крит, можно продолжить проверку гипотезы Н_0";
            //сравнение G_набл и G_крит


            //расчёт среднего значения xj
            double xjSumm = 0, xjAverage = 0;
            for (int j = 0; j < averageX.Count(); j++)
            {
                xjSumm += averageX[j];
                xjAverage = xjSumm / 5;
            }
            //расчёт среднего значения xj

            //расчёт S_общ^2
            double[] summGeneralS = new double[10];
            double generalS = 0;
            for (int j = 0; j < valuesTables.ToArray().Count(); j++)
            {
                summGeneralS[0] += Math.Pow(valuesTables.ToArray().ElementAt(j).F1 - Convert.ToDouble(xjAverage), 2);
                summGeneralS[1] += Math.Pow(valuesTables.ToArray().ElementAt(j).F2 - Convert.ToDouble(xjAverage), 2);
                summGeneralS[2] += Math.Pow(valuesTables.ToArray().ElementAt(j).F3 - Convert.ToDouble(xjAverage), 2);
                summGeneralS[3] += Math.Pow(valuesTables.ToArray().ElementAt(j).F4 - Convert.ToDouble(xjAverage), 2);
                summGeneralS[4] += Math.Pow(valuesTables.ToArray().ElementAt(j).F5 - Convert.ToDouble(xjAverage), 2);
            }
            for (int i = 0; i < summGeneralS.Length; i++)
            {
                generalS += summGeneralS[i];
            }
            ViewBag.generalS = "S_общ^2 = " + Math.Round(generalS, 3);
            //расчёт S_общ^2

            //расчёт x ̅
            List<double> varianceX = new List<double>();
            for (int j = 0; j < averageX.Count(); j++)
            {
                varianceX.Add(Math.Pow(averageX[j] - Convert.ToDouble(xjAverage), 2));
            }
            ViewBag.varianceX = varianceX;
            //расчёт x ̅

            //расчёт суммы x ̅
            double xUnderlineSumm = 0;
            for (int j = 0; j < varianceX.Count(); j++)
            {
                xUnderlineSumm += varianceX[j];
            }
            //расчёт суммы x ̅


            //расчёт S_факт^2
            ViewBag.actualS = "S_факт^2 = " + Math.Round(10 * xUnderlineSumm, 3);
            //расчёт S_факт^2

            //расчёт S_ост^2
            ViewBag.residualS = "S_ост^2 =  " + Math.Round(generalS - (10 * xUnderlineSumm), 3);
            //расчёт S_ост^2

            //расчёт s^2_факт
            ViewBag.sActual = "s^2_факт = " + Math.Round(10 * xUnderlineSumm / (5 - 1), 3);
            //расчёт s^2_факт

            //расчёт s^2_окт
            ViewBag.sResidual = "s^2_окт = " + Math.Round((generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1)), 3);
            //расчёт s^2_окт

            //сравнение s^2_факт и s^2_ост
            ViewBag.sСomparison = ((10 * xUnderlineSumm / (5 - 1)) < (generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1))) ? "–> s^2_факт < s^2_ост, отсюда следует справедливость гипотезы Н_0.\nНет необходимости прибегать к критерию Фишера." : "–> s^2_факт > s^2_ост, есть основания отвергнуть гипотезу Н_0";
            //сравнение s^2_факт и s^2_ост


            ViewBag.numerator = "k1 = " + (5 - 1); //Число степеней свободы числителя
            ViewBag.denominator = "k1 = " + (5 * (10 - 1)); //Число степеней свободы знаменателя 

            //расчёт F_набл
            ViewBag.observedF = "F_набл = " + Math.Round(10 * xUnderlineSumm / (5 - 1) / ((generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1))), 3);
            //расчёт F_набл

            ViewBag.criticalF = "F_крит = " + 2.58 + " - при уровне значимости α=0,05"; //значение F_крит с таблицы Фишера, при уровне значимости α=0,05

            //сравнение F_набл и F_крит
            ViewBag.comparisonF = (10 * xUnderlineSumm / (5 - 1) / ((generalS - (10 * xUnderlineSumm)) / (5 * (10 - 1))) < 2.58) ? "–> F_набл < F_крит, гипотеза не противоречит статистическим данным" : "–> F_набл > F_крит, гипотеза Н_0 отвергается";
            //сравнение F_набл и F_крит
            //расчёты


            Tables plvm = new Tables
            {
                valuesTables = valuesTables.ToList()
            };
            return View(plvm);
        }

        //public IActionResult Edit(int id = 0)
        //{
        //    Tables tables = new Tables();
        //    ValuesTables valuesTables = null;
        //    valuesTables = tables.valuesTables.FirstOrDefault(b => b.NumberRow == id);
        //    return View(valuesTables);
        //}

        //[HttpPost]
        //public IActionResult Edit(ValuesTables valuesTables)
        //{
        //    //Tables tables = new Tables();

        //    //Tables tables = new Tables();
        //    //ValuesTables valuesTables = null;
        //    //valuesTables = tables.valuesTables.FirstOrDefault(b => b.NumberRow == 1);
        //    MethodsRepository methodsRepository = new MethodsRepository();

        //    methodsRepository.SaveProduct(valuesTables);

        //    return RedirectToAction("Index");
        //}

        //public IActionResult Privacy(int id)
        //{
        //    //Tables tables = new Tables();

        //    MethodsRepository methodsRepository = new MethodsRepository();

        //    ValuesTables valuesTables = null;
        //    valuesTables = methodsRepository.listDB(id).ToList().FirstOrDefault(b => b.NumberRow == id);
        //    return View(valuesTables);

        //}
        //[HttpPost]
        //public IActionResult Privacy(ValuesTables valuesTables, int id)
        //{
        //    MethodsRepository methodsRepository = new MethodsRepository();
        //    methodsRepository.listDB(id).ToList();
        //    valuesTables = methodsRepository.listDB(id).ToList().FirstOrDefault(b => b.NumberRow == id);
        //    methodsRepository.SaveTable(valuesTables);

        //    return RedirectToAction("Index");

        //}



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
