namespace RedFox.Application.Service.Api;

public interface IUserService
{
    /// <summary>
    ///     Retrieves user data from the remote API as a stream
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to abort the operation</param>
    /// <returns>
    ///     A <see cref="Stream" /> containing the response body in JSON format
    /// </returns>
    /// <exception cref="HttpRequestException">
    ///     Thrown when the HTTP response status code does not indicate success
    /// </exception>
    /// <exception cref="OperationCanceledException">
    ///     Thrown when the operation is canceled via the cancellation token
    /// </exception>
    Task<Stream> GetUsers(CancellationToken cancellationToken);
}