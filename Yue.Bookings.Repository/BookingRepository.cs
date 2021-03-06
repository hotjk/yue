﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Dapper;
using Yue.Bookings.Contract;
using Yue.Common.Repository;
using Yue.Bookings.Repository.Model;

namespace Yue.Bookings.Repository
{
    public class BookingRepository : BaseRepository, IBookingRepository
    {
        public BookingRepository(SqlOption option) : base(option) { }

        private static readonly string[] _bookingColumns = new string[] {
"BookingId", "ResourceId", "State", "From", "To", "Minutes", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _activityColumns = new string[] {
"ActivityId", "ResourceId", "BookingId", "CreateBy", "CreateAt", "Type", "From", "To", "Minutes", "Message" };

        public Booking Get(int bookingId, bool withActivities)
        {
            using (IDbConnection connection = OpenConnection())
            {
                BookingPM booking = connection.Query<BookingPM>(
                    string.Format(
@"SELECT {0} FROM `bookings` 
WHERE `BookingId` = @BookingId;", SqlHelper.Columns(_bookingColumns)),
                    new { BookingId = bookingId }).SingleOrDefault();

                if (booking == null) return null;

                if (withActivities)
                {
                    var activities = connection.Query<BookingActivityPM>(
                        string.Format(
@"SELECT {0} FROM `booking_activities` 
WHERE `BookingId` = @BookingId ORDER BY ActivityId DESC;", SqlHelper.Columns(_activityColumns)),
                        new { BookingId = bookingId });

                    return BookingPM.FromPM(booking, activities);
                }
                return BookingPM.FromPM(booking);
            }
        }

        public IEnumerable<Booking> BookingsByResource(Contract.TimeSlot timeSlot, int resourceId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var bookings = connection.Query<BookingPM>(
                    string.Format(
@"SELECT {0} FROM `bookings` 
WHERE `ResourceId` = @ResourceId
AND `From` <= @To AND `To` >= @From;", SqlHelper.Columns(_bookingColumns)),
                    new { ResourceId = resourceId, From = timeSlot.From, To = timeSlot.To });
                return bookings.Select(n => BookingPM.FromPM(n));
            }
        }

        public IEnumerable<Booking> BookingsByUser(Contract.TimeSlot timeSlot, int userId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var bookings = connection.Query<BookingPM>(
                     string.Format(
@"SELECT {0} FROM `bookings` 
WHERE `CreateBy` = @CreateBy 
AND `From` <= @To AND `To` >= @From;", SqlHelper.Columns(_bookingColumns)),
                    new { CreateBy = userId, From = timeSlot.From, To = timeSlot.To });

                return bookings.Select(n => BookingPM.FromPM(n));
            }
        }
    }
}
