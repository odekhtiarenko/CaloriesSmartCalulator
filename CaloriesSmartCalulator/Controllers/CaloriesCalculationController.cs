using AutoMapper;
using CaloriesSmartCalulator.Handlers.Contracts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("post")]
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
