using System;
using System.Linq;
using ch.thommenmedia.common.Extensions;
using ch.thommenmedia.common.Interfaces;

namespace ch.thommenmedia.common.Model
{
    public class SimpleSelectItem : ISelectItem
    {
        private string _name { get; set; }
        public Guid Id { get; set; }

        public string Name
        {
            get => string.IsNullOrEmpty(_name) ? GetName() : _name;
            set => _name = value;
        }

        private string GetName()
        {
            if (Data == null)
                return string.Empty;

            if (Data.ImplementsInterface(typeof(IEntityBase)))
                return (Data as IEntityBase).ToStringOrEmpty();

            // no name/text attribute found
            return string.Empty;
        }
        public Guid? ObjectTypeId { get; set; }
        public object Data { get; set; }
        public bool IsPrimary { get; set; }
    }
}
