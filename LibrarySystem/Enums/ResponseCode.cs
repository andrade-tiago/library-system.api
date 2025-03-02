namespace LibrarySystem.Enums;

public enum ResponseCode
{
    // Author
    AuthorCreated = 1000,
    AuthorEmptyPage = 1001,
    AuthorFetched = 1002,
    AuthorFetchedMany = 1003,
    AuthorNotCreated = 1004,
    AuthorNotFound = 1005,
    AuthorNotFoundSome = 1006,
    AuthorNotUpdated = 1007,
    AuthorUpdated = 1008,

    // Book
    BookAuthorsRequired = 2000,
    BookFetched = 2001,
    BookFetchedMany = 2002,
    BookCreated = 2003,
    BookPageEmpty = 2004,
    BookReleaseDateRequired = 2005,
    NoAuthorBooks = 2006,
    BookNotCreated = 2007,
    BookNotFound = 2008,
    BookNotUpdated = 2009,
    BookTitleRequired = 2010,
    BookUpdated = 2011,

    // Customer
    CustomerBirthDateRequired = 3000,
    CustomerCpfRequired = 3001,
    CustomerCreated = 3002,
    CustomerEmptyPage = 3003,
    CustomerFetched = 3004,
    CustomerFetchedMany = 3005,
    InvalidCpf = 3006,
    CustomerNameRequired = 3007,
    CustomerNotCreated = 3008,
    CustomerNotFound = 3009,
    CustomerNotUpdated = 3010,
    CustomerUpdated = 3011,

    // Pagination
    PageIndexOutOfRange = 4000,
    PageSizeOutOfRange = 4001,

    // Request
    RequestInvalidData = 5000,
}
