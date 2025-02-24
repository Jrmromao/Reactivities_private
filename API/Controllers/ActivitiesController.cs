using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Appication.Activities;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase {

        private readonly IMediator _mediator;

        public ActivitiesController (IMediator mediator) {

            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> List () { //CancellationToken ct
            return await _mediator.Send (new List.Query ());
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Activity>> Details (Guid id) {

            var activity = await _mediator.Send (new Details.Query { Id = id });

            return activity;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Create ( Create.Command command) {
            return await _mediator.Send (command);
        }

        [HttpPut ("{id}")]
        public async Task<ActionResult<Unit>> Edit (Guid id, Edit.Command command) {
            command.Id = id;
            return await _mediator.Send (command);
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<Unit>> Delete (Guid id) {
            return await _mediator.Send (new Delete.Command { Id = id });
        }

    }
}