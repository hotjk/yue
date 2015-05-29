namespace Yue.Users.Contract.Commands
open System
open ACE

type UserCommandBase = inherit ICommand
type ActivateUser = { UserId: int; CreateAt: DateTime; CreateBy: int; } interface UserCommandBase
type CreateUser = { UserId: int; CreateAt: DateTime; CreateBy: int; Email:string; Name:string } interface UserCommandBase

type UserSecurityCommandBase = inherit ICommand
type CancelResetPasswordToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserSecurityCommandBase
type RequestActivateToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserSecurityCommandBase
type RequestResetPasswordToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserSecurityCommandBase
type ResetActivateToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserSecurityCommandBase
type VerifyResetPasswordToken = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string } interface UserSecurityCommandBase
type ChangePassword = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash:string } interface UserSecurityCommandBase
type CreateUserSecurity = { UserId: int; CreateAt: DateTime; CreateBy: int; PasswordHash:string } interface UserSecurityCommandBase
type ResetPassword = { UserId: int; CreateAt: DateTime; CreateBy: int; Token:string; PasswordHash:string } interface UserSecurityCommandBase