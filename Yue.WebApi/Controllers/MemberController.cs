using ACE;
using Grit.Sequence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Bookings.View.Model;
using Yue.Common.Contract;
using Yue.Members.Model;

namespace Yue.WebApi.Controllers
{
    [RoutePrefix("api/members")]
    public class MemberController : ApiAuthorizeController
    {
        private IActionBus _actionBus;
        private ISequenceService _sequenceService;
        private IMemberService _memberService;

        public MemberController(
            IAuthenticator authenticator,
            IActionBus actionBus,
            IEventBus eventBus,
            IMemberService memberService,
            ISequenceService sequenceService) 
            : base(authenticator, actionBus)
        {
            _actionBus = actionBus;
            _sequenceService = sequenceService;
            _memberService = memberService;
        }

        [HttpGet]
        [Route("{id}")]
        [ApiAuthorize]
        public IHttpActionResult Get(int id)
        {
            Member member = _memberService.Get(id);
            return Ok(member);
        }

    }
}
