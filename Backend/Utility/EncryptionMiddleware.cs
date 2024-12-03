using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Backend.Utility;

public class EncryptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly EncryptionUtil _encryptionService;

    public EncryptionMiddleware(RequestDelegate next, EncryptionUtil encryptionService)
    {
        _next = next;
        _encryptionService = encryptionService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.ContentLength > 0)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var encryptedRequestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (!string.IsNullOrWhiteSpace(encryptedRequestBody))
            {
                var decryptedBody = _encryptionService.Decrypt(encryptedRequestBody);
                var requestBodyBytes = System.Text.Encoding.UTF8.GetBytes(decryptedBody);

                context.Request.Body = new MemoryStream(requestBodyBytes);
                context.Request.ContentType = "application/json";
            }
        }
        
        var originalResponseBody = context.Response.Body;
        using var newResponseBody = new MemoryStream();
        context.Response.Body = newResponseBody;

        await _next(context);

        context.Response.Body = originalResponseBody;

        if (context.Response.StatusCode == 200)
        {
            newResponseBody.Seek(0, SeekOrigin.Begin);
            var plainResponseBody = await new StreamReader(newResponseBody).ReadToEndAsync();

            if (!string.IsNullOrWhiteSpace(plainResponseBody))
            {
                var encryptedResponseBody = _encryptionService.Encrypt(plainResponseBody);
                await context.Response.WriteAsync(encryptedResponseBody);
            }
        }
        else
        {
            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
        }
    }
}
