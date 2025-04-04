using Examen2.Clases;
using Examen2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Examen2.Controllers
{
    [RoutePrefix("api/Clientes")]
    public class ClientePrendaController : ApiController
    {
    
        //DELETE: Se utiliza para eliminar información en la base de datos
        [HttpGet] //Es el servicio que se va a exponer: GET, POST, PUT, DELETE
        [Route("ConsultarTodos")] //Es el nombre de la funcionalidad que se va a ejecutar
        public List<Cliente> ConsultarTodos()
        {
            //Se crea una instancia de la clase clsEmpleado
            clsCliente Cliente = new clsCliente();
            //Se invoca el método ConsultarTodos() de la clase clsEmpleado
            return Cliente.ConsultarTodos();
        }

        [HttpPost]
        [Route("Insertar")]
        public string Insertar([FromBody] Cliente cliente)
        {
            //Se crea una instancia de la clase clsEmpleado
            clsCliente cli = new clsCliente();
            //Se pasa la propieadad empleado al objeto de la clases clsEmpleado
            cli.cliente = cliente;
            //Se invoca el método insertar
            return cli.Insertar();
        }

        [HttpPut]
        [Route("Actualizar")]
        public string Actualizar([FromBody] Cliente cliente)
        {
            clsCliente cli = new clsCliente();
            cli.cliente = cliente;
            return cli.Actualizar();
        }

        [HttpDelete]
        [Route("EliminarXId")]
        public string EliminarXId(int id)
        {
            clsCliente Cliente= new clsCliente();
            return Cliente.Eliminar(id);
        }
    }
}