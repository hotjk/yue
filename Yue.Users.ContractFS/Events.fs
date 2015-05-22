namespace Yue.Users.ContractFS
open System
open ACE

type UserActiviteTokenGenerated = { UserId: int; CreateAt: DateTime; CreateaBy: int; Token: string; } interface IEvent
type UserPasswordChanged = { UserId: int; CreateAt: DateTime; CreateaBy: int }
type UserPasswordVerified = { UserId: int; CreateAt: DateTime; CreateaBy: int; Match: bool }
type UserResetPasswordTokenGenerated = { UserId: int; CreateAt: DateTime; CreateaBy: int; Token: string }
