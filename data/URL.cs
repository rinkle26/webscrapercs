/**
This class will contain information for each URL.
This will be returned from the parser or passed to the parser.
 */
namespace WebScraperModularized.data{
    public class URL{

        public enum URLType : int{
            APARTMENT_URL = 0,
            PROPERTY_URL = 1
        };

        public enum URLStatus : int{
            INITIAL = 0,
            RUNNING = 1,
            DONE = 2,
            ERROR = -1
        }

        public int Id;
        public string Url;

        public URLType Urltype;

        public Property Property;

        public URLStatus Status;
    }
}