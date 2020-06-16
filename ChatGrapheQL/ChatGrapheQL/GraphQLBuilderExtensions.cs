using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Server;
using GraphQL.Server.Transports.AspNetCore.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GraphQL.Server.Transports.AspNetCore.Common;
using GraphQL.Server.Transports.AspNetCore.SystemTextJson;
using GraphQL.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace ChatGrapheQL
{
    public static class GraphQLBuilderExtensions
    {
        /// <summary>
        /// Adds a <see cref="IGraphQLRequestDeserializer"/> and a <see cref="IDocumentWriter"/>
        /// to the service collection with the provided configuration/settings.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configureDeserializerSettings">
        /// Action to further configure the request deserializer's settings.
        /// Affects reading of the JSON from the HTTP request the middleware processes.
        /// </param>
        /// <param name="configureSerializerSettings">
        /// Action to further configure the response serializer's settings.
        /// Affects JSON returned by the middleware.
        /// </param>
        /// <returns>GraphQL Builder.</returns>
        public static IGraphQLBuilder AddSystemTextJson(this IGraphQLBuilder builder,
            Action<JsonSerializerOptions> configureDeserializerSettings = null,
            Action<JsonSerializerOptions> configureSerializerSettings = null)
        {
            builder.Services.AddSingleton<IGraphQLRequestDeserializer>(p => new GraphQLRequestDeserializer(configureDeserializerSettings));
            builder.Services.AddSingleton<IDocumentWriter>(p => new DocumentWriter(opt =>
            {
                opt.Converters.Add(new OperationMessageConverter());
                configureSerializerSettings?.Invoke(opt);
            }));

            return builder;
        }
    }


    /// <summary>
    /// Implementation of an <see cref="IGraphQLRequestDeserializer"/> that uses System.Text.Json;
    /// reading the request body asynchronously and optimally.
    /// </summary>
    /// <remarks>
    /// With thanks to David Fowler (@davidfowl) for his help getting this right.
    /// </remarks>
    public class GraphQLRequestDeserializer : IGraphQLRequestDeserializer
    {
        private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions();

        public GraphQLRequestDeserializer(Action<JsonSerializerOptions> configure)
        {
            // Add converter that deserializes Variables property
            _serializerOptions.Converters.Add(new ObjectDictionaryConverter());

            configure?.Invoke(_serializerOptions);
        }

        public async Task<GraphQLRequestDeserializationResult> DeserializeFromJsonBodyAsync(HttpRequest httpRequest, CancellationToken cancellationToken = default)
        {
            var bodyReader = httpRequest.BodyReader;

            JsonTokenType jsonTokenType;
            try
            {
                jsonTokenType = await PeekJsonTokenTypeAsync(bodyReader, cancellationToken);
            }
            catch (JsonException)
            {
                // Invalid request content, assign None so it falls through to IsSuccessful = false
                jsonTokenType = JsonTokenType.None;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var result = new GraphQLRequestDeserializationResult { IsSuccessful = true };
            switch (jsonTokenType)
            {
                case JsonTokenType.StartObject:
                    result.Single = ToGraphQLRequest(
                        await JsonSerializer.DeserializeAsync<InternalGraphQLRequest>(bodyReader.AsStream(), _serializerOptions, cancellationToken));
                    return result;
                case JsonTokenType.StartArray:
                    result.Batch = (await JsonSerializer.DeserializeAsync<InternalGraphQLRequest[]>(bodyReader.AsStream(), _serializerOptions, cancellationToken))
                        .Select(ToGraphQLRequest)
                        .ToArray();
                    return result;
                default:
                    result.IsSuccessful = false;
                    return result;
            }
        }

        private static async ValueTask<JsonTokenType> PeekJsonTokenTypeAsync(PipeReader reader, CancellationToken cancellationToken)
        {
            // Separate method so that we can use the ref struct
            static bool DetermineTokenType(in ReadOnlySequence<byte> buffer, out JsonTokenType jsonToken)
            {
                var jsonReader = new Utf8JsonReader(buffer);
                if (jsonReader.Read())
                {
                    jsonToken = jsonReader.TokenType;
                    return true;
                }
                jsonToken = JsonTokenType.None;
                return false;
            }

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var result = await reader.ReadAsync(cancellationToken);

                cancellationToken.ThrowIfCancellationRequested();

                var buffer = result.Buffer;

                if (DetermineTokenType(buffer, out var tokenType))
                {
                    // Don't consume any of the buffer so we can re-parse it with the
                    // serializer
                    reader.AdvanceTo(buffer.Start, buffer.Start);
                    return tokenType;
                }
                else
                {
                    // We don't have enough to read a token, keep buffering
                    reader.AdvanceTo(buffer.Start, buffer.End);
                }

                // If there's no more data coming, then bail
                if (result.IsCompleted)
                {
                    return JsonTokenType.None;
                }
            }
        }

        public Inputs DeserializeInputsFromJson(string json) => json?.ToInputs();

        private static GraphQLRequest ToGraphQLRequest(InternalGraphQLRequest internalGraphQLRequest)
            => new GraphQLRequest
            {
                OperationName = internalGraphQLRequest.OperationName,
                Query = internalGraphQLRequest.Query,
                Inputs = internalGraphQLRequest.Variables?.ToInputs() // must return null if not provided, not an empty dictionary
            };

        public Task<GraphQLRequestDeserializationResult> DeserializeFromJsonBodyAsync(HttpRequest httpRequest, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
