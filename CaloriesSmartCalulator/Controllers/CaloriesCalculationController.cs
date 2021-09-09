using AutoMapper;
using CaloriesSmartCalulator.Dtos;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using CaloriesSmartCalulator.Handlers.Contracts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<string> CreateCalculationTask([FromBody] string[] products)
        {
            var result = await _mediator.Send(new CreateCaloriesCalculationCommand(products));

            if (result.Success)
            {
                return result.Value.ToString();
            }

            return result.FailureMessage;
        }
    }
}
