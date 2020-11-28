using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Entidades;
using Servicios;
namespace WebApiDonaciones.Controllers
{
    public class HistorialDonacionesController : ApiController
    {
        // GET: api/HistorialDonaciones
        private DonacionService _donacionService = new DonacionService();
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/HistorialDonaciones/1
        public List<DonacionAux> Get(int id)
        {

            return _donacionService.MisDonacionesId(id);

        }

        // POST: api/HistorialDonaciones
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/HistorialDonaciones/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/HistorialDonaciones/5
        public void Delete(int id)
        {
        }
    }
}
