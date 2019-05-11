using ch.thommenmedia.common.Extensions;
using ch.thommenmedia.common.Helper;
using ch.thommenmedia.common.Interfaces;

namespace ch.thommenmedia.common.Utils
{
    /// <summary>
    ///     Represents a translatable string
    /// </summary>
    public struct TranslatableString
    {
        #region Static

        /// <summary>
        ///     A read-only instance of the ShortGuid class whose value
        ///     is guaranteed to be all zeroes.
        /// </summary>
        public static readonly TranslatableString Empty = new TranslatableString(string.Empty);

        #endregion

        #region Fields

        public string Content { get; set; }
        public object[] Replacements { get; set; }
        public bool MustTranslate { get; set; }

        #endregion

        #region Contructors

        /// <summary>
        ///     use this if you just have a simple string
        /// </summary>
        /// <param name="value">
        ///     The encoded guid as a
        ///     base64 string
        /// </param>
        public TranslatableString(string value)
        {
            Content = value;
            MustTranslate = true;
            Replacements = new object[0];
        }

        /// <summary>
        ///     Creates a ShortGuid from a Guid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="replacements"></param>
        public TranslatableString(string value, object[] replacements)
        {
            Content = value;
            MustTranslate = true;
            Replacements = replacements;
        }

        /// <summary>
        ///     Creates a ShortGuid from a Guid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="replacements"></param>
        /// <param name="translate">should the content be translated</param>
        public TranslatableString(string value, object[] replacements, bool translate)
        {
            Content = value;
            MustTranslate = translate;
            Replacements = replacements;
        }

        public TranslatableString(string value, bool translate)
        {
            Content = value;
            MustTranslate = translate;
            Replacements = new object[0];
        }

        #endregion

        #region Properties

        ///// <summary>
        /////     Gets/sets the underlying Guid
        ///// </summary>
        //public Guid Guid
        //{
        //    get => _guid;
        //    set
        //    {
        //        if (value != _guid)
        //        {
        //            _guid = value;
        //            _value = Encode(value);
        //        }
        //    }
        //}

        ///// <summary>
        /////     Gets/sets the underlying base64 encoded string
        ///// </summary>
        //public string Value
        //{
        //    get => _value;
        //    set
        //    {
        //        if (value != _value)
        //        {
        //            _value = value;
        //            _guid = Decode(value);
        //        }
        //    }
        //}

        #endregion

        #region ToString

        /// <summary>
        ///     Returns the base64 encoded guid as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Replacements.Length > 0 ? Content.Apply(Replacements): Content;
        }

        /// <summary>
        /// gets the translated string
        /// </summary>
        /// <param name="translator"></param>
        /// <returns></returns>
        public string ToString(ITranslator translator)
        {
            if (string.IsNullOrEmpty(Content))
                return string.Empty;
            var translated = translator.TranslateText(Content);
            return Replacements.Length > 0 ? translated.Apply(Replacements) : translated;
        }

        #endregion

        #region Equals

        /// <summary>
        ///     Returns a value indicating whether this instance and a
        ///     specified Object represent the same type and value.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is TranslatableString ts)
                return Content == ts.Content && JsonHelper.Serialize(Replacements) == JsonHelper.Serialize(ts.Replacements) && MustTranslate == ts.MustTranslate;
            if (obj is string s)
                return s == ToString();
            return false;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        ///     Returns the HashCode for underlying Guid.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return string.IsNullOrEmpty(ToString()) ? 0 : ToString().GetHashCode();
        }

        #endregion
        
        #region Operators

        /// <summary>
        ///     Determines if both ShortGuids have the same underlying
        ///     Guid value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(TranslatableString x, TranslatableString y)
        {
            return x.Equals(y);
        }

        /// <summary>
        ///     Determines if both ShortGuids do not have the
        ///     same underlying Guid value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(TranslatableString x, TranslatableString y)
        {
            return !(x == y);
        }

        /// <summary>
        ///     Implicitly converts the ShortGuid to it's string equivilent
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static implicit operator string(TranslatableString ts)
        {
            return ts.ToString();
        }

    }

    #endregion
}