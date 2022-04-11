using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Flex_SGM.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Globalization;
using System.IO;
using ClosedXML.Excel;


namespace Flex_SGM.Controllers
{
    public class Metricos1Controller : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Metricos1
        public async Task<ActionResult> Index(string amaquina, string maquina, string submaquina, string mgroup, string xmgroup, string btn = "Metricos por Dia", string dti = "", string dtf = "")
        {
            var metricos = db.Metricos.ToList<Metricos>();
            var fulldatafiltered = new List<Bitacora>();
            var datafiltered = new List<Bitacora>();
            var datafiltered1 = new List<Bitacora>();
            var datafiltered2 = new List<Bitacora>();
            var datafiltered3 = new List<Bitacora>();
            var oILs = db.OILs.Include(o => o.Maquinas);
            List<newmetricos2> Ldata = new List<newmetricos2>();
            List<newmetricos3> Ldata3 = new List<newmetricos3>();
            var fecha = DateTime.Now.AddDays(-7);
            var fechaf = DateTime.Now;
            DateTimeFormatInfo formatoFecha1 = CultureInfo.CurrentCulture.DateTimeFormat;
            string nombreMes1 = formatoFecha1.GetMonthName(fecha.Month);
            string nombreMes2 = formatoFecha1.GetMonthName(fechaf.Month);

            if (!string.IsNullOrEmpty(dti))
            {
                fecha = Convert.ToDateTime(dti);
            }
            if (!string.IsNullOrEmpty(dtf))
            {
                fechaf = Convert.ToDateTime(dtf);
            }
            //---data---

            var mindia = 480.0d;
            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {

                if (amaquina.Contains("Soldadura"))
                {
                    mindia = 440.0d;
                }

                if (amaquina.Contains("Ensamble"))
                {
                    mindia = 440.0d;
                }
                if (amaquina.Contains("Estampado"))
                {
                    mindia = 440.0d;
                }
            }
            var multiplicador = mindia;

            ViewBag.xmgroup = mindia.ToString();
            var dataf = db.Bitacoras.Where(w => (w.DiaHora.Year >= fecha.Year) &&
                           (w.DiaHora.Year <= fechaf.Year) &&
                           (w.Tiempo > 0)
                            );
            if (!string.IsNullOrEmpty(maquina))
                dataf = dataf.Where(m => m.Maquinas.Maquina == maquina);

            if (!String.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
            {
                if (amaquina.Contains("MetalFinish"))
                {
                    dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2" || s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat" || s.Maquinas.Area == "MetalFinish");
                    var dtdt = dataf.ToList();
                }
                else
                {
                    if (amaquina == "Cromo")
                    {
                        dataf = dataf.Where(s => s.Maquinas.Area == "Cromo" || s.Maquinas.Area == "Cromo1" || s.Maquinas.Area == "Cromo2" || s.Maquinas.Area == "AutoPulido1" || s.Maquinas.Area == "AutoPulido2");
                    }

                    if (amaquina.Contains("Pintura"))
                    {
                        dataf = dataf.Where(s => s.Maquinas.Area == "Pintura" || s.Maquinas.Area == "Ecoat" || s.Maquinas.Area == "Topcoat");
                    }

                    if (amaquina.Contains("Soldadura"))
                    {
                        amaquina = "Soldadura";
                    }
                    dataf = dataf.Where(s => s.Maquinas.Area.Contains(amaquina));
                }
            }

            ViewBag.amaquina = new SelectList(Enum.GetValues(typeof(flex_Areas)).Cast<flex_Areas>().ToList());
            string[] array = { "Maquina", "Falla", "Area", "SubMaquina" };
            ViewBag.mgroup = new SelectList(array);

            var maquinas = db.Maquinas.Where(m => m.ID > 0);
            if (!string.IsNullOrEmpty(amaquina))
                if (amaquina.Contains("MetalFinish"))
                {
                    maquinas = db.Maquinas.Where(m => m.Area == "Cromo" || m.Area == "Cromo1" || m.Area == "Cromo2" || m.Area == "AutoPulido1" || m.Area == "AutoPulido2" || m.Area == "Pintura" || m.Area == "Ecoat" || m.Area == "Topcoat" || m.Area == "MetalFinish");

                }
                else
                    maquinas = db.Maquinas.Where(m => m.Area == amaquina);

            if (!string.IsNullOrEmpty(maquina))
                maquinas = db.Maquinas.Where(m => m.Maquina == maquina);

            var maquis = maquinas.GroupBy(g => g.Maquina).ToList();

            var mul_maquinas = maquis.Count();
            ViewBag.maquina = new SelectList(maquinas.GroupBy(g => g.Maquina), "Key", "Key");

            ViewBag.submaquina = new SelectList(maquinas, "ID", "SubMaquina");

            List<newmetricos3> allmaq = new List<newmetricos3>();

            for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
            {
                for (int jmes = 1; jmes <= 12; jmes++)
                {
                    DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                    string nombreMes = formatoFecha.GetMonthName(jmes);
                    var dias = DateTime.DaysInMonth(iaño, jmes);

                    for (int kdia = 1; kdia <= dias; kdia++)
                    {
                        DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                        DateTime idi3er = idi.AddDays(+1);

                        if (idi >= fecha && idi <= fechaf)
                        {
                            //datafiltered = dataf.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day && w.DiaHora.Day != 1).ToList();
                            datafiltered = dataf.Where(w => w.DiaHora.Year == idi.Year && w.DiaHora.Month == idi.Month && w.DiaHora.Day == idi.Day && w.DiaHora.Hour > 7).ToList();
                            //datafiltered.AddRange(temp1);
                            var temp2 = dataf.Where(w => w.DiaHora.Year == idi3er.Year && w.DiaHora.Month == idi3er.Month && w.DiaHora.Day == idi3er.Day && w.DiaHora.Hour <= 7).ToList();
                            datafiltered.AddRange(temp2);
                            datafiltered1 = datafiltered.Where(s => (s.DiaHora.Hour > 7 && s.DiaHora.Hour <= 15)).ToList();
                            datafiltered2 = datafiltered.Where(s => (s.DiaHora.Hour > 15 && s.DiaHora.Hour <= 23)).ToList();
                            datafiltered3 = datafiltered.Where(s => (s.DiaHora.Hour > 23 || s.DiaHora.Hour <= 7)).ToList();
                            fulldatafiltered.AddRange(datafiltered);

                            foreach (var simplemaquina in maquis)
                            {
                                string thistiempo = "";
                                if (btn == "Metricos por Dia")
                                {
                                    thistiempo = iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString();
                                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                                    ViewBag.dxx = " Dia";
                                }

                                if (btn == "Metricos por Mes")
                                {
                                    thistiempo = iaño.ToString() + "-" + nombreMes;
                                    ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                                    ViewBag.dxx = " Mes";
                                }
                                if (btn == "Metricos por Años")
                                {
                                    thistiempo = iaño.ToString();
                                    ViewBag.dx = " Año:" + fecha.AddYears(-5).ToString("yyyy") + " a el Año:" + fecha.ToString("yyyy");
                                    ViewBag.dxx = " Anual";
                                }
                                newmetricos3 simplemaquinam = new newmetricos3
                                {
                                    TiempoLabel = thistiempo,
                                    maquina = simplemaquina.Key,
                                    Disponible1 = (Double)multiplicador,
                                    Disponible2 = (Double)multiplicador,
                                    Disponible3 = (Double)multiplicador,
                                    CantidadFallas1 = datafiltered1.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    CantidadFallas2 = datafiltered2.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    CantidadFallas3 = datafiltered3.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Count(),
                                    TiempoMuerto1 = datafiltered1.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    TiempoMuerto2 = datafiltered2.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    TiempoMuerto3 = datafiltered3.Where(w => w.Maquinas.Maquina == simplemaquina.Key).Sum(s => s.Tiempo),
                                    MTTR1 = 0,
                                    MTTR2 = 0,
                                    MTTR3 = 0,
                                    MTBF = (Double)multiplicador * 3,
                                    Confiabilidad = 100,
                                    TarjetasTPM = 0
                                };

                                if (simplemaquinam.CantidadFallas1 != 0)
                                {
                                    simplemaquinam.MTTR1 = simplemaquinam.TiempoMuerto1 / simplemaquinam.CantidadFallas1;
                                }
                                if (simplemaquinam.CantidadFallas2 != 0)
                                {
                                    simplemaquinam.MTTR2 = simplemaquinam.TiempoMuerto2 / simplemaquinam.CantidadFallas2;
                                }
                                if (simplemaquinam.CantidadFallas3 != 0)
                                {
                                    simplemaquinam.MTTR3 = simplemaquinam.TiempoMuerto3 / simplemaquinam.CantidadFallas3;
                                }
                                var SumDisponobilidad = simplemaquinam.Disponible1 + simplemaquinam.Disponible2 + simplemaquinam.Disponible3;
                                simplemaquinam.MTBF = SumDisponobilidad;
                                if (simplemaquinam.CantidadFallas1 != 0 || simplemaquinam.CantidadFallas2 != 0 || simplemaquinam.CantidadFallas3 != 0)
                                {
                                    var sumCantidadFallas = simplemaquinam.CantidadFallas1 + simplemaquinam.CantidadFallas2 + simplemaquinam.CantidadFallas3;
                                    var sumTiempoMuerto = simplemaquinam.TiempoMuerto1 + simplemaquinam.TiempoMuerto2 + simplemaquinam.TiempoMuerto3;
                                    var MTTR = (simplemaquinam.MTTR1 + simplemaquinam.MTTR2 + simplemaquinam.MTTR3) / 3;
                                    simplemaquinam.MTBF = SumDisponobilidad / sumCantidadFallas;
                                    //  simplemaquinam.Confiabilidad = simplemaquinam.MTBF / (simplemaquinam.MTBF + MTTR);
                                    simplemaquinam.Confiabilidad = (simplemaquinam.MTBF - MTTR) / simplemaquinam.MTBF;
                                }
                                allmaq.Add(simplemaquinam);
                            }
                        }
                    }
                }
            }

            var allmaqbyday1 = allmaq.GroupBy(g => g.maquina).ToList();

            foreach (var inmaq in allmaqbyday1)
            {
                var sumd1 = inmaq.Sum(s => s.Disponible1);
                var sumd2 = inmaq.Sum(s => s.Disponible2);
                var sumd3 = inmaq.Sum(s => s.Disponible3);
                var ft1 = inmaq.Sum(s => s.CantidadFallas1);
                var ft2 = inmaq.Sum(s => s.CantidadFallas2);
                var ft3 = inmaq.Sum(s => s.CantidadFallas3);
                var sumtp1 = inmaq.Sum(s => s.TiempoMuerto1);
                var sumtp2 = inmaq.Sum(s => s.TiempoMuerto2);
                var sumtp3 = inmaq.Sum(s => s.TiempoMuerto3);
                var sumatm1 = sumtp1;// ((sumtp1 / sumd1) * 100);
                var sumatm2 = sumtp2;//((sumtp2 / sumd2) * 100);
                var sumatm3 = sumtp2;//((sumtp3 / sumd3) * 100);
                var smttr11 = inmaq.Sum(s => s.MTTR1);
                var smttr12 = inmaq.Sum(s => s.MTTR2);
                var smttr13 = inmaq.Sum(s => s.MTTR3);
                var sumamttr1 = smttr11 / inmaq.Count();
                var sumamttr2 = smttr12 / inmaq.Count();
                var sumamttr3 = smttr13 / inmaq.Count();
                var sumamtbf = (sumd1 + sumd2 + sumd3) / (ft1 + ft2 + ft3); //inmaq.Sum(s => s.MTBF) / inmaq.Count();
                var sumamttrs = sumamttr1 + sumamttr2 + sumamttr3;
                var sumaMant = ((sumamtbf - sumamttrs) / sumamtbf) * 100;
                newmetricos3 data_show = new newmetricos3
                {
                    TiempoLabel = "-",
                    maquina = inmaq.Key,
                    CantidadFallas1 = ft1,
                    CantidadFallas2 = ft2,
                    CantidadFallas3 = ft3,
                    TiempoMuerto1 = Math.Round(sumatm1, 2),
                    TiempoMuerto2 = Math.Round(sumatm2, 2),
                    TiempoMuerto3 = Math.Round(sumatm3, 2),
                    MTTR1 = Math.Round(sumamttr1, 2),
                    MTTR2 = Math.Round(sumamttr2, 2),
                    MTTR3 = Math.Round(sumamttr3, 2),
                    MTBF = Math.Round(sumamtbf, 2),
                    Confiabilidad = Math.Round(sumaMant, 2),
                    TarjetasTPM = 0,
                    Disponible1 = Math.Round(sumd1, 2),
                    Disponible2 = Math.Round(sumd2, 2),
                    Disponible3 = Math.Round(sumd3, 2)
                };
                Ldata3.Add(data_show);
            }

            ViewBag.metricospermachine = Ldata3.ToList();

            var allmaqbyday = allmaq.GroupBy(g => g.TiempoLabel).ToList();

            foreach (var inmaq in allmaqbyday)
            {
                var sumd1 = inmaq.Sum(s => s.Disponible1);
                var sumd2 = inmaq.Sum(s => s.Disponible2);
                var sumd3 = inmaq.Sum(s => s.Disponible3);
                var ft1 = inmaq.Sum(s => s.CantidadFallas1);
                var ft2 = inmaq.Sum(s => s.CantidadFallas2);
                var ft3 = inmaq.Sum(s => s.CantidadFallas3);
                var sumtp1 = inmaq.Sum(s => s.TiempoMuerto1);
                var sumtp2 = inmaq.Sum(s => s.TiempoMuerto2);
                var sumtp3 = inmaq.Sum(s => s.TiempoMuerto3);
                var sumatm1 = ((sumtp1 / sumd1) * 100);
                var sumatm2 = ((sumtp2 / sumd2) * 100);
                var sumatm3 = ((sumtp3 / sumd3) * 100);
                var smttr11 = inmaq.Sum(s => s.MTTR1);
                var smttr12 = inmaq.Sum(s => s.MTTR2);
                var smttr13 = inmaq.Sum(s => s.MTTR3);
                var sumamttr1 = smttr11 / inmaq.Count();
                var sumamttr2 = smttr12 / inmaq.Count();
                var sumamttr3 = smttr13 / inmaq.Count();
                var sumamtbf = inmaq.Sum(s => s.MTBF) / inmaq.Count();
                // var sumamtbf = ((sumd1 + sumd2 + sumd3) / 1);
                var sumamttrs = sumamttr1 + sumamttr2 + sumamttr3;
                var sumaMant = ((sumamtbf - sumamttrs) / sumamtbf) * 100;

                //tpm
                int realizada = 0;
                int pendiente = 0;
                var alloils = oILs.Where(s => s.Tipo == "TPM").ToList();
                if (!string.IsNullOrEmpty(amaquina) && amaquina != "--Todas--")
                {
                    alloils = alloils.Where(s => s.Maquinas.Area == amaquina).ToList();
                }
                foreach (OILs oil in alloils)
                {
                    if (oil.DiaHora.Year <= fecha.Year)
                        pendiente++;

                    if (oil.Estatus == 1 && oil.DiaHora_Cierre.Value.Year <= fecha.Year)
                        realizada++;
                }

                var tpmt = Math.Round((realizada / (Double)pendiente) * 100, 2);

                newmetricos2 data_show = new newmetricos2
                {
                    TiempoLabel = inmaq.Key,
                    TiempoMuerto1 = Math.Round(sumatm1, 2),
                    TiempoMuerto2 = Math.Round(sumatm2, 2),
                    TiempoMuerto3 = Math.Round(sumatm3, 2),
                    MTTR1 = Math.Round(sumamttr1, 2),
                    MTTR2 = Math.Round(sumamttr2, 2),
                    MTTR3 = Math.Round(sumamttr3, 2),
                    MTBF = Math.Round(sumamtbf, 2),
                    Confiabilidad = Math.Round(sumaMant, 2),
                    TarjetasTPM = tpmt,
                    Disponible1 = Math.Round(sumd1, 2),
                    Disponible2 = Math.Round(sumd2, 2),
                    Disponible3 = Math.Round(sumd3, 2),
                    FallasT1 = Math.Round(ft1, 2),
                    FallasT2 = Math.Round(ft2, 2),
                    FallasT3 = Math.Round(ft3, 2),
                    TiempoM1 = Math.Round(sumtp1, 2),
                    TiempoM2 = Math.Round(sumtp2, 2),
                    TiempoM3 = Math.Round(sumtp3, 2)
                };
                Ldata.Add(data_show);
            }

            // Grafica de tiempo  --------------------------------------------

            string labels = "'";
            string gdata = "";
            string gdata2 = "";
            List<decimal> x = new List<decimal>();
            List<decimal> y = new List<decimal>();
            int j = 0;

            foreach (var h in Ldata)
            {
                labels = labels + h.TiempoLabel + "','";

                gdata = gdata + Convert.ToString(h.TiempoM1 + h.TiempoM2 + h.TiempoM3) + ",";

                j++;
                x.Add(j);
                if ((h.TiempoM1 + h.TiempoM2 + h.TiempoM3) != 0)
                    y.Add(Convert.ToDecimal(h.TiempoM1 + h.TiempoM2 + h.TiempoM3));
                else
                    y.Add(0);
            }

            if (x.Count > 1)
            {
                Trendline rvalue = new Trendline(y, x);
                for (int i = 0; i <= x.Count; i++)
                {
                    var tt = i;
                    var res = rvalue.GetYValue(tt);
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }
            }

            labels = labels.TrimEnd(',', (char)39);
            labels = labels + "'"; labels = labels.Replace("\r\n", "");
            gdata = gdata.TrimEnd(',', (char)39);
            gdata2 = gdata2.TrimEnd(',', (char)39);
            labels = labels.Replace('-', '_');

            ViewBag.labelsgrap = labels;// Label
            ViewBag.datasgrap = gdata; // Suma de timepo 
            ViewBag.data2sgrap = gdata2;// % acumulado

            // Pareto --------------------------------------------

            var groupdata = fulldatafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo)).Take(5);
            labels = "'";
            gdata = "";
            gdata2 = "";
            int stempt = 0;
            int stemp = fulldatafiltered.Sum(s => s.Tiempo);
            foreach (var h in groupdata)
            {
                labels = labels + h.Key + "','";
                gdata = gdata + h.Sum(s => s.Tiempo).ToString() + ",";
                stemp = stemp + h.Sum(s => s.Tiempo);
                double res = (stemp / (double)stempt) * 100;
                gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
            }

            labels = labels.TrimEnd(',', (char)39);
            labels = labels + "'"; labels = labels.Replace("\r\n", "");
            gdata.TrimEnd(',', (char)39);
            gdata2.TrimEnd(',', (char)39);

            ViewBag.labelsgrap2 = labels;
            ViewBag.datasgrap2 = gdata;
            ViewBag.data2sgrap2 = gdata2;

            //--------------------------------------------

            //*******************************************************************************************
            if (btn == "Metricos por Años")
            {
                labels = "'";
                gdata = "";
                gdata2 = "";
                //List<decimal> x = new List<decimal>();
                //List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();
                // datafiltered = data;
                var datatempa = fulldatafiltered.GroupBy(g => g.DiaHora.Year).ToList();
                datatempa = datatempa.OrderBy(o => o.Key).ToList();

                for (int i = fecha.Year; i <= fechaf.Year; i++)
                {
                    var dd = datatempa.Where(w => w.Key == i).ToList();
                    labels = labels + i.ToString() + "','";
                    if (dd.Count != 0)
                        gdata = gdata + dd.FirstOrDefault().Sum(s => s.Tiempo).ToString() + ",";
                    else
                        gdata = gdata + "0,";

                    x.Add(i);
                    if (dd.Count != 0)
                        y.Add(dd.FirstOrDefault().Count());
                    else
                        y.Add(0);
                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    for (int i = fecha.Year; i <= fechaf.Year; i++)
                    {
                        var tt = fecha.Year;
                        var res = rvalue.GetYValue(tt);
                        gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                    }
                }
                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                groupdata = fulldatafiltered.GroupBy(g => g.Maquinas.Maquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;
                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;

                stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                stemp = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    labels = labels + hdata[di].Key + "','";
                    double eltiempotemp = hdata[di].Sum(s => s.Tiempo);
                    double re1 = (eltiempotemp / (double)eltiempo) * 100;
                    gdata = gdata + string.Format("{0:0.##}", re1) + ",";
                    stemp = stemp + hdata[di].Sum(s => s.Tiempo);
                    double res = (stemp / (double)stempt) * 100;
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }

                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(fecha.Month);
                ViewBag.dx = " Año:" + fecha.AddYears(-5).ToString("yyyy") + " a el Año:" + fecha.ToString("yyyy");
                ViewBag.dxx = " Anual";
            }
            //*******************************************************************************************

            if (btn == "Metricos por Mes")
            {
                datafiltered.Clear();
                labels = "'";
                gdata = "";
                gdata2 = "";
                //List<decimal> x = new List<decimal>();
                //List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();

                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        if (
                             ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                             ||
                              ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                            ||
                              ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                            ||
                            (iaño != fecha.Year && iaño != fechaf.Year)
                            )
                        {
                            labels = labels + iaño.ToString() + "-" + nombreMes + "','";

                            var temp = dataaño.Where(w => w.DiaHora.Year == iaño && w.DiaHora.Month == jmes);
                            datafiltered.AddRange(temp);
                            gdata = gdata + temp.Sum(s => s.Tiempo).ToString() + ",";
                            x.Add(i);
                            y.Add(temp.Sum(s => s.Tiempo));
                            i++;
                        }
                    }
                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    i = 1;
                    for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                    {
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            if (
                                  ((fecha.Year == fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month) && (jmes <= fechaf.Month))
                                 ||
                                  ((fecha.Year != fechaf.Year) && (fecha.Year == iaño) && (jmes >= fecha.Month))
                                ||
                                  ((fecha.Year != fechaf.Year) && (fechaf.Year == iaño) && (jmes <= fechaf.Month))
                                ||
                                (iaño != fecha.Year && iaño != fechaf.Year)
                                )
                            {
                                var res = rvalue.GetYValue(i);
                                gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                                i++;
                            }
                        }
                    }
                }
                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);

                ViewBag.labelsgrap = labels;
                ViewBag.datasgrap = gdata;
                ViewBag.data2sgrap = gdata2;
                //--------------------------------------------------------
                groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                labels = "'";
                gdata = "";
                gdata2 = "";
                List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                int exit = 0;
                int eltiempo = 0;
                List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                foreach (var h in groupdata)
                {
                    newmetricosmaquina nmm = new newmetricosmaquina();
                    eltiempo += h.Sum(s => s.Tiempo);
                    if (exit < 5)
                    {
                        hdata.Add(h);
                        nmm.tiempod = multiplicador;
                        nmm.maquina = h.Key;
                        nmm.tiempof = h.Sum(s => s.Tiempo);
                        nmm.fallas = h.Count();
                        nmm.mttr = nmm.tiempof / nmm.fallas;
                        nmm.mtbf = multiplicador / nmm.fallas;
                        var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                        nmm.confiabilidad = disponibilidad;
                        lnmm.Add(nmm);
                    }
                    exit++;
                }
                ViewBag.lnmm = lnmm;

                var objet = "'";
                var dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mttr.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmttr = objet;
                ViewBag.Datamttr = dat;
                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.tiempof.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelstf = objet;
                ViewBag.Datatf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.fallas.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsf = objet;
                ViewBag.Dataf = dat;

                objet = "'";
                dat = "";
                foreach (var item in lnmm)
                {
                    objet = objet + item.maquina.ToString() + "','";
                    dat = dat + item.mtbf.ToString() + ",";
                }

                objet = objet.TrimEnd(',', (char)39);
                objet = objet + "'";
                dat = dat.TrimEnd(',', (char)39);

                ViewBag.labelsmtbf = objet;
                ViewBag.Datamtbf = dat;

                stempt = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    stempt += hdata[di].Sum(s => s.Tiempo);

                }

                stemp = 0;
                for (int di = 0; di < hdata.Count; di++)
                {
                    labels = labels + hdata[di].Key + "','";
                    double eltiempotemp = hdata[di].Sum(s => s.Tiempo);
                    double re1 = (eltiempotemp / (double)eltiempo) * 100;
                    gdata = gdata + string.Format("{0:0.##}", re1) + ",";
                    stemp = stemp + hdata[di].Sum(s => s.Tiempo);
                    double res = (stemp / (double)stempt) * 100;
                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                }

                labels = labels.TrimEnd(',', (char)39);
                labels = labels + "'"; labels = labels.Replace("\r\n", "");
                gdata = gdata.TrimEnd(',', (char)39);
                gdata2 = gdata2.TrimEnd(',', (char)39);
                ViewBag.labelsgrap2 = labels;
                ViewBag.datasgrap2 = gdata;
                ViewBag.data2sgrap2 = gdata2;
                DateTimeFormatInfo formatoFecha2 = CultureInfo.CurrentCulture.DateTimeFormat;
                nombreMes1 = formatoFecha2.GetMonthName(fecha.Month);
                nombreMes2 = formatoFecha2.GetMonthName(fechaf.Month);
                ViewBag.dx = "Mes " + nombreMes1 + " al Mes " + nombreMes2;
                ViewBag.dxx = " Mes";
            }
            //*******************************************************************************************

            if (btn == "Metricos por Dia")
            {
                datafiltered.Clear();
                labels = "'";
                gdata = "";
                gdata2 = "";
                // List<decimal> x = new List<decimal>();
                // List<decimal> y = new List<decimal>();
                x.Clear();
                y.Clear();
                int i = 1;
                for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                {
                    var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
                    for (int jmes = 1; jmes <= 12; jmes++)
                    {
                        DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                        string nombreMes = formatoFecha.GetMonthName(jmes);
                        var dias = DateTime.DaysInMonth(iaño, jmes);
                        var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                        for (int kdia = 1; kdia <= dias; kdia++)
                        {
                            DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());

                            /*if (idi >= fecha && idi <= fechaf)
                            {
                                labels = labels + iaño.ToString() + "-" + nombreMes + "-" + kdia.ToString() + "','";
                                var temp = datames.Where(w => w.DiaHora.Day == kdia);
                                datafiltered.AddRange(temp);
                                gdata = gdata + temp.Sum(s => s.Tiempo).ToString() + ",";
                                x.Add(i);
                                y.Add(temp.Sum(s => s.Tiempo));
                                i++;
                             }*/
                        }
                    }
                }
                if (x.Count > 1)
                {
                    Trendline rvalue = new Trendline(y, x);
                    i = 1;

                    for (int iaño = fecha.Year; iaño <= fechaf.Year; iaño++)
                    {
                        var dataaño = fulldatafiltered.Where(w => w.DiaHora.Year == iaño);
                        for (int jmes = 1; jmes <= 12; jmes++)
                        {
                            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
                            string nombreMes = formatoFecha.GetMonthName(jmes);
                            var dias = DateTime.DaysInMonth(iaño, jmes);
                            var datames = dataaño.Where(w => w.DiaHora.Month == jmes);
                            for (int kdia = 1; kdia <= dias; kdia++)
                            {
                                DateTime idi = Convert.ToDateTime(kdia.ToString() + "/" + jmes.ToString() + "/" + iaño.ToString());
                                if (idi >= fecha && idi <= fechaf)
                                {
                                    var res = rvalue.GetYValue(i);
                                    gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                                    i++;
                                }
                            }
                        }
                    }

                    labels = labels.TrimEnd(',', (char)39);
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata = gdata.TrimEnd(',', (char)39);
                    gdata2 = gdata2.TrimEnd(',', (char)39);

                    ViewBag.labelsgrap = labels;
                    ViewBag.datasgrap = gdata;
                    ViewBag.data2sgrap = gdata2;
                    //--------------------------------------------------------
                    groupdata = datafiltered.GroupBy(g => g.Maquinas.SubMaquina).OrderByDescending(o => o.Sum(s => s.Tiempo));
                    labels = "'";
                    gdata = "";
                    gdata2 = "";

                    List<IGrouping<String, Bitacora>> hdata = new List<IGrouping<String, Bitacora>>();
                    int exit = 0;
                    int eltiempo = 0;
                    List<newmetricosmaquina> lnmm = new List<newmetricosmaquina>();
                    foreach (var h in groupdata)
                    {
                        newmetricosmaquina nmm = new newmetricosmaquina();
                        eltiempo += h.Sum(s => s.Tiempo);
                        if (exit < 5)
                        {
                            hdata.Add(h);
                            nmm.tiempod = multiplicador;
                            nmm.maquina = h.Key;
                            nmm.tiempof = h.Sum(s => s.Tiempo);
                            nmm.fallas = h.Count();
                            nmm.mttr = nmm.tiempof / nmm.fallas;
                            nmm.mtbf = multiplicador / nmm.fallas;
                            var disponibilidad = nmm.mtbf / (nmm.mtbf + nmm.mttr);
                            nmm.confiabilidad = disponibilidad;
                            lnmm.Add(nmm);
                        }
                        exit++;
                    }
                    ViewBag.lnmm = lnmm;

                    var objet = "'";
                    var dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mttr.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmttr = objet;
                    ViewBag.Datamttr = dat;
                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.tiempof.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelstf = objet;
                    ViewBag.Datatf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.fallas.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsf = objet;
                    ViewBag.Dataf = dat;

                    objet = "'";
                    dat = "";
                    foreach (var item in lnmm)
                    {
                        objet = objet + item.maquina.ToString() + "','";
                        dat = dat + item.mtbf.ToString() + ",";
                    }

                    objet = objet.TrimEnd(',', (char)39);
                    objet = objet + "'";
                    dat = dat.TrimEnd(',', (char)39);

                    ViewBag.labelsmtbf = objet;
                    ViewBag.Datamtbf = dat;

                    stempt = 0;
                    for (int di = 0; di < hdata.Count; di++)
                    {
                        stempt += hdata[di].Sum(s => s.Tiempo);

                    }

                    stemp = 0;
                    for (int di = 0; di < hdata.Count; di++)
                    {
                        labels = labels + hdata[di].Key + "','";
                        double eltiempotemp = hdata[di].Sum(s => s.Tiempo);
                        double re1 = (eltiempotemp / (double)eltiempo) * 100;
                        gdata = gdata + string.Format("{0:0.##}", re1) + ",";
                        stemp = stemp + hdata[di].Sum(s => s.Tiempo);
                        double res = (stemp / (double)stempt) * 100;
                        gdata2 = gdata2 + string.Format("{0:0.##}", res) + ",";
                    }

                    labels = labels.TrimEnd(',', (char)39);
                    labels = labels + "'"; labels = labels.Replace("\r\n", "");
                    gdata = gdata.TrimEnd(',', (char)39);
                    gdata2 = gdata2.TrimEnd(',', (char)39);
                    ViewBag.labelsgrap2 = labels;
                    ViewBag.datasgrap2 = gdata;
                    ViewBag.data2sgrap2 = gdata2;

                    ViewBag.dx = "Dia " + fecha.ToString("dd") + " al Dia " + fechaf.ToString("dd");
                    ViewBag.dxx = " Dia";
                }
            }

            var id = User.Identity.GetUserId();
            ApplicationUser currentUser = UserManager.FindById(id);
            string cuser = "xxx";
            string cpuesto = "xxx";
            string cuare = "xxx";
            if (currentUser != null)
            {
                cuser = currentUser.UserFullName;
                cpuesto = currentUser.Puesto;
                cuare = currentUser.Area;
            }
            ViewBag.uarea = cuare;
            ViewBag.cuser = cuser;
            if (cpuesto.Contains("Supervisor") || cpuesto.Contains("Asistente") || cpuesto.Contains("SuperIntendente") || cpuesto.Contains("Gerente"))
                ViewBag.super = true;
            else
                ViewBag.super = false;
            //--------------------------------------------
            ViewBag.data = Ldata;

            ViewBag.datei = fecha.ToString("dd/MM/yyyy");
            ViewBag.datef = fechaf.ToString("dd/MM/yyyy");

            return View(await db.Metricos.ToListAsync());
        }

        // GET: Metricos1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // GET: Metricos1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Metricos1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            if (ModelState.IsValid)
            {
                db.Metricos.Add(metricos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(metricos);
        }

        // GET: Metricos1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // POST: Metricos1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DiaHora,Usuario,Usuario_area,Usuario_puesto,Usuario_responsable,Descripcion,Comentarios,Proyectos")] Metricos metricos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metricos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(metricos);
        }

        // GET: Metricos1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metricos metricos = await db.Metricos.FindAsync(id);
            if (metricos == null)
            {
                return HttpNotFound();
            }
            return View(metricos);
        }

        // POST: Metricos1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Metricos metricos = await db.Metricos.FindAsync(id);
            db.Metricos.Remove(metricos);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
