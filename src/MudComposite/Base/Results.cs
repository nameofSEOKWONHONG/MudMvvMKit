namespace MudComposite;

public class Results
{
    /// <summary>
    /// 결과 메세지 목록
    /// </summary>
    public IEnumerable<string> Messages { get; set; }

    /// <summary>
    /// 정합성 체크 결과
    /// </summary>
    public Dictionary<string, string> ValidateResults { get; set; }
        = new Dictionary<string, string>();
    
    /// <summary>
    /// 요청 성공 여부, false라면 ValidateResults에 내역이 있음. empty는  true로 처리함.
    /// </summary>
    public bool Succeeded { get; set; }
}

public class Results<T> : Results
{
    public T Data { get; set; }
}

public class PaginatedResult<T> : Results<T>
{
    public PaginatedResult(List<T> datum)
    {
        Datum = datum;
    }

    public IEnumerable<T> Datum { get; set; }

    internal PaginatedResult(bool succeeded, IEnumerable<T> datum = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
    {
        Datum = datum;
        PageNo = page;
        Succeeded = succeeded;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }

    public static PaginatedResult<T> Fail()
    {
        return Fail(new List<string>() { });
    }
    
    public static PaginatedResult<T> Fail(string message)
    {
        return Fail(new List<string>() { message });
    }

    public static PaginatedResult<T> Fail(List<string> messages)
    {
        return new PaginatedResult<T>(false, default, messages);
    }

    public static Task<PaginatedResult<T>> FailAsync()
    {
        return FailAsync(new List<string>() { });
    }

    public static Task<PaginatedResult<T>> FailAsync(string message)
    {
        return FailAsync(new List<string>() { message });
    }

    public static Task<PaginatedResult<T>> FailAsync(List<string> messages)
    {
        return Task.FromResult(new PaginatedResult<T>(false, default, messages));
    }

    public static PaginatedResult<T> Success(IEnumerable<T> data, int totalCount, int currentPage, int pageSize)
    {
        var result = new PaginatedResult<T>(true, data, null, totalCount, currentPage, pageSize);
        result.Messages = new List<string>() { "Success." };
        return result;
    }

    public static Task<PaginatedResult<T>> SuccessAsync(IEnumerable<T> data, int totalCount, int currentPage,
        int pageSize)
    {
        return Task.FromResult(Success(data, totalCount, currentPage, pageSize));
    }

    public int PageNo { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => PageNo > 1;

    public bool HasNextPage => PageNo < TotalPages;
}