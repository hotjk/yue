namespace Yue.Members.Contract
open System
open ACE
open Yue.Members.Contract

type MemberCommandBase = inherit ICommand
type AddMember = { MemberId: int; UserId: int; GroupId: int; MemberGroup: MemberGroup; CreateAt: DateTime; CreateBy: int; } interface MemberCommandBase
type RemoveMember = { MemberId: int; UserId: int; GroupId: int; MemberGroup: MemberGroup; CreateAt: DateTime; CreateBy: int; } interface MemberCommandBase
