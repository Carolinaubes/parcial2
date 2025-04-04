//using System.Collections.Generic;
//using System.IO;
//using System.Net.Http;
//using System.Net;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web;
//using System;


using Examen2.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Net;
using System.Linq;

namespace Examen2.Clases
{
    public class clsUpload
    {
        public int IdPrenda { get; set; } // Recibir  el id de la prenda a la que se asociará la imagen
        public HttpRequestMessage request { get; set; } // Solicitud HTTP que contiene el archivo

        private DBExamenEntities1 dbexamen = new DBExamenEntities1(); // Contexto de la base de datos
        private string carpeta = HttpContext.Current.Server.MapPath("~/Archivos"); // Ruta donde se guardaran las iamgenes

        public async Task<HttpResponseMessage> CargarArhivo (bool actualizar)
        {
            try
            {
                if (!request.Content.IsMimeMultipartContent())
                {
                    return request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "No se recibió un archivo");
                }

                var provider = new MultipartFormDataStreamProvider(carpeta);
                await request.Content.ReadAsMultipartAsync(provider); // Procesar la solicitud y extraer el archivo

                // se verifica si se recibio como minimo un archivo
                if (provider.FileData.Count == 0)
                {
                    return request.CreateErrorResponse(HttpStatusCode.BadRequest, "No se envió un archivo");
                }

                var file = provider.FileData[0]; // Obtener el primer archivo subido
                string fileName = Path.GetFileName(file.Headers.ContentDisposition.FileName.Trim('"')); // se obtiene el nombre del archivo
                string rutaCompleta = Path.Combine(carpeta, fileName); // Crea la ruta completa del archivo

                // Verificar la existencia del archivo en la carpeta
                if (File.Exists(rutaCompleta))
                {
                    if (actualizar)
                    {
                        File.Delete(rutaCompleta); // eliminar el archivo existente
                        File.Move(file.LocalFileName, rutaCompleta); // mueve el nuevo archivo a la ubicacion 
                        return request.CreateErrorResponse(HttpStatusCode.OK, "La imagen se actualizó correctamente");
                    }
                    else
                    {
                        File.Delete(file.LocalFileName); // elimina el archivo temporal en caso de que ya exista y no se quiera actualizar
                    }
                } else
                {
                    File.Move(file.LocalFileName,rutaCompleta); // guarda el archivo en la carpeta, ya no será más temporal
                }

                // guardar la imagen en la base de datos
                FotoPrenda fotoprenda = dbexamen.FotoPrendas.FirstOrDefault(f => f.idPrenda == IdPrenda);

                if (fotoprenda == null)
                {
                    // Si no existe una imagen asociada a la prenda, se crea una 
                    fotoprenda = new FotoPrenda()
                    {
                        idPrenda = IdPrenda,
                        FotoPrenda1 = fileName
                    };
                    dbexamen.FotoPrendas.Add(fotoprenda);
                } else
                {
                    // Si ya existe una imagen, actualizamos el nombre del archivo
                    fotoprenda.FotoPrenda1 = fileName;
                    dbexamen.Entry(fotoprenda).State = System.Data.Entity.EntityState.Modified; // Indicar que la entidad de entity framework ha sido modificada
                }
                dbexamen.SaveChanges(); // guardar cambios en la base de datos
                return request.CreateResponse(HttpStatusCode.OK, "Imagen guardada en la base de datos exitosamente");
           
            } catch (Exception ex)
            {
                return request.CreateErrorResponse(System.Net.HttpStatusCode.InternalServerError, ex.Message); // Mensaje en caso de error
            }
        }
    }
}