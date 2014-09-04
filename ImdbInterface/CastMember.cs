using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbInterface {
    public struct CastMember {
        private readonly Person person;
        private readonly string role;

        public CastMember(Person person, string role) {
            this.person = person;
            this.role = role;
        }

        public Person Person {
            get {
                return person;
            }
        }

        public string Role {
            get {
                return role;
            }
        }

        public override string ToString() {
            return string.Format("{0} as {1}", Person, Role);
        }
    }
}
