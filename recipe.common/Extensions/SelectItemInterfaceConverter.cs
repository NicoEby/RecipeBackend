using System;
using ch.thommenmedia.common.Interfaces;
using ch.thommenmedia.common.Model;
using Newtonsoft.Json.Converters;

namespace ch.thommenmedia.common.Extensions
{

    /// <summary>
    /// idea found here: https://stackoverflow.com/questions/5780888/casting-interfaces-for-deserialization-in-json-net
    /// </summary>
    public class SelectItemInterfaceConverter : CustomCreationConverter<ISelectItem>
    {
        public override ISelectItem Create(Type objectType)
        {
            return new SimpleSelectItem();
        }
    }
}
