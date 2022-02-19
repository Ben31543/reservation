﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Data.Entities
{
    public class Member
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public byte Age { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public long? BankCardId { get; set; }

        public BankCard BankCard { get; set; }
    }
}
