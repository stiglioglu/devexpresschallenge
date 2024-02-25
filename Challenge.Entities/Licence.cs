using DevExpress.Xpo;
using System;

namespace Challenge.Entities
{
    [OptimisticLocking(false)]
    public class Licence : XPBaseObject
    {
        public Licence() { }
        public Licence(Session session) : base(session) { }

        [Key(true)]
        public int Id { get; set; }
        public string Key { get; set; }
        public DateTime Create { get; set; }
        public DateTime Expiry { get; set; }
    }

}