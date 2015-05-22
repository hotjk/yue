namespace Yue.Bookings.Contract.Actions
open System
open ACE
open Yue.Bookings.Contract

type BookingActionBase = inherit IAction
type CancelSubscriotion = { ActionId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type ChangeTime = { ActionId: int; ResourceId: int; BookingId: int; Message: string; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type ConfirmSubscription = { ActionId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type LeaveAMessage = { ActionId: int; ResourceId: int; BookingId: int; Message: string; CreateAt: DateTime; CreateBy: int } interface BookingActionBase
type SubscribeResource = { ActionId: int; ResourceId: int; BookingId: int; Message: string; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface BookingActionBase