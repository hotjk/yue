﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Resources.Contract;

namespace Yue.Resources.Model
{
    public class Member
    {
        public int MemberId { get; private set; }
        public int UserId { get; private set; }
        public int GroupId { get; private set; }
        public MemberGroup MemberGroup { get; private set; }

        public int CreateBy { get; private set; }
        public DateTime CreateAt { get; private set; }
        public int UpdateBy { get; private set; }
        public DateTime UpdateAt { get; private set; }

        public static Member Create(AddMember action)
        {
            Member member = new Member();

            member.MemberId = action.MemberId;
            member.UserId = action.UserId;
            member.GroupId = action.GroupId;
            member.MemberGroup = action.MemberGroup;
            member.CreateBy = action.CreateBy;
            member.CreateAt = action.CreateAt;
            member.UpdateBy = action.CreateBy;
            member.UpdateAt = action.CreateAt;
            return member;
        }
    }
}
