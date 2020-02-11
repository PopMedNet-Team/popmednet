using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Utilities
{
    /// <summary>
    /// Class representing a Base36 number
    /// </summary>
    public struct Base36
    {
        #region Constants (and pseudo-constants)

        /// <summary>
        /// Base36 containing the maximum supported value for this type
        /// </summary>
        public static readonly Base36 MaxValue = new Base36(long.MaxValue);
        /// <summary>
        /// Base36 containing the minimum supported value for this type
        /// </summary>
        public static readonly Base36 MinValue = new Base36(long.MinValue + 1);

        #endregion

        #region Fields

        private long numericValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiate a Base36 number from a long value
        /// </summary>
        /// <param name="NumericValue">The long value to give to the Base36 number</param>
        public Base36(long numericValue)
        {
            this.numericValue = 0; //required by the struct.
            this.numericValue = numericValue;
        }


        /// <summary>
        /// Instantiate a Base36 number from a Base36 string
        /// </summary>
        /// <param name="Value">The value to give to the Base36 number</param>
        public Base36(string value)
        {
            numericValue = 0; //required by the struct.
            this.Value = value;
        }


        #endregion

        #region Properties

        /// <summary>
        /// Get or set the value of the type using a base-10 long integer
        /// </summary>
        public long NumericValue
        {
            get
            {
                return numericValue;
            }
            set
            {
                //Make sure value is between allowed ranges
                if (value <= long.MinValue || value > long.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }

                numericValue = value;
            }
        }


        /// <summary>
        /// Get or set the value of the type using a Base36 string
        /// </summary>
        public string Value
        {
            get
            {
                return Base36.NumberToBase36(numericValue);
            }
            set
            {
                try
                {
                    numericValue = Base36.Base36ToNumber(value);
                }
                catch
                {
                    //Catch potential errors
                    throw new ArgumentOutOfRangeException(value);
                }
            }
        }


        #endregion

        #region Public Static Methods

        /// <summary>
        /// Static method to convert a Base36 string to a long integer (base-10)
        /// </summary>
        /// <param name="base36Value">The number to convert from</param>
        /// <returns>The long integer</returns>
        public static long Base36ToNumber(string base36Value)
        {
            //Make sure we have passed something
            if (string.IsNullOrEmpty(base36Value))
                throw new ArgumentOutOfRangeException(base36Value);

            //Make sure the number is in upper case:
            base36Value = base36Value.ToUpper();

            //Account for negative values:
            bool isNegative = false;

            if (base36Value[0] == '-')
            {
                base36Value = base36Value.Substring(1);
                isNegative = true;
            }

            //Loop through our string and calculate its value
            try
            {
                //Keep a running total of the value
                long returnValue = Base36DigitToNumber(base36Value[base36Value.Length - 1]);

                //Loop through the character in the string (right to left) and add
                //up increasing powers as we go.
                for (int i = 1; i < base36Value.Length; i++)
                {
                    returnValue += ((long)Math.Pow(36, i) * Base36DigitToNumber(base36Value[base36Value.Length - (i + 1)]));
                }

                //Do negative correction if required:
                if (isNegative)
                {
                    return returnValue * -1;
                }
                else
                {
                    return returnValue;
                }
            }
            catch
            {
                //If something goes wrong, this is not a valid number
                throw new ArgumentOutOfRangeException(base36Value);
            }
        }


        /// <summary>
        /// Public static method to convert a long integer (base-10) to a Base36 number
        /// </summary>
        /// <param name="numericValue">The base-10 long integer</param>
        /// <returns>A Base36 representation</returns>
        public static string NumberToBase36(long numericValue)
        {
            try
            {
                //Handle negative values:
                if (numericValue < 0)
                {
                    return string.Concat("-", PositiveNumberToBase36(Math.Abs(numericValue)));
                }
                else
                {
                    return PositiveNumberToBase36(numericValue);
                }
            }
            catch
            {
                throw new ArgumentOutOfRangeException(numericValue.ToString());
            }
        }


        #endregion

        #region Private Static Methods

        private static string PositiveNumberToBase36(long NumericValue)
        {
            //This is a clever recursively called function that builds
            //the base-36 string representation of the long base-10 value
            if (NumericValue < 36)
            {
                //The get out clause; fires when we reach a number less than 
                //36 - this means we can add the last digit.
                return NumberToBase36Digit((byte)NumericValue).ToString();
            }
            else
            {
                //Add digits from left to right in powers of 36 
                //(recursive)
                return string.Concat(PositiveNumberToBase36(NumericValue / 36), NumberToBase36Digit((byte)(NumericValue % 36)).ToString());
            }
        }


        private static byte Base36DigitToNumber(char Base36Digit)
        {
            //Converts one base-36 digit to it's base-10 value
            if (!char.IsLetterOrDigit(Base36Digit))
            {
                throw new ArgumentOutOfRangeException(Base36Digit.ToString());
            }

            if (char.IsDigit(Base36Digit))
            {
                //Handles 0 - 9
                return byte.Parse(Base36Digit.ToString());
            }
            else
            {
                //Handles A - Z
                return (byte)((int)Base36Digit - 55);
            }
        }


        private static char NumberToBase36Digit(byte NumericValue)
        {
            //Converts a number to it's base-36 value.
            //Only works for numbers <= 35.
            if (NumericValue > 35)
            {
                throw new ArgumentOutOfRangeException(NumericValue.ToString());
            }

            //Numbers:
            if (NumericValue <= 9)
            {
                return NumericValue.ToString()[0];
            }
            else
            {
                //Note that A is code 65, and in this
                //scheme, A = 10.
                return (char)(NumericValue + 55);
            }
        }


        #endregion

        #region Operator Overloads

        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator > (Base36 lhs, Base36 rhs)
        {
            return lhs.numericValue > rhs.numericValue;
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator < (Base36 lhs, Base36 rhs)
        {
            return lhs.numericValue < rhs.numericValue;
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator >=(Base36 lhs, Base36 rhs)
        {
            return lhs.numericValue >= rhs.numericValue;
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <=(Base36 lhs, Base36 rhs)
        {
            return lhs.numericValue <= rhs.numericValue;
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(Base36 lhs, Base36 rhs)
        {
            return lhs.numericValue == rhs.numericValue;
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(Base36 lhs, Base36 rhs)
        {
            return !(lhs == rhs);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Base36 operator +(Base36 lhs, Base36 rhs)
        {
            return new Base36(lhs.numericValue + rhs.numericValue);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Base36 operator -(Base36 lhs, Base36 rhs)
        {
            return new Base36(lhs.numericValue - rhs.numericValue);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Base36 operator ++(Base36 value)
        {
            return new Base36(value.numericValue++);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Base36 operator --(Base36 value)
        {
            return new Base36(value.numericValue--);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Base36 operator *(Base36 lhs, Base36 rhs)
        {
            return new Base36(lhs.numericValue * rhs.numericValue);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Base36 operator /(Base36 lhs, Base36 rhs)
        {
            return new Base36(lhs.numericValue / rhs.numericValue);
        }


        /// <summary>
        /// Operator overload
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static Base36 operator %(Base36 lhs, Base36 rhs)
        {
            return new Base36(lhs.numericValue % rhs.numericValue);
        }


        /// <summary>
        /// Converts type Base36 to a base-10 long
        /// </summary>
        /// <param name="value">The Base36 object</param>
        /// <returns>The base-10 long integer</returns>
        public static implicit operator long(Base36 value)
        {
            return value.numericValue;
        }


        /// <summary>
        /// Converts type Base36 to a base-10 integer
        /// </summary>
        /// <param name="value">The Base36 object</param>
        /// <returns>The base-10 integer</returns>
        public static implicit operator int(Base36 value)
        {
            return (int)value.numericValue;
        }


        /// <summary>
        /// Converts type Base36 to a base-10 short
        /// </summary>
        /// <param name="value">The Base36 object</param>
        /// <returns>The base-10 short</returns>
        public static implicit operator short(Base36 value)
        {
            return (short)value.numericValue;
        }


        /// <summary>
        /// Converts a long (base-10) to a Base36 type
        /// </summary>
        /// <param name="value">The long to convert</param>
        /// <returns>The Base36 object</returns>
        public static implicit operator Base36(long value)
        {
            return new Base36(value);
        }


        /// <summary>
        /// Converts type Base36 to a string; must be explicit, since
        /// Base36 > string is dangerous!
        /// </summary>
        /// <param name="value">The Base36 type</param>
        /// <returns>The string representation</returns>
        public static explicit operator string(Base36 value)
        {
            return value.Value;
        }


        /// <summary>
        /// Converts a string to a Base36
        /// </summary>
        /// <param name="value">The string (must be a Base36 string)</param>
        /// <returns>A Base36 type</returns>
        public static implicit operator Base36(string value)
        {
            return new Base36(value);
        }


        #endregion

        #region Public Override Methods

        /// <summary>
        /// Returns a string representation of the Base36 number
        /// </summary>
        /// <returns>A string representation</returns>
        public override string ToString()
        {
            return Base36.NumberToBase36(numericValue);
        }


        /// <summary>
        /// A unique value representing the value of the number
        /// </summary>
        /// <returns>The unique number</returns>
        public override int GetHashCode()
        {
            return numericValue.GetHashCode();
        }


        /// <summary>
        /// Determines if an object has the same value as the instance
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>True if the values are the same</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Base36))
            {
                return false;
            }
            else
            {
                return this == (Base36)obj;
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a string representation padding the leading edge with
        /// zeros if necessary to make up the number of characters
        /// </summary>
        /// <param name="minimumDigits">The minimum number of digits that the string must contain</param>
        /// <returns>The padded string representation</returns>
        public string ToString(int minimumDigits)
        {
            string base36Value = Base36.NumberToBase36(numericValue);

            if (base36Value.Length >= minimumDigits)
            {
                return base36Value;
            }
            else
            {
                string padding = new string('0', (minimumDigits - base36Value.Length));
                return string.Format("{0}{1}", padding, base36Value);
            }
        }


        #endregion

    }

    /// <summary>
    /// Represents a globally unique identifier (GUID) with a
    /// shorter string value. Sguid
    /// </summary>
    public struct ShortGuid
    {
        #region Static

        /// <summary>
        /// A read-only instance of the ShortGuid class whose value
        /// is guaranteed to be all zeroes.
        /// </summary>
        public static readonly ShortGuid Empty = new ShortGuid(Guid.Empty);

        #endregion

        #region Fields

        Guid _guid;
        string _value;

        #endregion

        #region Contructors

        /// <summary>
        /// Creates a ShortGuid from a base64 encoded string
        /// </summary>
        /// <param name="value">The encoded guid as a
        /// base64 string</param>
        public ShortGuid(string value)
        {
            _value = value;
            _guid = Decode(value);
        }

        /// <summary>
        /// Creates a ShortGuid from a Guid
        /// </summary>
        /// <param name="guid">The Guid to encode</param>
        public ShortGuid(Guid guid)
        {
            _value = Encode(guid);
            _guid = guid;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets/sets the underlying Guid
        /// </summary>
        public Guid Guid
        {
            get { return _guid; }
            set
            {
                if (value != _guid)
                {
                    _guid = value;
                    _value = Encode(value);
                }
            }
        }

        /// <summary>
        /// Gets/sets the underlying base64 encoded string
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                if (value != _value)
                {
                    _value = value;
                    _guid = Decode(value);
                }
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the base64 encoded guid as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _value;
        }

        #endregion

        #region Equals

        /// <summary>
        /// Returns a value indicating whether this instance and a
        /// specified Object represent the same type and value.
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ShortGuid)
                return _guid.Equals(((ShortGuid)obj)._guid);
            if (obj is Guid)
                return _guid.Equals((Guid)obj);
            if (obj is string)
                return _guid.Equals(((ShortGuid)obj)._guid);
            return false;
        }

        #endregion

        #region GetHashCode

        /// <summary>
        /// Returns the HashCode for underlying Guid.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }

        #endregion

        #region NewGuid

        /// <summary>
        /// Initialises a new instance of the ShortGuid class
        /// </summary>
        /// <returns></returns>
        public static ShortGuid NewGuid()
        {
            return new ShortGuid(Guid.NewGuid());
        }

        #endregion

        #region Encode

        /// <summary>
        /// Creates a new instance of a Guid using the string value,
        /// then returns the base64 encoded version of the Guid.
        /// </summary>
        /// <param name="value">An actual Guid string (i.e. not a ShortGuid)</param>
        /// <returns></returns>
        public static string Encode(string value)
        {
            Guid guid = new Guid(value);
            return Encode(guid);
        }

        /// <summary>
        /// Encodes the given Guid as a base64 string that is 22
        /// characters long.
        /// </summary>
        /// <param name="guid">The Guid to encode</param>
        /// <returns></returns>
        public static string Encode(Guid guid)
        {
            string encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded
              .Replace("/", "_");
            return encoded.Substring(0, 22);
        }

        #endregion

        #region Decode

        /// <summary>
        /// Decodes the given base64 string
        /// </summary>
        /// <param name="value">The base64 encoded string of a Guid</param>
        /// <returns>A new Guid</returns>
        public static Guid Decode(string value)
        {
            value = value
              .Replace("_", "/");
            byte[] buffer = Convert.FromBase64String(value + "==");
            return new Guid(buffer);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines if both ShortGuids have the same underlying
        /// Guid value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(ShortGuid x, ShortGuid y)
        {
            if ((object)x == null) return (object)y == null;
            return x._guid == y._guid;
        }

        /// <summary>
        /// Determines if both ShortGuids do not have the
        /// same underlying Guid value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(ShortGuid x, ShortGuid y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Implicitly converts the ShortGuid to it's string equivilent
        /// </summary>
        /// <param name="shortGuid"></param>
        /// <returns></returns>
        public static implicit operator string(ShortGuid shortGuid)
        {
            return shortGuid._value;
        }

        /// <summary>
        /// Implicitly converts the ShortGuid to it's Guid equivilent
        /// </summary>
        /// <param name="shortGuid"></param>
        /// <returns></returns>
        public static implicit operator Guid(ShortGuid shortGuid)
        {
            return shortGuid._guid;
        }

        /// <summary>
        /// Implicitly converts the string to a ShortGuid
        /// </summary>
        /// <param name="shortGuid"></param>
        /// <returns></returns>
        public static implicit operator ShortGuid(string shortGuid)
        {
            return new ShortGuid(shortGuid);
        }

        /// <summary>
        /// Implicitly converts the Guid to a ShortGuid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static implicit operator ShortGuid(Guid guid)
        {
            return new ShortGuid(guid);
        }

        #endregion
    }
}
