namespace Yue.Users.ContractFS
open System
open ACE

type UserActionBase = inherit IAction
type Activate = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserActionBase
type CancelResetPasswordToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserActionBase
type ChangePassword = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash:string } interface UserActionBase
type PasswordUserActionBase = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash:string } interface UserActionBase
type Register = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash:string; Email: string; Name: string } interface UserActionBase
type ResetPassword = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash: string; Token: string } interface UserActionBase
type VerifyResetPasswordToken = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash:string } interface UserActionBase
type RequestActivateToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserActionBase
type RequestResetPasswordToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserActionBase

