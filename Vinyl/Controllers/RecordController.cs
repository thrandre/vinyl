using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Vinyl.Actions;
using Vinyl.Models;
using Vinyl.Models.Wrappers;

namespace Vinyl.Controllers
{
    [Route("api/[controller]")]
    public class RecordController : Controller
    {
        private GetRecordsAction GetRecordsAction { get; }
        private CreateRecordAction CreateRecordAction { get; }

        public RecordController
        (
            GetRecordsAction getRecordsAction,
            CreateRecordAction createRecordAction
        )
        {
            GetRecordsAction = getRecordsAction;
            CreateRecordAction = createRecordAction;
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

        [HttpPost]
        public void CreateRecord([FromBody] Record record)
        {
            CreateRecordAction.Execute(record);
        }
    }
}
