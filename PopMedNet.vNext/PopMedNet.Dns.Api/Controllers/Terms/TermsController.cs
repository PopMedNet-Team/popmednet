using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopMedNet.Dns.Data;
using PopMedNet.Dns.DTO.Security;
using PopMedNet.Dns.DTO;

namespace PopMedNet.Dns.Api.Terms
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = PopMedNet.Utilities.WebSites.Security.AuthSchemeConstants.Scheme)]
    public class TermsController : ApiDataControllerBase<Data.Term, TermDTO, DataContext, PermissionDefinition>
    {
        public TermsController(IConfiguration config, DataContext db, IMapper mapper)
            : base(db, mapper, config)
        {
        }
    }
}
