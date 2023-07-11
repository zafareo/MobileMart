using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileMarketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ApiControllerBase<T> : ControllerBase
    {
        protected IMapper _mapper => HttpContext.RequestServices.GetRequiredService<IMapper>();
        protected IValidator<T> _validator => HttpContext.RequestServices.GetRequiredService<IValidator<T>>();

    }
}
