namespace Yue.Resources.Contract
open System
open ACE
open Yue.Resources.Contract

type MemberCommandBase = inherit ICommand
type AddMember = { MemberId: int; UserId: int; GroupId: int; MemberGroup: MemberGroup; CreateAt: DateTime; CreateBy: int; } interface MemberCommandBase
type RemoveMember = { MemberId: int; UserId: int; GroupId: int; MemberGroup: MemberGroup; CreateAt: DateTime; CreateBy: int; } interface MemberCommandBase
