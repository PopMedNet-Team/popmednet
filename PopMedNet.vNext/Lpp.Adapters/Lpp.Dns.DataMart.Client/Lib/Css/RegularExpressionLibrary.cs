using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lpp.Dns.DataMart.Lib.Css
{
    /// <summary>
    /// Common regular Expressions
    /// </summary>
    [Serializable]
    public struct RegularExpressionLibrary
    {
        #region Misc Patterns
        /// <summary>
        /// Matches XML,XHTML
        /// </summary>
        public const String XMLPattern = "</?\\w+((\\s+\\w+(\\s*=\\s*(?:\".*?\"|'.*?'|[^'\">\\s]+))?)+\\s*|\\s*)/?>";

        /// <summary>
        /// Basic Phone Number allowing - . \
        /// </summary>
        public const String PhoneNumber = @"^[0-9\-\.\\]+$";

        /// <summary>
        /// File Extension
        /// </summary>
        public const String FileExtension = @"^((\.[A-Za-z0-9\-_\(\)~]+)+)$";

        /// <summary>
        /// Use to ensure a string is not an injection attack.
        /// validates against:
        /// '
        /// --
        /// </summary>
        /// <remarks>http://www.symantec.com/connect/articles/detection-sql-injection-and-cross-site-scripting-attacks</remarks>        
        public const String SQLInjectionValidation_Basic = @"( /(\%27)|(\')|(\-\-)|(\%23)|(#)/ix)";

        /// <summary>
        /// Matches Standard .Net and SQL Guid
        /// </summary>
        public const String GUIDPattern = @"^([a-zA-Z0-9]{8}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{12})$";

        #endregion

        #region Internet Patterns

        /// <summary>
        /// Matches CSS selectors and returns groups of selector[propertyname:propertyValue]
        /// use to parse CSS in a string or file
        /// http://stackoverflow.com/a/2694121/899290
        /// </summary>
        public const String CSSGroups = @"(?<selector>(?:(?:[^,{]+),?)*?)\{(?:(?<name>[^}:]+):?(?<value>[^};]+);?)*?\}";

        /// <summary>
        /// Regex matching CSS Comments
        /// </summary>
        public const String CSSComments = @"(?<!"")\/\*.+?\*\/(?!"")";

        /// <summary>
        /// Use this RegularExpression to test if an email is in proper format
        /// Set Regular Expression to Case insensitive
        /// Checks if the top level domain is valid
        /// Does not check for valid top level domains only if the pattern is correct.
        /// </summary>
        /// <remarks>http://xyfer.blogspot.com/2005/01/javascript-regexp-email-validator.html</remarks>
        public const String EmailPattern = @"^((""[\w-\s]+"")|([\w-]+(?:\.[\w-]+)*)|(""[\w-\s]+"")([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-zA-Z]{2,6}(?:\.[a-zA-Z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)";

        //@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|edu|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum)\b";
        /// <summary>
        /// Use this Regular Expression to test if an email is in proper format
        /// Set Regular Expression to Case Insensitive
        /// Does not check if domains are valid. Does not allow domains
        /// larger than 4 characters long
        /// </summary>
        public const String EmailGeneralPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

        /// <summary>
        /// Standard Public Url and Some Intranet Urls.
        /// Will match 
        /// </summary>
        public const String URLPattern = @"^((ht|f)tp(s?)\:\/\/|~/|/)?([\w]+:\w+@)?(([a-zA-Z]{1}([\w\-]+\.)+([\w]{2,5}))|([a-zA-Z]{1}([\w\-]+[^\.])+([\w]{2,5})))(:[\d]{1,5})?|((^(25[0-5]|(2[0-4])\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}))((/?\w+/)+|/?)(\w+\.[\w]{3,4})?((\?\w+(=\w+)?)(&\w+(=\w+)?)*)?";

        /// <summary>
        /// HTTP, FTP, HTTPS, FTPS, network Protocols
        /// </summary>
        public const String NetworkProtocolPattern = @"^((ht|f)tp(s?)\:\/\/|~/|/)";

        #endregion

        #region US Patterns
        /// <summary>
        /// US Social Security Number
        /// </summary>
        public const String USSocialSecurityNumberPattern = @"^(([0-9]{3}-[0-9]{2}-[0-9]{4})|([0-9]{9})$)";

        /// <summary>
        /// US Zip Code Pattern
        /// </summary>
        public const String USZipCodePattern = @"^([0-9]{5}|[0-9]{5}\-[0-9]{4}|[0-9]{9})$";

        /// <summary>
        /// US Phone Number Pattern
        /// </summary>
        public const String USPhoneNumber = @"^((([0-9]{1}[\-|\.])?[0-9]{3}[\-|\.][0-9]{3}[\-|\.][0-9]{4})|([0-9]{10,11}))$";
        #endregion

        #region Numerical Patterns
        /// <summary>
        /// Any Kind of Natural Number, Not 00
        /// </summary>
        public const String NaturalNumber = @"^0*[1-9][0-9]*$";

        /// <summary>
        /// Any Integer
        /// </summary>
        public const String IntegerPattern = @"^([-]|[0-9])[0-9]*$";
        /// <summary>
        /// Positive Inegers
        /// </summary>
        public const String IntegerPositivePattern = @"^([0-9])[0-9]*$";
        /// <summary>
        /// Negative Integers
        /// </summary>
        public const String IntegerNegativePattern = @"^-([0-9])[0-9]*$";

        /// <summary>
        /// Real Numbers
        /// </summary>
        public const String RealPattern = @"^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";

        /// <summary>
        /// Positive Real Numbers
        /// </summary>
        public const String RealPositivePattern = @"^([.]|[0-9])[0-9]*[.]*[0-9]+$";
        /// <summary>
        /// Negative Real Numbers
        /// </summary>
        public const String RealNegativePattern = @"^([-]|[-.]|-[0-9])[0-9]*[.]*[0-9]+$";

        /// <summary>
        /// Alpha Only, Float
        /// </summary>
        public const String AlphaPattern = @"[a-zA-Z]";
        /// <summary>
        /// AlphaNumberic, Float
        /// </summary>
        public const String AlphaNumericPattern = @"[a-zA-Z0-9]";
        #endregion

        #region IPAddresses
        /// <summary>
        /// Valid IPv4 Addresses
        /// </summary>
        public const String IPV4AddressPattern = @"^(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}";

        /// <summary>
        /// Valid IPv4 Addresses with a comma delimiter at the end,Max 10 Addresses
        /// </summary>
        public const String IPV4AddressPattern_WComma = @"^((((25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}),?){1,10})$";

        /// <summary>
        /// Matches all IPv6 patterns
        /// </summary>
        public const String IPv6AddressPattern = @"^(((?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4})|(((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)::((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?))|(((?:[0-9A-Fa-f]{1,4}:){6,6})(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3})|(((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?) ::((?:[0-9A-Fa-f]{1,4}:)*)(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}))";

        /// <summary>
        /// Matches all IPv6 patterns
        /// </summary>
        public const String IPv6AddressPattern_WComma = @"^(((?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4})|(((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)::((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?))|(((?:[0-9A-Fa-f]{1,4}:){6,6})(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3})|(((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?) ::((?:[0-9A-Fa-f]{1,4}:)*)(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3})(,$)|($))";

        /// <summary>
        /// IPv6 Full 
        /// </summary>
        public const String IPv6AddressPatternStandard = @"^(?:[0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}";
        /// <summary>
        /// IPv6 Hex Compressed
        /// </summary>
        public const String IPv6AddressPattern_HEXCompressed = @"^((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)::((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?)";
        /// <summary>
        /// IPv6 Hex + 4 dec
        /// </summary>
        public const String IPv6AddressPattern_6Hex4Dec = @"^((?:[0-9A-Fa-f]{1,4}:){6,6})(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}";
        /// <summary>
        /// IPv6 "Mixed"
        /// </summary>
        public const String IPv6AddressPattern_Hex4DecCompressed = @"^((?:[0-9A-Fa-f]{1,4}(?::[0-9A-Fa-f]{1,4})*)?) ::((?:[0-9A-Fa-f]{1,4}:)*)(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}";

        #endregion

        #region SQL ConnectionString Patterns
        /// <summary>
        /// SQL Server 2000/2005 Connection String Pattern
        /// </summary>      
        public const String SQLConnectionStringPattern = @"((((Server)|(Data Source)){1}[=]{1}([A-Za-z0-9\-?_?,?\.?](\\[A-Za-z0-9\-?_?,?\.?])?){1,30};){1})((((Database)|(Initial Catalog)){1}[=]{1}([A-Za-z0-9_?]\s?){1,30};){1})((?=)|(?<=))(((Integrated Security)|(Trusted Connection)){1}[=]{1}((SSPI;)|(true;)|((false;)((((User Id)|(UID))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)(((Password)|(PWD))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)|(((Password)|(PWD))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)(((User Id)|(UID))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)))))|((((User Id)|(UID))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)(((Password)|(PWD))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)|(((Password)|(PWD))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)(((User Id)|(UID))[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};))";
        /// <summary>
        /// SQL Server 2000/2005 Conection String Pattern for Server or Data Source Names Section
        /// </summary>
        public const String SQLConnectionStringServerPattern = @"((((Server)|(Data Source)){1}[=]{1}([A-Za-z0-9\-?_?,?\.?](\\[A-Za-z0-9\-?_?,?\.?])?){1,30};){1})";
        /// <summary>
        /// SQL Server 2000/2005 Conection String Pattern for Datbase or Initial Catalog Names Section
        /// </summary>
        public const String SQLConnectionStringDatabasePattern = @"((((Database)|(Initial Catalog)){1}[=]{1}([A-Za-z0-9_?]\s?){1,30};){1})";
        /// <summary>
        /// SQL Server 2000/2005 Conection String Pattern for Security Settings Section
        /// </summary>
        public const String SQLConnectionStringSecurityTypePattern = @"((?=)|(?<=))(((Integrated Security)|(Trusted Connection)){1}[=]{1}((SSPI;)|(true;)|((false;)(((User ID)|(UID)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)((Password)|(PWD)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)|((Password)|(PWD)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)((User Id)|(UID)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)))))";
        /// <summary>
        /// SQL Server 2000/2005 Conection String Pattern for User ID and password Section
        /// </summary>
        public const String SQLConnecitonStringLoginInfoPattern = @"(((User ID)|(UID)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)((Password)|(PWD)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)|((Password)|(PWD)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};)((User Id)|(UID)[=]{1}[A-Za-z0-9\-_\.!#\$%&=\+\[\]\{\}\|\<\>\']{1,30};))";
        #endregion

        #region Credit Card Patterns
        /// <summary>
        /// All Major Credit Cards Pattern
        /// </summary>
        public const String MajorCreditCardPattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6011[0-9]{12}|3(?:0[0-5]|[68][0-9])[0-9]{11}|3[47][0-9]{13})$";
        /// <summary>
        /// American Express Credit Card
        /// </summary>
        public const String AmericanExpressCreditCardPattern = @"^3[47][0-9]{13}$";
        /// <summary>
        /// Diners Club Credit Card
        /// </summary>
        public const String DinersClubCreditCardPattern = @"^3(?:0[0-5]|[68][0-9])[0-9]{11}$";
        /// <summary>
        /// Discover Card Credit Card
        /// </summary>
        public const String DiscoverCreditCardPattern = @"^6011[0-9]{12}$";
        /// <summary>
        /// Master Card Credit Card
        /// </summary>
        public const String MasterCardCreditCardPattern = @"^5[1-5][0-9]{14}$";
        /// <summary>
        /// Visa Credit Card Pattern, Long and Short
        /// </summary>
        public const String VisaCreditCardPattern = @"^4[0-9]{12}(?:[0-9]{3})?$";

        #endregion

    }
}
