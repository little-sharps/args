namespace Args
{
    /// <summary>
    /// A simple interface to impelment for type conversion from string. If a TypeConverter for the type does not already exist, this is the easiest way to do type conversion in Args
    /// </summary>
    public interface IArgsTypeConverter
    {
        /// <summary>
        /// When implemented, this method will convert the provided value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object Convert(string value);
    }
}
