using LibrarySystem.Enums;

namespace LibrarySystem.Constants;

public class ResponseStatus
{
    private static readonly Dictionary<int, string> _messages = [];

    public bool Success { get; init; }
    public ResponseCode Code { get; init; }
    public string Message { get; init; }

    private ResponseStatus(bool success, ResponseCode code, string message)
    {
        int codeValue = (int)code;

        if (_messages.ContainsKey(codeValue))
            throw new InvalidOperationException($"Duplicate message code detected: {codeValue}");

        if (_messages.ContainsValue(message))
            throw new InvalidOperationException($"Duplicate message text detected: \"{message}\"");

        _messages[codeValue] = message;

        Success = success;
        Code = code;
        Message = message;
    }

    // Author Messages
    public static readonly ResponseStatus AuthorCreated = new(true, ResponseCode.AuthorCreated, ResponseMessages.AuthorCreated);
    public static readonly ResponseStatus AuthorEmptyPage = new(true, ResponseCode.AuthorEmptyPage, ResponseMessages.AuthorEmptyPage);
    public static readonly ResponseStatus AuthorFetched = new(true, ResponseCode.AuthorFetched, ResponseMessages.AuthorFetched);
    public static readonly ResponseStatus AuthorFetchedMany = new(true, ResponseCode.AuthorFetchedMany, ResponseMessages.AuthorFetchedMany);
    public static readonly ResponseStatus AuthorNotCreated = new(false, ResponseCode.AuthorNotCreated, ResponseMessages.AuthorNotCreated);
    public static readonly ResponseStatus AuthorNotFound = new(false, ResponseCode.AuthorNotFound, ResponseMessages.AuthorNotFound);
    public static readonly ResponseStatus AuthorNotFoundSome = new(false, ResponseCode.AuthorNotFoundSome, ResponseMessages.AuthorNotFoundSome);
    public static readonly ResponseStatus AuthorNotUpdated = new(false, ResponseCode.AuthorNotUpdated, ResponseMessages.AuthorNotUpdated);
    public static readonly ResponseStatus AuthorUpdated = new(true, ResponseCode.AuthorUpdated, ResponseMessages.AuthorUpdated);

    // Book Messages
    public static readonly ResponseStatus BookAuthorsRequired = new(false, ResponseCode.BookAuthorsRequired, ResponseMessages.BookAuthorsRequired);
    public static readonly ResponseStatus BookFetched = new(true, ResponseCode.BookFetched, ResponseMessages.BookFetched);
    public static readonly ResponseStatus BookFetchedMany = new(true, ResponseCode.BookFetchedMany, ResponseMessages.BookFetchedMany);
    public static readonly ResponseStatus BookCreated = new(true, ResponseCode.BookCreated, ResponseMessages.BookCreated);
    public static readonly ResponseStatus BookPageEmpty = new(true, ResponseCode.BookPageEmpty, ResponseMessages.BookPageEmpty);
    public static readonly ResponseStatus BookReleaseDateRequired = new(false, ResponseCode.BookReleaseDateRequired, ResponseMessages.BookReleaseDateRequired);
    public static readonly ResponseStatus NoAuthorBooks = new(true, ResponseCode.NoAuthorBooks, ResponseMessages.NoAuthorBooks);
    public static readonly ResponseStatus BookNotCreated = new(false, ResponseCode.BookNotCreated, ResponseMessages.BookNotCreated);
    public static readonly ResponseStatus BookNotFound = new(false, ResponseCode.BookNotFound, ResponseMessages.BookNotFound);
    public static readonly ResponseStatus BookNotUpdated = new(false, ResponseCode.BookNotUpdated, ResponseMessages.BookNotUpdated);
    public static readonly ResponseStatus BookTitleRequired = new(false, ResponseCode.BookTitleRequired, ResponseMessages.BookTitleRequired);
    public static readonly ResponseStatus BookUpdated = new(true, ResponseCode.BookUpdated, ResponseMessages.BookUpdated);

    // Customer Messages
    public static readonly ResponseStatus CustomerBirthDateRequired = new(false, ResponseCode.CustomerBirthDateRequired, ResponseMessages.CustomerBirthDateRequired);
    public static readonly ResponseStatus CustomerCpfRequired = new(false, ResponseCode.CustomerCpfRequired, ResponseMessages.CustomerCpfRequired);
    public static readonly ResponseStatus CustomerCreated = new(true, ResponseCode.CustomerCreated, ResponseMessages.CustomerCreated);
    public static readonly ResponseStatus CustomerEmptyPage = new(true, ResponseCode.CustomerEmptyPage, ResponseMessages.CustomerEmptyPage);
    public static readonly ResponseStatus CustomerFetched = new(true, ResponseCode.CustomerFetched, ResponseMessages.CustomerFetched);
    public static readonly ResponseStatus CustomerFetchedMany = new(true, ResponseCode.CustomerFetchedMany, ResponseMessages.CustomerFetchedMany);
    public static readonly ResponseStatus InvalidCpf = new(false, ResponseCode.InvalidCpf, ResponseMessages.InvalidCpf);
    public static readonly ResponseStatus CustomerNameRequired = new(false, ResponseCode.CustomerNameRequired, ResponseMessages.CustomerNameRequired);
    public static readonly ResponseStatus CustomerNotCreated = new(false, ResponseCode.CustomerNotCreated, ResponseMessages.CustomerNotCreated);
    public static readonly ResponseStatus CustomerNotFound = new(false, ResponseCode.CustomerNotFound, ResponseMessages.CustomerNotFound);
    public static readonly ResponseStatus CustomerNotUpdated = new(false, ResponseCode.CustomerNotUpdated, ResponseMessages.CustomerNotUpdated);
    public static readonly ResponseStatus CustomerUpdated = new(true, ResponseCode.CustomerUpdated, ResponseMessages.CustomerUpdated);

    // Pagination Messages
    public static readonly ResponseStatus PageIndexOutOfRange = new(false, ResponseCode.PageIndexOutOfRange, ResponseMessages.PageIndexOutOfRange);
    public static readonly ResponseStatus PageSizeOutOfRange = new(false, ResponseCode.PageSizeOutOfRange, ResponseMessages.PageSizeOutOfRange);

    // Request Messages
    public static readonly ResponseStatus RequestInvalidData = new(false, ResponseCode.RequestInvalidData, ResponseMessages.RequestInvalidData);
}
