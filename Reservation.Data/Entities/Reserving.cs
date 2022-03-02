﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Data.Entities
{
    public class Reserving
    {
        [Key]
        public long Id { get; set; }

        public DateTime ReservationDate { get; set; }

        public long MemberId { get; set; }

        public long ServiceMemberBranchId { get; set; }

        public long ServiceMemberId { get; set; }

        public long? BankCardId { get; set; }

        public long? BankAccountId { get; set; }

        public decimal Amount { get; set; }

        public bool IsOnlinePayment { get; set; }

        public string Tables { get; set; }

        public string Dishes { get; set; }

        public bool IsTakeOut { get; set; }

        public Member Member { get; set; }

        public ServiceMember ServiceMember { get; set; }

        public ServiceMemberBranch ServiceMemberBranch { get; set; }

        public BankCard BankCard { get; set; }

        public BankAccount BankAccount { get; set; }
    }
}