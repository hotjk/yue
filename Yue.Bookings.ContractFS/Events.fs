namespace Yue.Bookings.Contract.Events
open System
open ACE
open Yue.Bookings.Contract

type BookingIsCreated = { ResourceId: int; BookingId: int; State: BookingState; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface IEvent
type BookingStateChanged = { OrignalState: BookingState; State: BookingState; CreateAt: DateTime; CreateBy: int } interface IEvent
type BookingTimeChanged = { OrignalTimeSlot: TimeSlot; TimeSlot: TimeSlot; CreateAt: DateTime; CreateBy: int } interface IEvent
