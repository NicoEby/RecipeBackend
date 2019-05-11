using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ch.thommenmedia.common.Interfaces
{
    /// <summary>
    /// interface to identify the database context and set default methods and properties
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// the default settings accessor point
        /// </summary>
        IQueryable<IApplicationSetting> ApplicationSettings { get; set; }
    }
}
