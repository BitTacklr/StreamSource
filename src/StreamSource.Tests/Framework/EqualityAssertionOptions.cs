namespace StreamSource.Framework
{
    public class EqualityAssertionOptions
    {
        public static readonly EqualityAssertionOptions EqualsGetHashCode = new EqualityAssertionOptions
        {
            VerifyEqualsOperator = false,
            VerifyNotEqualsOperator = false
        };

        public EqualityAssertionOptions()
        {
            VerifyObjectEquals = true;
            VerifyTypeEquals = true;
            VerifyEqualsOperator = true;
            VerifyNotEqualsOperator = true;
            VerifyGetHashCode = true;
        }

        public bool VerifyObjectEquals { get; set; }

        public bool VerifyTypeEquals { get; set; }

        public bool VerifyEqualsOperator { get; set; }
        
        public bool VerifyNotEqualsOperator { get; set; }

        public bool VerifyGetHashCode { get; set; }
    }
}