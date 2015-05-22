namespace Yue.Users.Contract.Events
open System
open ACE

type UserActiviteTokenGenerated = { UserId: int; CreateAt: DateTime; CreateaBy: int; Token: string; } interface IEvent
type UserPasswordChanged = { UserId: int; CreateAt: DateTime; CreateaBy: int } interface IEvent
type UserPasswordVerified = { UserId: int; CreateAt: DateTime; CreateaBy: int; Match: bool } interface IEvent
type UserResetPasswordTokenGenerated = { UserId: int; CreateAt: DateTime; CreateaBy: int; Token: string } interface IEvent
