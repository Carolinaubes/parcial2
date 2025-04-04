using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Examen2.Models;


namespace Examen2.Clases
{
    public class clsCliente
    {
        private DBExamenEntities1 dbexamen = new DBExamenEntities1();
        public Cliente cliente { get; set; } = new Cliente();
        public string Insertar()
        {
            try
            {
                dbexamen.Clientes.Add(cliente);
                dbexamen.SaveChanges();
                return "Se ingresó el cliente " + cliente.Nombre + " a la base de datos.";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string Actualizar()
        {
            try
            {
                Cliente cli = Consultar(cliente.idCliente);
                if (cli == null)
                {
                    return "El cliente no existe";
                }
                dbexamen.Clientes.AddOrUpdate(cliente);
                dbexamen.SaveChanges();
                return "Se actualizó el cliente con id #" + cliente.idCliente + " correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public Cliente Consultar(int id)
        {
            Cliente cli = dbexamen.Clientes.FirstOrDefault(c => c.idCliente == id);
            return cli;
        }

        public List<Cliente> ConsultarTodos()
        {
            return dbexamen.Clientes
                .OrderBy(c => c.Nombre)
                .ToList();
        }

        public string Eliminar(int id)
        {
            try
            {
                Cliente cli = Consultar(id);
                if (cli == null)
                {
                    return "El cliente no existe";
                }
                dbexamen.Clientes.Remove(cli);
                dbexamen.SaveChanges();
                return "Se eliminó el cliente " + cli.Nombre + " correctamente";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // Metodo que retorna una IQueryable con la lista de prendas    
        public IQueryable ListarPrendasXCliente(string documento)
        { // Info del cliente, datos de las prendas y nombres de las imagenes
            return from C in dbexamen.Set<Cliente>()
                   join P in dbexamen.Set<Prenda>()
                   on C.Documento equals P.Cliente
                   join FP in dbexamen.Set<FotoPrenda>()
                   on P.IdPrenda equals FP.idPrenda
                   where C.Documento.Equals(documento)
                   orderby P.Descripcion
                   select new
                   {
                       DocumentoCliente = C.Documento,
                       NombreCliente = C.Nombre,
                       EmailCliente = C.Email,
                       CelularCliente = C.Celular,
                       IdPrenda = P.IdPrenda,
                       TipoPrenda = P.TipoPrenda,
                       DescripcionPrenda = P.Descripcion,
                       ValorPrenda = P.Valor,
                       IdFoto = FP.idFoto,
                       FotoPrenda = FP.FotoPrenda1,
                       IdPrendaFoto = FP.idPrenda
                   };  // Izquierda codigo C# - Derecha SQL
        }
    }

}