using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;

namespace ConsoleTransito
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string sDias = ConfigurationManager.AppSettings["DiasDeProceso"];
            string sUri = ConfigurationManager.AppSettings["Uri"];
            int intDias = Convert.ToInt32(sDias);

            //Proceso del dia de la fecha a 7 dias para adelante
            for (int i = 0; i < intDias; i++)
            {
                string sFecha = DateTime.Now.AddDays(i).ToString("dd/MM/yyyy");

                Processes processesInicio = new Processes
                {
                    FechaInicio = DateTime.Now,
                    Name = "CONSOLE_TRANSITO",
                    Detalle = sFecha


                };
                db.Processes.Add(processesInicio);
                db.SaveChanges();


                
                                
                using (var client = new WebClient()) //WebClient  
                {
                    client.Headers.Add("Content-Type:application/json"); //Content-Type  
                    client.Headers.Add("Accept:application/json");


                    //var result = client.DownloadString(sUri + sFecha); //URI  
                    var htmlData = client.DownloadData(sUri + sFecha); //URI  
                    var htmlCode = Encoding.UTF8.GetString(htmlData);
                    //IEnumerable<Turn> json = JsonConvert.DeserializeObject<IEnumerable<Turn>>(result);
                    IEnumerable<Turn> json = JsonConvert.DeserializeObject<IEnumerable<Turn>>(htmlCode);




                    foreach (var item in json)
                    {
                        try
                        {
                            //Primero verifico si el turno ya esta guardado

                            //  bool existCall = db.CallCenterTurns.Where(x => x.Gestion == item.Gestion.ToString()).Any();

                            CallCenterTurns call = db.CallCenterTurns.Where(x => x.Gestion == item.Gestion.ToString()).FirstOrDefault();

                            if (call==null)
                            {
                                CallCenterTurns callCenterTurns = new CallCenterTurns
                                {
                                    DNI = item.Dni.ToString(),
                                    Apellido = item.Apellido.ToUpper().Trim(),
                                    Nombre = item.Nombre.Trim().ToUpper(),
                                    Asignado = false,
                                    Barrio = item.Barrio,
                                    Estado = item.Estado,
                                    Fecha = DateTime.Now,
                                    FechaTurno = Convert.ToDateTime( item.Turno),
                                    Gestion = item.Gestion.ToString(),
                                    Tel_Celular = item.Tel_Celular,
                                    Tel_Particular = item.Tel_Particular,
                                    TipoTramite = item.Motivo.Trim().ToUpper(),
                                    Vencimiento_licencia = item.Vencimiento_Licencia

                                };

                                db.CallCenterTurns.Add(callCenterTurns);

                                db.SaveChanges();

                            }
                            else //Lo actualizo salvo que alla sido modificado
                            {
                                if (call.FechaModificacion==null)
                                {
                                    call.DNI = item.Dni.ToString();
                                    call.Apellido = item.Apellido.ToUpper().Trim();
                                    call.Nombre = item.Nombre.Trim().ToUpper();
                                    call.Asignado = false;
                                    call.Barrio = item.Barrio;
                                    call.Estado = item.Estado;
                                    call.Fecha = DateTime.Now;
                                    call.FechaTurno = Convert.ToDateTime(item.Turno);
                                    call.Gestion = item.Gestion.ToString();
                                    call.Tel_Celular = item.Tel_Celular;
                                    call.Tel_Particular = item.Tel_Particular;
                                    call.TipoTramite = item.Motivo.Trim().ToUpper();
                                    call.Vencimiento_licencia = item.Vencimiento_Licencia;

                                    db.Entry(call).State = EntityState.Modified;
                                    db.SaveChanges();
                                }


                            }




                        }
                        catch (Exception e)
                        {

                            ProcessLogs logs = new ProcessLogs
                            {
                                IsOk = false,
                                ErrorDescripcion = item.Gestion + " - " + e.Message,
                                Fecha = DateTime.Now,
                                Name = "CONSOLE_TRANSITO",
                                ProcessId = processesInicio.Id

                            };
                            db.ProcessLogs.Add(logs);
                            db.SaveChanges();
                        }





                    }

                    if (processesInicio.Id >= 0)
                    {
                        Processes processesFin = db.Processes.Find(processesInicio.Id);
                        processesFin.FechaFin = DateTime.Now;
                        db.Entry(processesFin).State = EntityState.Modified;
                        db.SaveChanges();

                    }



                }
            } 
        }



        private void Proceso (string sFecha)
        {


        }

    }
}
