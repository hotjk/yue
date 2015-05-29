namespace Yue.Bookings.Contract.Actions
open System
open ACE
open Yue.Bookings.Contract

type BookingActionBase = inherit IAction
type CancelSubscriotion = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type ChangeTime = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type ConfirmSubscription = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type LeaveAMessage = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type SubscribeResource = { ActivityId: int; ResourceId: int; BookingId: int; Message: string; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface BookingActionBase