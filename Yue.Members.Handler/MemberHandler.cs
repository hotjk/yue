using ACE;
using ACE.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Common.Contract;
using Yue.Members.Contract;
using Yue.Members.Model;

namespace Yue.Members.Handler
{
    public class MemberHandler :
        ICommandHandler<AddMember>,
        ICommandHandler<RemoveMember>
    {
        private IEventBus _eventBus;
        private IMemberWriteRepository _repository;

        public MemberHandler(IEventBus eventBus, IMemberWriteRepository repository)
        {
            _eventBus = eventBus;
            _repository = repository;
        }

        public void Execute(AddMember command)
        {
            Member member = _repository.GetForUpdate(command.MemberId);
            if (member != null)
            {
                throw new BusinessException(BusinessStatusCode.Conflict, "Member already existed.");
            }
            member = Member.Create(command);
            _repository.Add(member);
        }

        public void Execute(RemoveMember command)
        {
            Member member = _repository.GetForUpdate(command.MemberId);
            if(member == null)
            {
                throw new BusinessException(BusinessStatusCode.NotFound, "Member not found.");
            }
            _repository.Remove(member);
        }
    }
}
