using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Common.Infrastructure;
using Dapper;
using Yue.Bookings.Contract;

namespace Yue.Bookings.Repository
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(SqlOption option) : base(option) { }

        public Booking Get(int bookingId, bool withActions)
        {
            using (IDbConnection connection = OpenConnection())
            {
                Booking booking = connection.Query<Booking, TimeSlot, Booking>(
@"
SELECT `BookingId`,
    `ResourceId`,
    `State`,
    `From`,
    `To`,
    `Minutes`,
    `CreateBy`,
    `UpdateBy`,
    `CreateAt`,
    `UpdateAt`
FROM `bookings`
WHERE `BookingId` = @BookingId;
",
                    (b, t) => { b.ChangeTime(t); return b; },
                    new { BookingId = bookingId }).SingleOrDefault();

                if (booking == null) return null;

                if (withActions)
                {
                    booking.InitalActions(connection.Query<BookingActionBase, TimeSlot, BookingActionBase>(
@"
SELECT `ActionId`,
    `ResourceId`,
    `BookingId`,
    `CreateBy`,
    `CreateAt`,
    `Type`,
    `From`,
    `To`,
    `Minutes`,
    `Message`
FROM `booking_actions` 
WHERE `BookingId` = @BookingId;
",
                        (a, t) => { a.TimeSlot = t; return a; },
                        new { BookingId = bookingId }));
                }
                return booking;
            }
        }

        public IEnumerable<Booking> BookingsByResource(Contract.TimeSlot timeSlot, int resourceId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Booking, TimeSlot, Booking>(
@"
SELECT `BookingId`,
    `ResourceId`,
    `State`,
    `From`,
    `To`,
    `Minutes`,
    `CreateBy`,
    `UpdateBy`,
    `CreateAt`,
    `UpdateAt`
FROM `bookings`
WHERE `ResourceId` = @ResourceId
AND From <= @To AND To >=From;
",
                    (b, t) => { b.ChangeTime(t); return b; },
                    new { ResourceId = resourceId });
            }
        }

        public IEnumerable<Booking> BookingsByUser(Contract.TimeSlot timeSlot, int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Booking, TimeSlot, Booking>(
@"
SELECT `BookingId`,
    `ResourceId`,
    `State`,
    `From`,
    `To`,
    `Minutes`,
    `CreateBy`,
    `UpdateBy`,
    `CreateAt`,
    `UpdateAt`
FROM `bookings`
WHERE `CreateBy` = @CreateBy;
AND From <= @To AND To >=From;
",
                    (b, t) => { b.ChangeTime(t); return b; },
                    new { CreateBy = userId, From = timeSlot.From, To = timeSlot.To });
            }
        }
    }
}
