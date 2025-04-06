using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Examen2.Clases;

namespace Examen2.Controllers
{
    [RoutePrefix("api/FotoPrenda")]
    public class FotoPrendaController : ApiController
    {

        [HttpPut]
        [Route("InsertarPrendaCliente")]
        public string InsertarPrendaCliente(int idCliente, string documento, string nombre, string email, string celular, string tipoPrenda, string descripcion, float valor)
        {
            clsPrenda Prenda = new clsPrenda();

            return Prenda.InsertarPrendaCliente(idCliente, documento, nombre, email, celular, tipoPrenda, descripcion, valor);
        }

        [HttpGet]
        [Route("ListarPrendasXCliente")]
        public IQueryable ListarPrendasXCliente(string documentoCliente)
        {
            clsPrenda prenda = new clsPrenda();
            return prenda.ListarPrendasXCliente(documentoCliente);
        }

        [HttpPost]
        [Route("CargarImagenPrenda")]
        public async Task<HttpResponseMessage> CargarImagenPrenda(HttpRequestMessage request, int idPrenda)
        {
            clsUpload upload = new clsUpload();
            upload.request = request;
            upload.IdPrenda = idPrenda;

            return await upload.CargarArhivo(false); // False par indicar que se trata de una inserción, no una actualización 
        }

        [HttpPut]
        [Route("ActualizarImagenPrenda")] 
        public async Task<HttpResponseMessage> ActualizarImagenPrenda (HttpRequestMessage request, int idPrenda){ 
            clsUpload upload = new clsUpload();
            upload.request = request;
            upload.IdPrenda = idPrenda;
            upload.request = Request;

            return await upload.CargarArhivo(true); // True para indicar que se trata de una actualizaci´´on
        }

        [HttpDelete]
        [Route("EliminarImagen")]
        public string EliminarImagen(string nombreArchivo)
        {
            clsPrenda prenda = new clsPrenda();
            return prenda.EliminarImagen(nombreArchivo);
        }
    }
}
