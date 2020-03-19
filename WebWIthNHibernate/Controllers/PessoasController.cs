using Microsoft.AspNetCore.Mvc;
using NHibernate;
using WebWIthNHibernate.Models;

namespace WebWIthNHibernate.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly ISession _session;

        public PessoasController(ISession session)
            => _session = session;

        [HttpGet(Name = "GetAll")]
        [EnableQueryCustom]
        public IActionResult Get()
        {
            return Ok(_session.Query<Pessoa>());
        }

        [HttpPost]
        public IActionResult Post(Pessoa pessoa)
        {
            _session.Persist(pessoa);
            _session.Flush();
            return Created(Url.Link("GetAll", null), pessoa);
        }
    }
}
