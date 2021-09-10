using AutoMapper;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.Dtos.Requests;
using CaloriesSmartCalulator.Dtos.Responses;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.Handlers.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CaloriesSmartCalulator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaloriesCalculationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CaloriesCalculationController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskResult(Guid taskId)
        {
            var result = await _mediator.Send(new GetCaloriesCalculationTaskResultQuery(taskId));

            return Ok(_mapper.Map<CalculationTaskResult>(result));
        }

        [HttpGet("status/{taskId}")]
        public async Task<IActionResult> GetCalculationTaskStatus(Guid taskId)
        {
            var result = await _mediator.Send(new GetCaloriesCalculationTaskStatusQuery(taskId));

            return Ok(_mapper.Map<StatusObject>(result));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCalculationTask([FromBody] CalculateMealCaloriesRequest products)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(_mapper.Map<CreateCaloriesCalculationCommand>(products));

                if (result.Success)
                {
                    return Ok(_mapper.Map<CalculateMealCaloriesResponse>(result.Value));
                }

                return BadRequest(result.FailureMessage);
            }

            return BadRequest(ModelState.Values.SelectMany(x => x.Errors));
        }
    }
}
