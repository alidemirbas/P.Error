using System;
using System.Collections.Generic;
using System.Linq;

namespace P.ErrorManagement
{
    public class ProtocolStatus : IEquatable<ProtocolStatus>
    {
        ProtocolStatus()
        {
            _hashSet = new HashSet<ProtocolStatus>();

            Add(new ProtocolStatus { Name = "Success", Code = 200, Description = "OK" });
            Add(new ProtocolStatus { Name = "InternalError", Code = 500, Description = "Internal Server Error" });
            Add(new ProtocolStatus { Name = "BadRequest", Code = 400, Description = "Bad Request Error" });
            Add(new ProtocolStatus { Name = "Unauthorized", Code = 401, Description = "Unauthorized Error" });
            Add(new ProtocolStatus { Name = "Forbidden", Code = 403, Description = "Forbidden Error" });
            Add(new ProtocolStatus { Name = "NotFound", Code = 404, Description = "Not Found Error" });
            Add(new ProtocolStatus { Name = "SwitchingProtocols", Code = 101, Description = "Switching Protocols" });
            Add(new ProtocolStatus { Name = "HalfOpen", Code = 600, Description = "Dropped Connection" });
        }

        private static ProtocolStatus _instance;
        public static ProtocolStatus Instance{ get { return _instance ?? (_instance = new ProtocolStatus()); } }

        public ProtocolStatus this[int code]
        {
            get
            {
                foreach (var p in _hashSet)
                {
                    if (p.Code == code)
                        return p;
                }

                throw new System.Exception("");//todo
            }
        }

        public void Add(ProtocolStatus protocolStatus)
        {
            if (!_hashSet.Add(protocolStatus))
                throw new System.Exception();//bunu da yonetmeye gerek yok
        }

        public string Name { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }

        private HashSet<ProtocolStatus> _hashSet;

        public bool Equals(ProtocolStatus other)
        {
            if (other == null)
                return false;

            return this.Code == other.Code && this.Description == other.Description;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProtocolStatus);
        }

        public static bool operator ==(ProtocolStatus p1, ProtocolStatus p2)
        {
            if (object.ReferenceEquals(p1, null) || object.ReferenceEquals(p2, null))
                return false;

            return p1.Equals(p2);
        }

        public static bool operator !=(ProtocolStatus p1, ProtocolStatus p2)
        {
            return !(p1 == p2);
        }
    }
}
