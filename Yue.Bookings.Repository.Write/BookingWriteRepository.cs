﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yue.Bookings.Contract;
using Yue.Bookings.Contract.Actions;
using Yue.Bookings.Model;
using Yue.Bookings.Model.Write;
using Yue.Common.Repository;
using Dapper;

namespace Yue.Bookings.Repository.Write
{
    public class BookingWriteRepository : BaseRepository, IBookingWriteRepository
    {
        public BookingWriteRepository(SqlOption option) : base(option) { }

        private const string[] _bookingColumns = new string[] {
"BookingId", "ResourceId", "State", "From", "To", "Minutes", "CreateBy", "UpdateBy", "CreateAt", "UpdateAt" };
        private const string[] _actionColumns = new string[] {
"ActionId", "ResourceId", "BookingId", "CreateBy", "CreateAt", "Type", "From", "To", "Minutes", "Message" };

        public Model.Booking GetForUpdate(int bookingId)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return connection.Query<Booking, TimeSlot, Booking>(
                     string.Format(@"SELECT {0} FROM `bookings` WHERE `BookingId` = @BookingId FOR UPDATE;", 
                     SqlHelper.Columns(_bookingColumns)),
                     (b, t) => { b.ChangeTime(t); return b; },
                     new { BookingId = bookingId }).SingleOrDefault();
            }
        }

        public bool Add(Model.Booking booking)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `bookings` ({0}) VALUES ({1});", 
                    SqlHelper.Columns(_bookingColumns), 
                    SqlHelper.Params(_bookingColumns)), 
                    booking);
            }
        }

        public bool Update(Model.Booking booking)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"UPDATE `bookings` SET {0} WHERE `BookingId` = @BookingId;",
                    SqlHelper.Sets(_bookingColumns)),
                    booking);
            }
        }

        public bool AddAction(BookingActionBase action)
        {
            using (IDbConnection connection = OpenConnection())
            {
                return 1 == connection.Execute(
                    string.Format(@"INSERT INTO `booking_actions` ({0}) VALUES ({1});",
                    SqlHelper.Columns(_actionColumns),
                    SqlHelper.Params(_actionColumns)),
                    action);
            }
        }
    }
}