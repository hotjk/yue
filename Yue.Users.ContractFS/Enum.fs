namespace Yue.Users.ContractFS
open System
open ACE

type UserCommand = 
    | Create = 0 
    | ChangeProfile = 10

type UserRole =
    | Normal = 0
    | Admin = 10

type UserSecurityCommand =
    | CreateUserSecurity = 0
    | RequestActivateToken = 1
    | ActivateUser = 2
    | VerifyPassword = 3
    | RequestResetPasswordToken = 10
    | VerifyResetPasswordToken = 11
    | CancelResetPasswordToken = 12
    | ResetPassword = 13
    | ChangePassword = 14
    | Block = 20
    | Destory = 21
    | Restore = 22

type UserState =
    | Initial = 0
    | Inactive = 1
    | Normal = 2
    | Blocked = 10
    | Destroyed = 11
