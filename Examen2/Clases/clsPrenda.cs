
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Examen2.Models;

namespace Examen2.Clases
{
    public class clsPrenda
    {
        private DBExamenEntities1 dbexamen = new DBExamenEntities1();
        private clsCliente cliente = new clsCliente();

        //El insertar va a grabar la prenda y el cliente
        public int InsertarPrendaCliente(int idCliente, string documento, string nombre, string email, string celular, string tipoPrenda, string descripcion, float valor)
        {
            try
            {
                // verificar si el cliente existe en la bse de datos
                Cliente objCliente = cliente.Consultar(idCliente);

                if (objCliente == null) // El cliente no existe en la base de datos, toca crearlo
                {
                    objCliente = new Cliente
                    {
                        Documento = documento,
                        Nombre = nombre,
                        Email = email,
                        Celular = celular
                    };

                    //Insertar el nuevo cliente
                    cliente.cliente = objCliente;
                    cliente.Insertar();

                    // Volver a consultar el cliente recién insertado para verificar si se insertó correctamente o no
                    objCliente = cliente.Consultar(objCliente.idCliente);

                    if (objCliente == null)
                    {
                        throw new Exception("Error al insertar el cliente");
                    }

                    Prenda objPrenda = new Prenda
                    {
                        TipoPrenda = tipoPrenda,
                        Descripcion = descripcion,
                        Valor = valor,
                        Cliente = objCliente.Documento
                    };

                    dbexamen.Prendas.Add(objPrenda);
                    dbexamen.SaveChanges(); // Confirmar y guardar los cambios en la base de datos

                    return objPrenda.IdPrenda; // retornar el id de la prenda recien insertada
                }
                throw new Exception("Mensaje para validr");

            } catch (Exception ex)
            {
                throw new Exception("Error al guardar la prenda: " + ex.Message);
            }
        }

        // Metodo para eliminar una imagen segun su nombre (FotoPrenda) --> SOLO LA ELIMINA DE LA CARPETA, MÁS NO DE LA BASE DE DATOS
        public string EliminarImagen(string nombreArchivo)
        {
            try
            {
                // ruta completa en donde se encuentra el archivo
                string rutaCarpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Archivos");
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo); // Combino la ruta de la carpeta con la ruta completa del archivo

                if (File.Exists(rutaCompleta))
                {
                    File.Delete(rutaCompleta);
                    return "La imagen se eliminó correctamente";
                } else
                {
                    return "La imagen no existe";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al intentar eliminar la imagen: " + ex.Message);
            }
        }

        // Metodo para obtener las prendas asociadas a un cliente en especifico
        public IQueryable ListarPrendasXCliente(string documento) // Es necesario que reciba eldocumento porque en la bd ya está de esa manera
        {
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
           };

        }

       
    }
}