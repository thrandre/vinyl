using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Vinyl.Actions;
using Vinyl.Models;
using Vinyl.Models.Wrappers;

namespace Vinyl.Controllers
{
    [Route("api/[controller]")]
    public class RecordController : Controller
    {
        private GetRecordsAction GetRecordsAction { get; }

        public RecordController(GetRecordsAction getRecordsAction)
        {
            GetRecordsAction = getRecordsAction;
        }

        [HttpGet]
        public List<Record> GetAll()
        {
            return GetRecordsAction.Execute();
        }

        [HttpGet("{id}")]
        public Record GetById([FromRoute] RecordId id)
        {
            return GetRecordsAction.Execute(id);
        }
    }
}
