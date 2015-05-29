namespace Yue.Bookings.Contract.Commands
open System
open ACE
open Yue.Bookings.Contract

type BookingCommandBase = inherit ICommand
type CancelSubscriotion = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingCommandBase
type ChangeTime = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface BookingCommandBase
type ConfirmSubscription = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingCommandBase
type LeaveAMessage = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingCommandBase
type SubscribeResource = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface BookingCommandBase