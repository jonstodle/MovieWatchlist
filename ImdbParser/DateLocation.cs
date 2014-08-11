﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbParser {
    public struct DateLocation {
        private readonly DateTime date;
        private readonly string location;

        public DateLocation(DateTime date, string location) {
            this.date = date;
            this.location = location;
        }

        public DateTime Date {
            get {
                return date.Date;
            }
        }

        public string Location {
            get {
                return location;
            }
        }

        public override string ToString() {
            return Date.ToString() + " (" + Location + ")";
        }

        public static bool operator ==(DateLocation dl1, DateLocation dl2) {
            return dl1.Location == dl2.Location && dl1.Date == dl1.Date;
        }

        public static bool operator !=(DateLocation dl1, DateLocation dl2) {
            return dl1.Location != dl2.Location || dl1.Date != dl2.Date;
        }
    }
}
