﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Model;
using Yue.Common.Repository;
using Dapper;
using Yue.Bookings.Handler;
using Yue.Bookings.Repository.Model;
using Yue.Bookings.Contract.Commands;

namespace Yue.Bookings.Repository.Write
{
    public class BookingWriteRepository : BaseRepository, IBookingWriteRepository
    {
        public BookingWriteRepository(SqlOption option) : base(option) { }

        private static readonly string[] _bookingColumns = new string[] {
"BookingId", "ResourceId", "State", "From", "To", "Minutes", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private static readonly string[] _bookingUpdateColumns = new string[] {
            "State", "From", "To", "Minutes", "UpdateBy", "UpdateAt" };

        private static readonly string[] _activityColumns = new string[] {
"ActivityId", "ResourceId", "BookingId", "CreateBy", "CreateAt", "Type", "From", "To", "Minutes", "Message" };

        public Booking GetForUpdate(int bookingId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                var booking = connection.Query<BookingPM>(
                     string.Format(@"SELECT {0} FROM `bookings` WHERE `BookingId` = @BookingId FOR UPDATE;", 
                     SqlHelper.Columns(_bookingColumns)),
                     new { BookingId = bookingId }).SingleOrDefault();
                if (booking == null) return null;
                return BookingPM.FromPM(booking);
            }
        }

        public bool Add(Booking booking)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `bookings` ({0}) VALUES ({1});", 
                    SqlHelper.Columns(_bookingColumns), 
                    SqlHelper.Params(_bookingColumns)),
                    BookingPM.ToPM(booking));
            }
        }

        public bool Update(Booking booking)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"UPDATE `bookings` SET {0} WHERE `BookingId` = @BookingId;",
                    SqlHelper.Sets(_bookingUpdateColumns)),
                    BookingPM.ToPM(booking));
            }
        }

        public bool AddAction(BookingCommandBase activity)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `booking_activities` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_activityColumns),
                    SqlHelper.Params(_activityColumns)),
                    BookingActivityPM.ToPM(activity));
            }
        }
    }
}
