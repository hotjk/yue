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
        private static readonly string[] _actionColumns = new string[] {
"ActionId", "ResourceId", "BookingId", "CreateBy", "CreateAt", "Type", "From", "To", "Minutes", "Message" };

        public Booking Get(int bookingId, bool withActions)
        {
            using (IDbConnection connection = OpenConnection())
            {
                BookingPM booking = connection.Query<BookingPM>(
                    string.Format(
@"SELECT {0} FROM `bookings` 
WHERE `BookingId` = @BookingId;", SqlHelper.Columns(_bookingColumns)),
                    new { BookingId = bookingId }).SingleOrDefault();

                if (booking == null) return null;

                if (withActions)
                {
                    var actions = connection.Query<BookingActionPM>(
                        string.Format(
@"SELECT {0} FROM `booking_actions` 
WHERE `BookingId` = @BookingId ORDER BY ActionId DESC;", SqlHelper.Columns(_actionColumns)),
                        new { BookingId = bookingId });

                    return BookingPM.FromPM(booking, actions);
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
AND From <= @To AND To >=From;", SqlHelper.Columns(_bookingColumns)),
                    new { ResourceId = resourceId });
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
AND From <= @To AND To >=From;", SqlHelper.Columns(_bookingColumns)),
                    new { CreateBy = userId, From = timeSlot.From, To = timeSlot.To });

                return bookings.Select(n => BookingPM.FromPM(n));
            }
        }
    }
}
