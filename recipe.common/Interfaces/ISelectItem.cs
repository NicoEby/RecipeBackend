using System;

namespace ch.thommenmedia.common.Interfaces
{
    /// <summary>
    /// represents one filtered/selected item/object
    /// </summary>
    public interface ISelectItem
    {
        Guid Id { get; set; }
        string Name { get; set; }

        /// <summary>
        /// determines the underlaying type; could be set if you expect different types of objects
        /// </summary>
        Guid? ObjectTypeId { get; set; }

        /// <summary>
        /// the Data could contain additional Information About the source object eg: PersonDTO
        /// Data is unstructured and can therefor be used to transfer any desired data
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// use this to mark a selectitem in a list as primary
        /// </summary>
        bool IsPrimary { get; set; }

    }

}
