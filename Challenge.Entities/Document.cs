using DevExpress.Xpo;
using System;

namespace Challenge.Entities
{
    [OptimisticLocking(false)]
    public class Document : XPBaseObject
    {
        public Document() : base(){}

        public Document(Session session) : base(session){}

        [Key(true)]
        public int Id { get; set; }
        public string name { get; set; }
        public string path { get; set; }

    }

}